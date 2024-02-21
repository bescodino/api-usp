using LabSid.Models.Interfaces;

namespace LabSid.Models
{
    public class User : IUser
    {
        public User() { }

        public User(IUser user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Password = user.Password;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
