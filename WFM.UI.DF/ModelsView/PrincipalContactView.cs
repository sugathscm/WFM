using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WFM.DAL;

namespace WFM.UI.ModelsView
{
    public class PrincipalContactView
    {
        //Contact Person Name, Contact Person Mobile, Contact Person Fixed Line, Designation

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Mobile { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string FixedLine { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
        public int DesignationId { get; set; }
        public int PrincipalId { get; set; }
        public Designation Designation { get; set; }
        public Principal Principal { get; set; }

    }

}