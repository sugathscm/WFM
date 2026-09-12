using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public class TaskTrackerDocument
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string DocumentName { get; set; }

        public string DocumentPath { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}