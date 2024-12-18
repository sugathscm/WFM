using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public class LoginAuditViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string DateLogged { get; set; }

        public List<UsersInRoleViewModel> UsersInRoleViewModels { get; set; }
    }

}