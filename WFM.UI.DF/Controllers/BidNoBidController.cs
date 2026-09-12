using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class BidNoBidController : BaseController
    {
        private readonly BidNoBidDecisionService bidNoBidDecisionService = new BidNoBidDecisionService();
        private readonly ProjectService projectService = new ProjectService();

        // GET: BidNoBid
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var projects = projectService.GetProjects(0);
            var decisions = bidNoBidDecisionService.GetList();

            List<BidNoBidDecisionViewModel> modelList = new List<BidNoBidDecisionViewModel>();

            foreach (var project in projects)
            {
                var decision = decisions.Where(d => d.ProjectId == project.Id).FirstOrDefault();

                modelList.Add(new BidNoBidDecisionViewModel
                {
                    Id = (decision == null) ? 0 : decision.Id,
                    ProjectId = project.Id,
                    ProjectCode = project.Code,
                    ProjectName = project.Name,
                    StrategicFit = (decision == null) ? null : decision.StrategicFit,
                    WinProbability = (decision == null) ? null : decision.WinProbability,
                    TechCapability = (decision == null) ? null : decision.TechCapability,
                    Resource = (decision == null) ? null : decision.Resource,
                    Profitability = (decision == null) ? null : decision.Profitability,
                    ClientRelationship = (decision == null) ? null : decision.ClientRelationship,
                    Risks = (decision == null) ? null : decision.Risks,
                    Total = (decision == null) ? null : decision.Total,
                    Decision = (decision == null) ? "" : decision.Decision,
                    Notes = (decision == null) ? "" : decision.Notes
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }
    }
}
