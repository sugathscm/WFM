using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFM.UI.Models
{
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
    }
}