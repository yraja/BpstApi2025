using Bpst.API.DbModels;
using Bpst.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Bpst.API.Services.UserAccount
{
    public interface IUserAccountService
    {
        public Task<bool> IfUserExists(string email);
        public Task<User> GetUserByEmail(string email);
        public Task<UserRegistrationResponse> RegisterNewUserAsync(UserRegistrationVM user);
        public Task<LoginResponse> Login(LoginVM login);
        public Task<DefaultApiResponse> AddRoles(int userId, List<string> roles);
    }
}
