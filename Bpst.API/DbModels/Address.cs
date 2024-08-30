using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Bpst.API.DbModels
{
    public class Address
    {
        [Key]
        [Display(Name = "Unique ID")]
        public int UniqueId { get; set; }
        public string? AddressLine1 {  get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? City {  get; set; }
        public string? State {  get; set; }
        public string? Country {  get; set; }
        public string? Pincode {  get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
