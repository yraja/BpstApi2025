using System.ComponentModel.DataAnnotations;

namespace Bpst.API.DbModels
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
