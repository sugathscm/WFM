using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WFM.DAL;

namespace WFM.UI.ModelsView
{
    public class PrincipalView
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string AddressLine1 { get; set; }
        [MaxLength(50)]
        public string AddressLine2 { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Postcode { get; set; }
        [MaxLength(50)]
        public string Province { get; set; }
        public int CountryId { get; set; }
        [MaxLength(50)]
        public string Website { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        public Country Country { get; set; }

        public ICollection<Contact> PrincipalContacts { get; set; }
    }
}