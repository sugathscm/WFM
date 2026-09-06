using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.Models
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string ContactNumbers { get; set; }
        public string Email { get; set; }
        public string FBProfile { get; set; }
        public string LIProfile { get; set; }
        public string TTProfile { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}