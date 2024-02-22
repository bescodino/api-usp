using LabSid.Models;

namespace LabSid.Api.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

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
