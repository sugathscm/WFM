using System.Collections.Generic;

namespace WFM.UI.DF.ModelsView
{
    public class ManagementDashboardViewModel
    {
        public int ActiveOpportunities { get; set; }
        public int ActiveProposals { get; set; }
        public decimal? WinRate90d { get; set; }
        public int OngoingProjects { get; set; }
        public int OverdueCount { get; set; }
        public decimal? AvgBidScore { get; set; }

        public int FunnelOpportunity { get; set; }
        public int FunnelGoDecision { get; set; }
        public int FunnelProposal { get; set; }
        public int FunnelShortlisted { get; set; }
        public int FunnelWon { get; set; }

        public int HealthOnTrack { get; set; }
        public int HealthAttention { get; set; }
        public int HealthDelayed { get; set; }

        public List<DashboardRiskItem> Risks { get; set; }
    }

    public class DashboardRiskItem
    {
        public string ProjectName { get; set; }
        public string Issue { get; set; }
        public string Severity { get; set; } // danger | warning
    }
}
