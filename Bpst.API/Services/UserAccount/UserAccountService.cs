using Bpst.API.DB;
using Bpst.API.DbModels;
using Bpst.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Bpst.API.Services.UserAccount
{
    public class UserAccountService(AppDbContext context, IConfiguration config) : IUserAccountService
    {
        private readonly AppDbContext _context = context;
        private readonly IConfiguration _config = config;
        public async Task<LoginResponse> Login(string LoginName, string Password)
        {
            LoginResponse response = new LoginResponse();

            var _appUser = await GetUserByEmail(LoginName);
            if (_appUser != null)
            {

                response = await ValidateCredentials(LoginName, Password);
                if (response.IsLoginSuccess)
                {
                    await PopulateLoginResponse(response, LoginName);
                }
                else
                {
                    response.IsLoginSuccess = false;
                    response.ErrorMessages ??= new List<string>() { };
                    response.ErrorMessages.Add("Invalid Credentials");
                    // response.ErrorMessages.Add("You entered a valid credentials but your user status is InActive, i.e. you are not allowed to login, please reach to admin");
                    return response;
                }
            }

            else response.ErrorMessages = new List<string>() { "User not Registerd in the Portal" };
            return response;
        }
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
            UserRegistrationResponse response = new UserRegistrationResponse() { };

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
        private async Task<string> CreateJwtToken(string email)
        {

            var secretKey = _config["JwtSettings:Key"];
            var validIssuer = _config["JwtSettings:Issuer"];
            var validAudience = _config["JwtSettings:Audience"];

            var _appUser = await GetUserByEmail(email);
            //var userClaims = _appUser.Roles.Split(",").Select(r => new Claim(ClaimTypes.Role, r)).ToList();
            //userClaims.Add(new Claim(ClaimTypes.Name, _appUser.EMAILID));
            //userClaims.Add(new Claim(ClaimTypes.Email, _appUser.EMAILID));
            //userClaims.Add(new Claim(ClaimTypes.NameIdentifier, _appUser.Id.ToString()));
            //userClaims.Add(new Claim(ClaimTypes.Sid, _appUser.Login));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                //   claims: userClaims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred,
                issuer: validIssuer,
                audience: validAudience
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
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
            response.Token = await CreateJwtToken(loginName);
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
