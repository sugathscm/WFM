using System.Linq;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.DAL;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class HandoverController : BaseController
    {
        private const int ProposalTypeId = 7;

        private readonly ProjectService projectService = new ProjectService();
        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly ProjectHandoverService projectHandoverService = new ProjectHandoverService();

        // GET: Handover
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList()
        {
            var list = projectService.GetProjectList(null, ProposalTypeId, null, null, null, null);
            var handoverList = projectHandoverService.GetList();
            var employeeList = employeeService.GetEmployeeList();

            var enrichedList = list.Select(p =>
            {
                var handover = handoverList.Where(h => h.ProjectId == p.Id).FirstOrDefault();
                var pm = (p.AssigneeId == null) ? null : employeeList.Where(e => e.Id == p.AssigneeId).FirstOrDefault();

                return new
                {
                    p.Id,
                    ProjRef = p.ProjectCode,
                    ProjectName = p.Name,
                    Client = p.OrganizationName,
                    ContractValue = p.LKRValue,
                    StartDate = (p.StartDate == null) ? "" : p.StartDate.Value.ToString("yyyy-MM-dd"),
                    EndDate = (p.ExpiaryDate == null) ? "" : p.ExpiaryDate.Value.ToString("yyyy-MM-dd"),
                    PMName = (pm == null) ? "" : pm.Name,
                    HandoverDone = (handover != null && handover.HandoverDone == true) ? "Yes" : "No",
                    HandoverStatus = (handover == null) ? "NOT READY" : handover.HandoverStatus
                };
            }).ToList();

            JsonResult jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult = Json(new { data = enrichedList }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult GetChecklist(int projectId)
        {
            var handover = projectHandoverService.GetByProjectId(projectId);
            if (handover == null)
            {
                handover = new WFM_ProjectHandover { ProjectId = projectId, HandoverStatus = "NOT READY" };
            }

            return Json(handover, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveChecklist([Bind(Prefix = "handover")] WFM_ProjectHandover handover)
        {
            var existing = projectHandoverService.GetByProjectId(handover.ProjectId);
            if (existing != null)
            {
                handover.Id = existing.Id;
                handover.CreatedBy = existing.CreatedBy;
                handover.CreatedDate = existing.CreatedDate;
            }
            else
            {
                handover.CreatedBy = User.Identity.Name;
                handover.CreatedDate = System.DateTime.Now;
            }
            handover.UpdatedBy = User.Identity.Name;
            handover.UpdatedDate = System.DateTime.Now;

            projectHandoverService.SaveOrUpdate(handover);

            return Json(new { success = true, handoverStatus = handover.HandoverStatus, handoverDone = handover.HandoverDone });
        }
    }
}
