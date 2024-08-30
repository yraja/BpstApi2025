using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bpst.API.DbModels
{
    public class AccountDetails
    {
        [Key]
        [Display(Name = "Unique ID")]
        public int UniqueId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string AccountNo { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }


    }
}
