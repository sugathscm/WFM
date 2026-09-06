using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class GateControlController : BaseController
    {
        private readonly GateControlService gateControlService = new GateControlService();
        private readonly ProjectService projectService = new ProjectService();

        // GET: GateControl
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var projects = projectService.GetProjects(0);
            var gateControls = gateControlService.GetList();

            List<GateControlViewModel> modelList = new List<GateControlViewModel>();

            foreach (var project in projects)
            {
                var gateControl = gateControls.Where(g => g.ProjectId == project.Id).FirstOrDefault();

                modelList.Add(new GateControlViewModel
                {
                    Id = (gateControl == null) ? 0 : gateControl.Id,
                    ProjectId = project.Id,
                    ProjectCode = project.Code,
                    ProjectName = project.Name,
                    QG1Bid = (gateControl == null) ? null : gateControl.QG1Bid,
                    QG2Kickoff = (gateControl == null) ? null : gateControl.QG2Kickoff,
                    QG3Fifty = (gateControl == null) ? null : gateControl.QG3Fifty,
                    QG4Ninety = (gateControl == null) ? null : gateControl.QG4Ninety,
                    QG5Approve = (gateControl == null) ? null : gateControl.QG5Approve,
                    QG6Submit = (gateControl == null) ? null : gateControl.QG6Submit,
                    GatesDone = (gateControl == null) ? null : gateControl.GatesDone,
                    PercentComplete = (gateControl == null) ? null : gateControl.PercentComplete,
                    CurrentStage = (gateControl == null) ? "" : gateControl.CurrentStage
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }
    }
}
