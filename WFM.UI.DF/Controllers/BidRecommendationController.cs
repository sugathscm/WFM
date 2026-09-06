using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class BidRecommendationController : BaseController
    {
        private readonly BidRecommendationService bidRecommendationService = new BidRecommendationService();
        private readonly RecommendStatusService recommendStatusService = new RecommendStatusService();
        private readonly ProjectService projectService = new ProjectService();

        // GET: BidRecommendation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var projects = projectService.GetProjects(0);
            var bidRecommendations = bidRecommendationService.GetList();
            var recommendStatuses = recommendStatusService.GetRecommendStatusList();

            List<BidRecommendationViewModel> modelList = new List<BidRecommendationViewModel>();

            foreach (var project in projects)
            {
                var bidRecommendation = bidRecommendations.Where(b => b.ProjectId == project.Id).FirstOrDefault();
                var recommendStatus = (bidRecommendation == null) ? null : recommendStatuses.Where(r => r.Id == bidRecommendation.RecommendStatusId).FirstOrDefault();

                modelList.Add(new BidRecommendationViewModel
                {
                    Id = (bidRecommendation == null) ? 0 : bidRecommendation.Id,
                    ProjectId = project.Id,
                    ProjectCode = project.Code,
                    ProjectName = project.Name,
                    ConsortiumJV = (bidRecommendation == null) ? "" : bidRecommendation.ConsortiumJV,
                    Source = (bidRecommendation == null) ? "" : bidRecommendation.Source,
                    StrategicFit = (bidRecommendation == null) ? "" : bidRecommendation.StrategicFit,
                    Eligible = (bidRecommendation == null) ? null : bidRecommendation.Eligible,
                    RecommendStatusId = (bidRecommendation == null) ? null : bidRecommendation.RecommendStatusId,
                    RecommendStatusName = (recommendStatus == null) ? "" : recommendStatus.Name
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }
    }
}
