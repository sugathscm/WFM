using System;

namespace WFM.UI.DF.ModelsView
{
    public class BidRecommendationViewModel
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string ConsortiumJV { get; set; }
        public string Source { get; set; }
        public string StrategicFit { get; set; }
        public Nullable<bool> Eligible { get; set; }
        public Nullable<int> RecommendStatusId { get; set; }

        // List view (Project Management > Bid Recommendation List) display fields
        public string RecommendStatusName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }
}
