using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public class ReportsViewModel
    {
        [Required]
        [Display(Name = "Report Type")]
        public int? ReportTypeId { get; set; }
        public string ReportType { get; set; }

        [Required]
        [Display(Name = "From Date (mm/dd/yyyy)")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FromDate { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "To Date (mm/dd/yyyy)")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ToDate { get; set; } = DateTime.Today;

        public int? ProjectId { get; set; }
        public int? MeetingId { get; set; }
        public int? AssigneeId { get; set; }
        public int? StatusId { get; set; }
        public int? PriorityId { get; set; }
        public int? DateTypeId { get; set; }
    }

}