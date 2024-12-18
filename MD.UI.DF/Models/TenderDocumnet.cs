using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.Models
{
    public class TenderDocumnet
    {
        public string DocumentTypeId { get; set; }
        public HttpPostedFileBase PostedFile { get; set; }

        public string DocumentPath { get; set; }
        public string Name { get; set; }
    }
}