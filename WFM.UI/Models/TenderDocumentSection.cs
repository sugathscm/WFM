using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFM.UI.Models
{
    public class TenderDocumentSection
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int ParentId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}