using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Runtime.Remoting.Lifetime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using WFM.BAL.Enums;
using WFM.BAL.Services;
using WFM.BAL.ViewModels;
using WFM.DAL;
using WFM.UI.DF.Models;
using WFM.UI.DF.ModelsView;
using static WFM.UI.DF.Models.ProposalManagementViewModel;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly ProjectService projectService = new ProjectService();
        private readonly ProjectTypeService projectTypeService = new ProjectTypeService();
        private readonly VCPService vCPService = new VCPService();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TaskTrackerService taskTrackerService = new TaskTrackerService();
        private readonly CommonDataService commonDataService = new CommonDataService();
        private readonly MeetingTypeService meetingService = new MeetingTypeService();
        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly TaskTypeService taskTypeService = new TaskTypeService();
        private readonly TaskTrackerCategoryService taskTrackerCategoryService = new TaskTrackerCategoryService();
        private readonly BidNoBidDecisionService bidNoBidDecisionService = new BidNoBidDecisionService();
        private readonly ProposalOutcomeService proposalOutcomeService = new ProposalOutcomeService();
        private readonly GateControlService gateControlService = new GateControlService();
        private readonly ProjectHandoverService projectHandoverService = new ProjectHandoverService();
        private readonly ConfigurationService configurationService = new ConfigurationService();

        private const int OpportunityTypeId = 4;
        private const int ProposalTypeId = 7;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult MyTasks()
        {
            ViewBag.ProjectList = projectService.GetProjects(0);
            ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            ViewBag.MeetingList = meetingService.GetMeetingList();
            ViewBag.EmployeeList = employeeService.GetEmployeeList();
            ViewBag.TaskTypeList = taskTypeService.GetTaskTypeList();
            ViewBag.TaskTrackerCategoryList = taskTrackerCategoryService.GetTaskTrackerCategoryList();
            ViewBag.TaskTrackerStatusList = taskTrackerService.GetTaskStatusList();

            PrepareDashboardProjectList();

            var UserId = User.Identity.GetUserId();
            var empId = employeeService.GetEmployeeByUserId(UserId);

            if (empId != null)
            {
                ViewBag.EmployeeId = empId.Id;
            }

            return View();
        }

        public ActionResult GetList(
            int? StatusId, int? ProjectId,
            int? MeetingId, int? TaskTypeId,
            int? AssigneeId, int? PriorityId, int? Id)
        {
            var UserId = User.Identity.GetUserId();
            var empId = employeeService.GetEmployeeByUserId(UserId);
            if (empId != null)
            {
                AssigneeId = empId.Id;
            }

            List<GetTaskList_Result> modelList = new List<GetTaskList_Result>();

            modelList = taskTrackerService.GetTaskList(StatusId, ProjectId, MeetingId, TaskTypeId, AssigneeId, Id).ToList();
            if ((PriorityId > 0) || (PriorityId != null))
                modelList = modelList.Where(t => t.PriorityId == PriorityId).ToList();

            JsonResult jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult = Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        private void PrepareDashboardProjectList()
        {
            try
            {
                //List<ProjectViewModel> projectsWFM = projectService.GetProjects(0);

                //var ProjectTypes = projectsWFM.GroupBy(p => p.ProjectTypeName).ToList();

                //Dictionary<string, List<ProjectViewModel>> projectListPerType = new Dictionary<string, List<ProjectViewModel>>();

                //foreach (var ProjectType in ProjectTypes)
                //{
                //    projectListPerType.Add(ProjectType.Key, ProjectType.ToList());
                //}

                //ViewBag.ProjectTypes = projectListPerType;

                List<Dictionary<string, List<string>>> dashboardDataList = new List<Dictionary<string, List<string>>>();

                CommonDataService commonDataService = new CommonDataService();
                List<DAL.WFM_CommonData> divisionalStatusList = commonDataService.GetCommonData(12);
                var projectTypes = projectTypeService.GetProjectTypeList().Where(p => p.ShowInMenu == true).ToList();

                Dictionary<string, List<string>> dashboardData = new Dictionary<string, List<string>>();
                List<string> data = new List<string>();

                foreach (var commonData in divisionalStatusList)
                {
                    data.Add(commonData.Name);
                }
                data.Add("Total");
                dashboardData.Add("H", data);
                dashboardDataList.Add(dashboardData);

                var actualDashboardData = projectService.GetDashboardData();
                var typeList = actualDashboardData.Select(t => t.Name).Distinct();

                foreach (var type in typeList)
                {
                    data = new List<string>();

                    data.Add(type);

                    var typeDataList = actualDashboardData.Where(d => d.Name == type).ToList();
                    int total = 0;

                    foreach (var commonData in divisionalStatusList)
                    {
                        var value = typeDataList.Where(d => d.Name == type && d.TabName == commonData.Name).FirstOrDefault();
                        if (value == null)
                        {
                            data.Add("0");
                        }
                        else
                        {
                            total++;
                            data.Add(value.COU.ToString());
                        }
                    }

                    var projectType = projectTypes.Where(p => p.Name == type).FirstOrDefault();

                    if (projectType != null)
                        data.Add("<a href='/" + projectType.Path + "/Index/" + projectType.Id + "'>" + total.ToString() + "</a>");
                    else
                        data.Add(total.ToString());

                    dashboardData.Add(type, data);
                }


                ViewBag.DataList = dashboardDataList;
                ViewBag.TypeList = typeList;
            }
            catch (System.Exception ex)
            {
                log.Error("Error in loading projects : " + ex.Message);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Index()
        {
            var allProjects = projectService.GetProjectList(null, null, null, null, null, null).Where(p => p.IsActive).ToList();
            var opportunities = allProjects.Where(p => p.ProjectTypeId == OpportunityTypeId).ToList();
            var proposals = allProjects.Where(p => p.ProjectTypeId == ProposalTypeId).ToList();

            var bidNoBidList = bidNoBidDecisionService.GetList();
            var outcomeList = proposalOutcomeService.GetList();
            var gateControlList = gateControlService.GetList();
            var handoverList = projectHandoverService.GetList();

            int alertLeadTimeDays = int.Parse(configurationService.GetValue("AlertLeadTimeDays", "3"));
            int healthOnTrackAt = int.Parse(configurationService.GetValue("ProjectHealthOnTrackAt", "80"));
            int healthAttentionAt = int.Parse(configurationService.GetValue("ProjectHealthAttentionAt", "60"));
            var today = DateTime.Today;

            var goDecisions = bidNoBidList.Where(b => b.Decision == "GO").ToList();
            var wonOutcomes = outcomeList.Where(o => o.Outcome == "Won").ToList();
            var shortlisted = outcomeList.Count(o => o.Outcome == "Shortlisted");

            // Win rate over the last 90 days, by result date.
            var recentOutcomes = outcomeList.Where(o => o.ResultDate != null && o.ResultDate.Value >= today.AddDays(-90)).ToList();
            int recentWon = recentOutcomes.Count(o => o.Outcome == "Won");
            int recentLost = recentOutcomes.Count(o => o.Outcome == "Lost");
            decimal? winRate90d = (recentWon + recentLost == 0) ? (decimal?)null : Math.Round((decimal)recentWon / (recentWon + recentLost) * 100, 0);

            // "Ongoing projects" = proposals that converted (won) and are still active.
            var wonProjectIds = wonOutcomes.Select(o => o.ProjectId).ToList();
            var ongoingProjects = proposals.Where(p => wonProjectIds.Contains(p.Id)).ToList();

            int healthOnTrack = 0, healthAttention = 0, healthDelayed = 0;
            var riskItems = new List<DashboardRiskItem>();

            foreach (var p in ongoingProjects)
            {
                if (p.ExpiaryDate == null) continue;
                int daysLeft = (p.ExpiaryDate.Value.Date - today).Days;
                if (daysLeft >= healthOnTrackAt) healthOnTrack++;
                else if (daysLeft >= healthAttentionAt) healthAttention++;
                else healthDelayed++;
            }

            // Risk 1: proposals overdue against their deadline, still undecided.
            foreach (var p in proposals)
            {
                if (p.ExpiaryDate == null) continue;
                var outcome = outcomeList.Where(o => o.ProjectId == p.Id).FirstOrDefault();
                string outcomeName = (outcome == null || string.IsNullOrEmpty(outcome.Outcome)) ? "Pending" : outcome.Outcome;
                if (outcomeName != "Pending" && outcomeName != "Shortlisted") continue;

                int daysLeft = (p.ExpiaryDate.Value.Date - today).Days;
                if (daysLeft < 0)
                    riskItems.Add(new DashboardRiskItem { ProjectId = p.Id, ProjectName = p.Name, Issue = "Overdue", Severity = "danger" });
                else if (daysLeft <= alertLeadTimeDays)
                    riskItems.Add(new DashboardRiskItem { ProjectId = p.Id, ProjectName = p.Name, Issue = "Due in " + daysLeft + "d", Severity = "warning" });
            }

            // Risk 2: gate control stalled - not updated in the last 14 days and not complete.
            foreach (var g in gateControlList.Where(g => g.PercentComplete != 100 && g.UpdatedDate != null && g.UpdatedDate.Value <= today.AddDays(-14)))
            {
                var p = allProjects.FirstOrDefault(x => x.Id == g.ProjectId);
                if (p == null) continue;
                int daysStalled = (today - g.UpdatedDate.Value.Date).Days;
                riskItems.Add(new DashboardRiskItem { ProjectId = p.Id, ProjectName = p.Name, Issue = "Gate stalled " + daysStalled + "d", Severity = "warning" });
            }

            // Risk 3: handover not ready for a project nearing its end date.
            foreach (var h in handoverList.Where(h => h.HandoverStatus != "READY"))
            {
                var p = allProjects.FirstOrDefault(x => x.Id == h.ProjectId);
                if (p == null || p.ExpiaryDate == null) continue;
                int daysLeft = (p.ExpiaryDate.Value.Date - today).Days;
                if (daysLeft <= alertLeadTimeDays)
                    riskItems.Add(new DashboardRiskItem { ProjectId = p.Id, ProjectName = p.Name, Issue = "Handover not ready", Severity = "danger" });
            }

            var model = new ManagementDashboardViewModel
            {
                ActiveOpportunities = opportunities.Count,
                ActiveProposals = proposals.Count,
                WinRate90d = winRate90d,
                OngoingProjects = ongoingProjects.Count,
                OverdueCount = riskItems.Count,
                AvgBidScore = goDecisions.Any() ? Math.Round(goDecisions.Average(b => (decimal)(b.Total ?? 0)), 0) : (decimal?)null,

                FunnelOpportunity = opportunities.Count,
                FunnelGoDecision = goDecisions.Count,
                FunnelProposal = proposals.Count,
                FunnelShortlisted = shortlisted,
                FunnelWon = wonOutcomes.Count,

                HealthOnTrack = healthOnTrack,
                HealthAttention = healthAttention,
                HealthDelayed = healthDelayed,

                Risks = riskItems
            };

            return View(model);
        }
    }
}