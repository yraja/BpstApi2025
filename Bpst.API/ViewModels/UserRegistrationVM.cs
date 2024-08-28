using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Bpst.API.ViewModels
{
    public class UserRegistrationVM
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastLame { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(4)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;


        [BindNever]

        public string ErrorMessage { get; set; } = string.Empty;
        [BindNever]
        public string SuccessMessage { get; set; } = string.Empty;
        [SwaggerSchema(ReadOnly = true)]

        public string NotificationMessage { get; set; } = string.Empty;
    }
}
