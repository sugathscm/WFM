using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public class TenderDocumentSectionView
    {
        public int Id { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}