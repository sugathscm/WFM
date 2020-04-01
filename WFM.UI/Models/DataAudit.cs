using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WFM.UI.Models
{
    public class DataAudit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public string Entity { get; set; }
        public string OldData { get; set; }
        public string NewData { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}