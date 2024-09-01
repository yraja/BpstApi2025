using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bpst.API.DB;
using Bpst.API.DbModels;
using Bpst.API.Services.UserAccount;
using Bpst.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Bpst.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController(AppDbContext context, IUserAccountService userService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly IUserAccountService _userService = userService;


        [AllowAnonymous]
        [HttpPost("UserRegistration")]
        public async Task<ActionResult<UserRegistrationResponse>> PostUser(UserRegistrationVM user)
        {
            var result = await _userService.RegisterNewUserAsync(user);
            return result;
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginVM login)
        {
            var result = await _userService.Login(login);
            return result;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("AddRoles")]
        public async Task<ActionResult<DefaultApiResponse>> AddRoles(int userId, List<string> roles)
        {
            var result = await _userService.AddRoles(userId, roles);
            return result;
        }
    }
}
