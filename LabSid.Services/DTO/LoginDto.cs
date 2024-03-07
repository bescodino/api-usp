using LabSid.Models.Interfaces;

namespace LabSid.Services.DTO
{
    public class LoginDto
    {
        public string email{ get; set; }
        public string token { get; set; }
        public string id { get; set; }

    }
}
