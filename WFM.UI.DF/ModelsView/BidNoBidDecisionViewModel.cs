using System;

namespace WFM.UI.DF.ModelsView
{
    public class BidNoBidDecisionViewModel
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> StrategicFit { get; set; }
        public Nullable<int> WinProbability { get; set; }
        public Nullable<int> TechCapability { get; set; }
        public Nullable<int> Resource { get; set; }
        public Nullable<int> Profitability { get; set; }
        public Nullable<int> ClientRelationship { get; set; }
        public Nullable<int> Risks { get; set; }
        public Nullable<int> Total { get; set; }
        public string Decision { get; set; }
        public string Notes { get; set; }

        // List view (Project Management > Bid/No-Bid List) display fields
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }
}
