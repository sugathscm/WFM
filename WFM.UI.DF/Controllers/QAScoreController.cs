using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class QAScoreController : BaseController
    {
        private readonly QAScoreService qaScoreService = new QAScoreService();
        private readonly ProjectService projectService = new ProjectService();

        // GET: QAScore
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var projects = projectService.GetProjects(0);
            var scores = qaScoreService.GetList();

            List<QAScoreViewModel> modelList = new List<QAScoreViewModel>();

            foreach (var project in projects)
            {
                var score = scores.Where(s => s.ProjectId == project.Id).FirstOrDefault();

                modelList.Add(new QAScoreViewModel
                {
                    Id = (score == null) ? 0 : score.Id,
                    ProjectId = project.Id,
                    ProjectCode = project.Code,
                    ProjectName = project.Name,
                    RfpTorCompliance = (score == null) ? null : score.RfpTorCompliance,
                    MethodologyApproach = (score == null) ? null : score.MethodologyApproach,
                    KeyExpertsCv = (score == null) ? null : score.KeyExpertsCv,
                    RelevantExperience = (score == null) ? null : score.RelevantExperience,
                    WorkPlanDeliverables = (score == null) ? null : score.WorkPlanDeliverables,
                    MandatoryAttachments = (score == null) ? null : score.MandatoryAttachments,
                    FinancialCompetitiveness = (score == null) ? null : score.FinancialCompetitiveness,
                    ProposalQuality = (score == null) ? null : score.ProposalQuality,
                    FinalQcSubmissionReadiness = (score == null) ? null : score.FinalQcSubmissionReadiness,
                    Total = (score == null) ? null : score.Total,
                    Decision = (score == null) ? "" : score.Decision,
                    Notes = (score == null) ? "" : score.Notes
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }
    }
}
