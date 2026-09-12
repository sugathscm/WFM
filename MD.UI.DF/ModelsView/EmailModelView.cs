using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public class EmailModelView
    {
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attachment { get; set; }
    }
}