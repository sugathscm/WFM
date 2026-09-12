using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WFM.DAL;
using WFM.UI.DF.Models;

namespace WFM.UI.DF.ModelsView
{
    public class ProjectTenderViewModel : WFM_Project
    {
        public Form8BViewModel Form8BViewModel { get; set; }
        public Form12ViewModel Form12ViewModel { get; set; }
        public bool Form8BGenerated { get; set; }
        public bool Form12Generated { get; set; }
        public BidNoBidDecisionViewModel BidNoBidDecisionViewModel { get; set; }
        public QAScoreViewModel QAScoreViewModel { get; set; }
        public GateControlViewModel GateControlViewModel { get; set; }
        public BidRecommendationViewModel BidRecommendationViewModel { get; set; }
        public ProposalOutcomeViewModel ProposalOutcomeViewModel { get; set; }

        public List<FileNoteViewModel> FileNotes { get; set; }
        public List<GetTaskList_Result> Tasks { get; set; }

        public string strDatePublished { get; set; }
        public string strExpiaryDate { get; set; }
        public string strPreBidMeetingDate { get; set; }
        public string strTenderDocCollectionFee { get; set; }
        public string strLKRValue { get; set; }
        public string strUSDValue { get; set; }
        public string ContinentName { get; set; }
    }
}