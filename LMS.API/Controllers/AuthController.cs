using LMS.API.Dtos;
using LMS.API.DTOs;
using LMS.API.Helpers;
using LMS.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            _authRepo = authRepo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var existingUser = await _authRepo.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict(ResponseHelper<string>.Fail("User already exists with this email.", 409));
            }

            var success = await _authRepo.RegisterAsync(request);
            if (!success)
            {
                return StatusCode(500, ResponseHelper<string>.Fail("Something went wrong while creating the user.", 500));
            }

            return StatusCode(201, ResponseHelper<string>.Success(null, "User registered successfully", 201));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _authRepo.GetUserByEmailAsync(loginRequestDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.PasswordHash))
            {
                return Unauthorized(ResponseHelper<string>.Fail("Invalid email or password"));
            }

            var role = await _authRepo.GetUserRoleAsync(user.UserId);

            if (string.IsNullOrEmpty(role))
            {
                return StatusCode(500, ResponseHelper<string>.Fail("User role not found."));
            }

            var token = JwtHelper.GenerateToken(user.UserId, user.Email, role, _config); // ✅


            var response = new AuthResponseDto
            {
                Token = token,
                Role = role,
                Email = user.Email,
                FullName = user.FullName
            };

            return Ok(ResponseHelper<AuthResponseDto>.Success(response, "Login successful"));
        }

    }
}
