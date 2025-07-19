using Dapper;
using LMS.API.Data;
using LMS.API.Dtos;
using LMS.API.DTOs;
using LMS.API.Models;
using MySql.Data.MySqlClient;

namespace LMS.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DapperContext _context;

        public AuthRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            using var connection = (MySqlConnection)_context.CreateConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // 1. Insert into Users
                var insertUserQuery = @"INSERT INTO Users (FullName, Email, PasswordHash)
                                VALUES (@FullName, @Email, @PasswordHash);
                                SELECT LAST_INSERT_ID();";

                var userId = await connection.ExecuteScalarAsync<int>(insertUserQuery,
                    new
                    {
                        request.FullName,
                        request.Email,
                        PasswordHash = passwordHash
                    }, transaction);

                // 2. Get RoleId
                var roleId = await connection.ExecuteScalarAsync<int>(
                    "SELECT RoleId FROM Roles WHERE RoleName = @Role",
                    new { Role = request.Role }, transaction);

                // 3. Map UserId to RoleId
                await connection.ExecuteAsync(
                    "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)",
                    new { UserId = userId, RoleId = roleId }, transaction);

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }


        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<string?> GetUserRoleAsync(int userId)
        {
            var query = @"
                SELECT r.RoleName FROM Roles r
                INNER JOIN UserRoles ur ON r.RoleId = ur.RoleId
                WHERE ur.UserId = @UserId
            ";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<string>(query, new { UserId = userId });
        }
    }
}
