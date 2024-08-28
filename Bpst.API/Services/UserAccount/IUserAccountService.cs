using Bpst.API.DbModels;
using Bpst.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Bpst.API.Services.UserAccount
{
    public interface IUserAccountService
    {
        public Task<LoginResponse> Login(string LoginName, string Password);
        public Task<bool> IfUserExists(string email);
        public Task<User> GetUserByEmail(string email);
        public Task<UserRegistrationResponse> RegisterNewUser(UserRegistrationVM user);
    }
}
