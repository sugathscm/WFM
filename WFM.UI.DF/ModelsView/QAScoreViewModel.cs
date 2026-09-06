using System;

namespace WFM.UI.DF.ModelsView
{
    public class QAScoreViewModel
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> RfpTorCompliance { get; set; }
        public Nullable<int> MethodologyApproach { get; set; }
        public Nullable<int> KeyExpertsCv { get; set; }
        public Nullable<int> RelevantExperience { get; set; }
        public Nullable<int> WorkPlanDeliverables { get; set; }
        public Nullable<int> MandatoryAttachments { get; set; }
        public Nullable<int> FinancialCompetitiveness { get; set; }
        public Nullable<int> ProposalQuality { get; set; }
        public Nullable<int> FinalQcSubmissionReadiness { get; set; }
        public Nullable<int> Total { get; set; }
        public string Decision { get; set; }
        public string Notes { get; set; }

        // List view (Project Management > QA Score List) display fields
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }
}
