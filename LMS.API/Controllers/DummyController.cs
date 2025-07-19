using LMS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DummyController : ControllerBase
    {
        [Authorize(Roles ="Teacher")]
        [HttpGet("check")]
        public IActionResult CheckAuthorization()
        {
            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(teacherId))
            {
                return BadRequest(new
                {
                    statusCode = 400,
                    message = "Invalid token",
                    data = (string?)null
                });
            }

            return Ok(new
            {
                statusCode = 200,
                message = "✅ You are authorized as Teacher",
                data = new { teacherId, role }
            });
        }
        [Authorize(Roles ="Student")]
        [HttpGet("check-here-students")]
        public IActionResult TestAuth()
        {
            return Ok("You are authorized");
        }
    }
}
