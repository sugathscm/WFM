using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WFM.BAL.Enums;
using WFM.BAL.Services;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class ProposalController : BaseController
    {
        private const int ProposalTypeId = 7;

        private readonly ProjectService projectService = new ProjectService();
        private readonly ProjectSectorService sectorService = new ProjectSectorService();
        private readonly StatusService statusService = new StatusService();
        private readonly OrganizationService organizationService = new OrganizationService();
        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly ConfigurationService configurationService = new ConfigurationService();
        private readonly BidNoBidDecisionService bidNoBidDecisionService = new BidNoBidDecisionService();
        private readonly QAScoreService qaScoreService = new QAScoreService();
        private readonly GateControlService gateControlService = new GateControlService();
        private readonly ProposalOutcomeService proposalOutcomeService = new ProposalOutcomeService();

        // GET: Proposal
        public ActionResult Index()
        {
            ViewBag.StatusList = statusService.GetStatusList();
            ViewBag.SectorList = sectorService.GetProjectSectorParentList();
            ViewBag.OrganizationList = organizationService.GetOrganizationList();

            return View();
        }

        public ActionResult GetList(int? StatusId, int? SectorId, int? OrganizationId)
        {
            var list = projectService.GetProjectList(StatusId, ProposalTypeId, SectorId, OrganizationId, null, null);

            var bidNoBidList = bidNoBidDecisionService.GetList();
            var qaScoreList = qaScoreService.GetList();
            var gateControlList = gateControlService.GetList();
            var outcomeList = proposalOutcomeService.GetList();
            var employeeList = employeeService.GetEmployeeList();

            int alertLeadTimeDays = int.Parse(configurationService.GetValue("AlertLeadTimeDays", "3"));
            int amberWarningWindowDays = int.Parse(configurationService.GetValue("AmberWarningWindowDays", "5"));

            var today = DateTime.Today;

            var enrichedList = list.Select(p =>
            {
                var bidNoBid = bidNoBidList.Where(b => b.ProjectId == p.Id).FirstOrDefault();
                var qaScore = qaScoreList.Where(q => q.ProjectId == p.Id).FirstOrDefault();
                var gateControl = gateControlList.Where(g => g.ProjectId == p.Id).FirstOrDefault();
                var outcome = outcomeList.Where(o => o.ProjectId == p.Id).FirstOrDefault();
                var owner = (p.AssigneeId == null) ? null : employeeList.Where(e => e.Id == p.AssigneeId).FirstOrDefault();

                int? daysLeft = null;
                string delayStatus = "";

                if (p.ExpiaryDate != null)
                {
                    daysLeft = (p.ExpiaryDate.Value.Date - today).Days;

                    if (daysLeft < 0)
                        delayStatus = "OVERDUE";
                    else if (daysLeft <= alertLeadTimeDays)
                        delayStatus = "CRITICAL (<=" + alertLeadTimeDays + "d)";
                    else if (daysLeft <= amberWarningWindowDays)
                        delayStatus = "AT RISK";
                    else
                        delayStatus = "ON TRACK";
                }

                return new
                {
                    p.Id,
                    p.ProjectCode,
                    p.OrganizationName,
                    p.SectorName,
                    p.LKRValue,
                    p.StatusName,
                    p.CSSStatusName,
                    StartDate = (p.StartDate == null) ? "" : p.StartDate.Value.ToString("yyyy-MM-dd"),
                    Deadline = (p.ExpiaryDate == null) ? "" : p.ExpiaryDate.Value.ToString("yyyy-MM-dd"),
                    DaysLeft = daysLeft,
                    DelayStatus = delayStatus,
                    BidNoBidTotal = (bidNoBid == null) ? (int?)null : bidNoBid.Total,
                    QAScoreTotal = (qaScore == null) ? (int?)null : qaScore.Total,
                    GatesDone = (gateControl == null) ? (int?)null : gateControl.GatesDone,
                    GateControlPercentComplete = (gateControl == null) ? (int?)null : gateControl.PercentComplete,
                    GateControlCurrentStage = (gateControl == null) ? "" : gateControl.CurrentStage,
                    Outcome = (outcome == null) ? "" : outcome.Outcome,
                    ReasonForLoss = (outcome == null) ? "" : outcome.ReasonForLoss,
                    OwnerName = (owner == null) ? "" : owner.Name
                };
            }).ToList();

            JsonResult jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult = Json(new { data = enrichedList }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
    }
}
