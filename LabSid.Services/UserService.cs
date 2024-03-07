using LabSid.Infra.Interfaces;
using LabSid.Models;
using LabSid.Models.Interfaces;
using LabSid.Services.Auth;
using LabSid.Services.DTO;
using LabSid.Services.Interfaces;

namespace LabSid.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<UserDto> GetByIdAsync(long id)
        {
            var user = await this._userRepository.GetByIdAsync(id);
            if (user == null) 
                return null; 
            return new UserDto(user);
        }

        public async Task<LoginDto> Login(string email, string password)
        {
            try
            {
                var user = new UserDto(await this._userRepository.GetByEmail(email));

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                bool matches = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (!matches)
                {
                    throw new Exception("Password is incorrect.");
                }

                var token = TokenService.GenerateToken(user.Email, user.Id.ToString());

                return new LoginDto()
                {
                    token = token,
                    email = user.Email,                    
                    id = user.Id.ToString(),
                };
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<RefreshTokenDto> RefreshToken(long id, string token)
        {
            try
            {
                var user = await this._userRepository.GetByIdAsync(id);

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var refresh_token = TokenService.RefreshToken(user.Email, user.Id, token);

                return refresh_token;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<UserDto> SaveAsync(IUser user)
        {
            return new UserDto(await this._userRepository.SaveAsync(user));
        }
    }
}