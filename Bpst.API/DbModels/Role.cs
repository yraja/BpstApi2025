using System.ComponentModel.DataAnnotations;

namespace Bpst.API.DbModels
{
    public class Role
    {
        [Key]
        [Display(Name = "Unique ID")] 
        public int UniqueId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
