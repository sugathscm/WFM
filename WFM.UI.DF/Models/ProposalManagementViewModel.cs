using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.Models
{
    public class ProposalManagementViewModel
    {
        public ProposalPipelineViewModel Pipeline { get; set; }

        public ProposalTrackingViewModel Tracking { get; set; }

        public List<ProposalRiskViewModel> Risks { get; set; }

        public ProjectPortfolioViewModel Projects { get; set; }


        public class ProposalPipelineViewModel
        {
            public int EoiSubmitted { get; set; }
            public int Shortlisted { get; set; }
            public int RfpStage { get; set; }
            public int Awarded { get; set; }
        }

        public class ProposalTrackingViewModel
        {
            public int Submitted { get; set; }
            public int Ongoing { get; set; }
            public int InPreparation { get; set; }

            public int Won { get; set; }
            public int Lost { get; set; }
            public int Rfi { get; set; }
            public int Rfp { get; set; }

            public int Total
            {
                get
                {
                    return Won + Lost + Rfi + Rfp;
                }
            }
        }

        public class ProposalRiskViewModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public int Count { get; set; }

            // 0 - 100
            public int Percentage { get; set; }

            public string Color { get; set; }
            public string IconClass { get; set; }
        }

    }
}