using LMS.API.Dtos;
using LMS.API.DTOs;
using LMS.API.Models;

namespace LMS.API.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> RegisterAsync(RegisterRequestDto request);
        Task<User?> GetUserByEmailAsync(string email);
        Task<string?> GetUserRoleAsync(int userId);
    }
}
