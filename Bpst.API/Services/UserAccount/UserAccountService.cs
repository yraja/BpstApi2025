using Bpst.API.DB;
using Bpst.API.DbModels;
using Bpst.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bpst.API.Services.UserAccount
{
    public class UserAccountService(AppDbContext context, IConfiguration config) : IUserAccountService
    {
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _config = config;
        public async Task<bool> IfUserExists(string email)
        {
            var user = await _context.AppUsers.AnyAsync(u => u.LoginEmail.Equals(email));
            return user;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                return await _context.AppUsers.Where(u => u.LoginEmail.Equals(email)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
            return null;
        }
        public async Task<UserRegistrationResponse> RegisterNewUserAsync(UserRegistrationVM user)
        {
            var response = new UserRegistrationResponse() { };

            if (await IfUserExists(user.Email))
            {
                response.ErrorMessages = new List<string>() { "User already exist with given Id, please try with different email" };
                return response;
            }
            else
            {
                var appUser = new User()
                {
                    CreatedDate = DateTime.Now,
                    LoginEmail = user.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    FirstName = user.FirstName,
                    LastName = user.LastLame,
                    Mobile = user.PhoneNumber,
                    Roles = user.Roles,
                };
                _context.AppUsers.Add(appUser);
                try
                {
                    await _context.SaveChangesAsync();
                    string msg = "Dear User,\r<br>\r<br>Welcome    ... ";
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                    if (response.ErrorMessages == null)
                        response.ErrorMessages = new List<string>();
                    response.ErrorMessages.Add(ex.Message);
                    response.ErrorMessages.Add(ex.InnerException.Message);
                    response.ErrorMessages.Add(ex.StackTrace);
                    return response;
                }
                response.UniqueId = appUser.UniqueId;
                response.IsCreated = true;
                return response;
            }
        }
        public async Task<LoginResponse> Login(LoginVM login)
        {
            var response = new LoginResponse();
            var _appUser = await GetUserByEmail(login.LoginName);
            if (_appUser != null)
            {
                response = await ValidateCredentials(login.LoginName, login.Password);
                if (response.IsLoginSuccess)
                    await PopulateLoginResponse(response, login.LoginName);
                else
                {
                    response.IsLoginSuccess = false;
                    response.ErrorMessages ??= [];
                    response.ErrorMessages.Add("Invalid Credentials");
                    return response;
                }
            }
            else response.ErrorMessages = ["User not Registerd in the Portal"];
            return response;
        }
        public async Task<DefaultApiResponse> AddRoles(int userId, List<string> roles)
        {
            var result = new DefaultApiResponse() { };
            var dbRoles = await _context.Roles.Where(r => roles.Contains(r.RoleName)).ToListAsync();
            foreach (var role in dbRoles)
                await _context.UserRoles.AddAsync(new UserRole() { UserId = userId, RoleId = role.UniqueId });
            await _context.SaveChangesAsync();
            result.IsSuccess = true;
            result.SuccessMessages = new List<string>() { "Roles Are Saved Successfully" };
            return result;
        }

        private string CreateJwtToken(User _appUser)
        {
            var userClaims = _appUser.Roles?.Select(r => new Claim(ClaimTypes.Role, r)).ToList() ?? [];

            userClaims.Add(new Claim(ClaimTypes.Name, _appUser.FullName));
            userClaims.Add(new Claim(ClaimTypes.Email, _appUser.LoginEmail));
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, _appUser.UniqueId.ToString()));

            var token = new JwtSecurityToken(
              claims: userClaims,
              expires: DateTime.Now.AddHours(2),
              signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"])), SecurityAlgorithms.HmacSha256Signature),
              issuer: _config["JwtSettings:Issuer"],
              audience: _config["JwtSettings:Audience"]
              );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<LoginResponse> ValidateCredentials(string email, string password)
        {
            var response = new LoginResponse() { };
            var _appUser = await GetUserByEmail(email);
            response.IsLoginSuccess = _appUser == null ? false
                 : BCrypt.Net.BCrypt.Verify(password, _appUser.PasswordHash);
            return response;
        }
        private async Task PopulateLoginResponse(LoginResponse response, string loginName)
        {
            var _appUser = await GetUserByEmail(loginName);
            response.IsLoginSuccess = true;
            response.FName = _appUser.FirstName;
            response.LName = _appUser.LastName;
            response.Email = _appUser.LoginEmail;
            response.Mobile = _appUser.Mobile;
            response.UserId = _appUser.UniqueId.ToString();
            response.Token = CreateJwtToken(_appUser);
            response.userRoles = await GetUserRole(_appUser.LoginEmail);
        }
        private async Task<List<Role>?> GetUserRole(string loginEmail)
        {
            var roles = await (from user in _context.AppUsers
                               join userRole in _context.UserRoles on user.UniqueId equals userRole.UserId
                               join role in _context.Roles on userRole.RoleId equals role.UniqueId
                               where user.LoginEmail == loginEmail
                               select role).ToListAsync();

            return roles;
        }

    }
}
