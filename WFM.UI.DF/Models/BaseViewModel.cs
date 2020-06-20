using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel.DataAnnotations;

namespace WFM.UI.DF.Models
{
    public class BaseViewModel
    {
        public int Id { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "White space found in Name field.")]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }
}