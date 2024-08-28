using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bpst.API.DbModels
{
    public class User
    {

        [Key]
        [Display(Name = "Unique ID")]

        public int UniqueId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;


        [ReadOnly(true)]
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
         


        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string LoginEmail { get; set; } = string.Empty;


        public string PasswordHash { get; internal set; } = string.Empty;
        public string Mobile { get; internal set; } = string.Empty;
        public List<UserRole>? Roles { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
