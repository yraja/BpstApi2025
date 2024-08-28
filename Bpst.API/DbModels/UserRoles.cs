using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bpst.API.DbModels
{
    public class UserRole
    {
        [Key]
        [Display(Name = "Unique ID")]
        public int UniqueId { get; set; }
         
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }


        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
