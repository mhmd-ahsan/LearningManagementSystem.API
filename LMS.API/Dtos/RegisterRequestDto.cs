using System.ComponentModel.DataAnnotations;

namespace LMS.API.Dtos
{
        public class RegisterRequestDto
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }  // ✅ this must be sent
        }

}
