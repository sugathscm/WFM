using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public partial class Form12BViewModel
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string NameOfRequestingParty { get; set; }
        public Nullable<int> DivisionId { get; set; }
        public Nullable<System.DateTime> DateOfRequest { get; set; }
        public string ShortDescriptionOfTheReasonForPayment { get; set; }
        public string ScopeOfWork { get; set; }
        public string ReceivedAndApprovedQuotation { get; set; }
        public string InvoiceReferenceNo { get; set; }
        public string RecommendationByDivisionalHead { get; set; }
        public string SDMailTrail { get; set; }
        public string SDApprovedQuotation { get; set; }
        public string SDAgreedScope { get; set; }
        public string ApprovedAndCertifiedInvoiceAttached { get; set; }
        public Nullable<System.DateTime> FormDate { get; set; }
    }
}