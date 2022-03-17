using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ZemogaTest.Repositories.Repositories;
using ZemogaTest.Services.Interfaces;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Entities;
using ZemogaTest.Utilities.Helpers;
using ZemogaTest.Utilities.Payloads;

namespace ZemogaTest.Sevices.Users
{
    public class UserService : IUserService
    {

        private readonly IBlogEngineRepository<User> _userRepository;
        private readonly AppSettingsDto _appSettings;
        public UserService(IBlogEngineRepository<User> spaRepository, IOptions<AppSettingsDto> appSettings)
        {
            _userRepository = spaRepository;
            _appSettings = appSettings.Value;

        }

        public UserDto Authenticate(UserPayload userPayload)
        {
            try
            {
                var user = this.GetUser(userPayload);

                if (user == null)
                    return null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.SecureKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                return user.WithoutPassword();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private UserDto GetUser(UserPayload userPayload)
        {
            var user = _userRepository.GetAll().FirstOrDefault(x => x.UserName == userPayload.UserName && x.Password == userPayload.PassWord);
            if (user == null)
                return null;

            return new UserDto
            {
                Id = user.UserId,
                Username = user.UserName,
                Password = user.Password,
                FullName = user.FullName,
                RoleId = user.RolId
            };
        }
    }
}
