using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSid.Services.DTO
{
    public class RefreshTokenDto
    {
        public string Email { get; set; }
        public int Id { get; set; }
        public string RefreshToken { get; set; }
    }

}
