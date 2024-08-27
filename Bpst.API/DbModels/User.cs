using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bpst.API.DbModels
{
    public class User
    {
        [Key]
        [Display(Name = "Unique ID")]
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastLame { get; set; } = string.Empty;

        [ReadOnly(true)]
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastLame; } }

        public List<UserRole> Roles { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }


    }
}
