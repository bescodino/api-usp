using LabSid.Models.Interfaces;

namespace LabSid.Services.DTO
{
    public class LoginDto
    {
        public UserDto user { get; set; }
        public string token { get; set; }

    }
}
