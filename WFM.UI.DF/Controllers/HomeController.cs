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
            var model = new ProposalManagementViewModel
            {
                Pipeline = new ProposalPipelineViewModel
                {
                    EoiSubmitted = 12,
                    Shortlisted = 7,
                    RfpStage = 9,
                    Awarded = 4
                },

                Tracking = new ProposalTrackingViewModel
                {
                    Submitted = 7,
                    Ongoing = 5,
                    InPreparation = 3,
                    Won = 4,
                    Lost = 3,
                    Rfi = 2,
                    Rfp = 3
                },

                Risks = new List<ProposalRiskViewModel>
                {
                    new ProposalRiskViewModel
                    {
                        Title = "Lack of Expert Team",
                        Description = "Both - No qualified expert mapped to a required position",
                        Count = 4,
                        Percentage = 85,
                        Color = "purple"
                    },

                    new ProposalRiskViewModel
                    {
                        Title = "Methodology Submission Delay",
                        Description = "Ongoing - Methodology document still in draft / missing past plan",
                        Count = 4,
                        Percentage = 65,
                        Color = "orange"
                    },

                    new ProposalRiskViewModel
                    {
                        Title = "Financial Proposal Weakness",
                        Description = "Submitted - Pricing scored low / uncompetitive vs winners",
                        Count = 2,
                        Percentage = 45,
                        Color = "red"
                    },

                    new ProposalRiskViewModel
                    {
                        Title = "Technical Proposal Weakness",
                        Description = "Submitted - Technical score below qualification threshold",
                        Count = 1,
                        Percentage = 30,
                        Color = "blue"
                    }
                },

                Projects = new ProjectPortfolioViewModel
                {
                    Delayed = 2,
                    NeedsAttention = 4,
                    OnTrack = 4,

                    Categories = new List<ProjectCategoryViewModel>
                    {
                        new ProjectCategoryViewModel
                        {
                            Name = "Engineering",
                            Delayed = 0,
                            Attention = 1,
                            OnTrack = 2
                        },

                        new ProjectCategoryViewModel
                        {
                            Name = "Environment",
                            Delayed = 0,
                            Attention = 1,
                            OnTrack = 2
                        },

                        new ProjectCategoryViewModel
                        {
                            Name = "Social & Urban Dev.",
                            Delayed = 1,
                            Attention = 1,
                            OnTrack = 1
                        },

                        new ProjectCategoryViewModel
                        {
                            Name = "Hydrology & Water",
                            Delayed = 0,
                            Attention = 1,
                            OnTrack = 2
                        }
                    }
                }
            };

            return View(model);
        }
    }
}