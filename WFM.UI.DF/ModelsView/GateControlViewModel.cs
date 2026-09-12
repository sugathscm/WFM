using System;

namespace WFM.UI.DF.ModelsView
{
    public class GateControlViewModel
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<bool> QG1Bid { get; set; }
        public Nullable<bool> QG2Kickoff { get; set; }
        public Nullable<bool> QG3Fifty { get; set; }
        public Nullable<bool> QG4Ninety { get; set; }
        public Nullable<bool> QG5Approve { get; set; }
        public Nullable<bool> QG6Submit { get; set; }
        public Nullable<int> GatesDone { get; set; }
        public Nullable<int> PercentComplete { get; set; }
        public string CurrentStage { get; set; }

        // List view (Project Management > Gate Control List) display fields
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }
}
