using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public class DataAuditViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Entity { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public string UpdatedOn { get; set; }

        public List<UsersInRoleViewModel> UsersInRoleViewModels { get; set; }
    }

}