using System;

namespace WFM.UI.DF.ModelsView
{
    public class ProposalOutcomeViewModel
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string Outcome { get; set; }
        public Nullable<DateTime> ResultDate { get; set; }
        public string WinningConsultant { get; set; }
        public Nullable<int> TechnicalScore { get; set; }
        public Nullable<int> FinancialScore { get; set; }
        public Nullable<int> TotalScore { get; set; }
        public string ReasonForLoss { get; set; }
        public string LessonLearned { get; set; }
        public string FollowUpAction { get; set; }
        public string ActionOwner { get; set; }

        // List view display fields
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
    }
}
