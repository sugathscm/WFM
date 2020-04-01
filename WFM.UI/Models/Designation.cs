using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFM.UI.Models
{
    public class Designation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public ICollection<PrincipalContact> PrincipalContacts { get; set; }
    }
}