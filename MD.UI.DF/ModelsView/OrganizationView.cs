using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WFM.DAL;

namespace WFM.UI.DF.ModelsView
{
    public class OrganizationView
    {
        //Contact Person Name, Contact Person Mobile, Contact Person Fixed Line, Designation

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string AddressLine1 { get; set; }
        [Required, MaxLength(100)]
        public string AddressLine2 { get; set; }
        [Required, MaxLength(100)]
        public string City { get; set; }
        [Required, MaxLength(100)]
        public string PostCode { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string FixedLine { get; set; }
        [MaxLength(50)]
        public string LandLine { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
        public Designation Designation { get; set; }
    }
}