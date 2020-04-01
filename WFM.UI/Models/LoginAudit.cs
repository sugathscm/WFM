using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WFM.UI.Models
{
    public class LoginAudit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public string IPAddress { get; set; }
        public Nullable<System.DateTime> DateLogged { get; set; }
    }
}