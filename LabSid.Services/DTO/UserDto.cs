using LabSid.Models;
using LabSid.Models.Interfaces;

namespace LabSid.Services.DTO
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserDto(IUser user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Password = user.Password;
        }

        public User ToEntity()
        {
            return new User
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Password = this.Password
            };
        }
    }
}
