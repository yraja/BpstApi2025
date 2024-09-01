using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bpst.API.DbModels
{
    public class Qualification
    {
        [Key]
        [Display(Name = "Unique ID")]
        public int UniqueId { get; set; }
        public string? DegreeName {  get; set; }
        public string? StartDate {  get; set; }
        public string? EndDate { get; set;}
        public string? Percenatge {  get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

    }
}
