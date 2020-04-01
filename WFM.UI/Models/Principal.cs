using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFM.UI.Models
{
    public class Principal
    {
        //Name, Address Line 1, Address Line 2, City, Postcode, Province/State, Country, Website, Email

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

        public ICollection<PrincipalContact> PrincipalContacts { get; set; }
    }
}