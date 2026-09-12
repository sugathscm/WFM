using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Enums;
using WFM.BAL.Services;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    public class ReportsController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly TaskTrackerService taskTrackerService = new TaskTrackerService();
        private readonly ProjectService projectService = new ProjectService();
        private readonly CommonDataService commonDataService = new CommonDataService();
        private readonly MeetingTypeService meetingService = new MeetingTypeService();
        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly TaskTypeService taskTypeService = new TaskTypeService();
        private readonly TaskTrackerCategoryService taskTrackerCategoryService = new TaskTrackerCategoryService();

        // GET: TaskTracker
        public ReportsController()
        {

        }

        public ReportsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public ActionResult TaskStatusReport()
        {
            var clientDate = CommonService.GetClientDate(Request);

            List<SelectListItem> projects = projectService.GetProjects(0).ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name.ToString(),
                    Value = a.Id.ToString(),
                    Selected = false
                };
            }).ToList();

            var newItem = new SelectListItem { Text = "No Project", Value = "1" };
            projects.Add(newItem);

            List<SelectListItem> statuses = taskTrackerService.GetTaskStatusList().ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name.ToString(),
                    Value = a.Id.ToString(),
                    Selected = false
                };
            }).ToList();

            newItem = new SelectListItem { Text = "Pending", Value = "111" };
            statuses.Add(newItem);

            var dateTypes = new List<SelectListItem>();

            newItem = new SelectListItem { Text = "Assigned Date", Value = "0" };
            dateTypes.Add(newItem);
            newItem = new SelectListItem { Text = "Due Date", Value = "1" };
            dateTypes.Add(newItem);
            newItem = new SelectListItem { Text = "Scheduled Date", Value = "2" };
            dateTypes.Add(newItem);

            ViewBag.ProjectList = projects;
            ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            ViewBag.MeetingList = meetingService.GetMeetingList();
            ViewBag.EmployeeList = employeeService.GetEmployeeList();
            ViewBag.TaskTypeList = taskTypeService.GetTaskTypeList();
            ViewBag.TaskTrackerCategoryList = taskTrackerCategoryService.GetTaskTrackerCategoryList();
            ViewBag.TaskTrackerStatusList = statuses;
            ViewBag.DateTypeList = dateTypes;
            ViewBag.GeneratedBy = User.Identity.Name;
            ReportsViewModel reportsViewModel = new ReportsViewModel();

            return View(reportsViewModel);

        }

        public ActionResult TaskScheduleReport()
        {
            var clientDate = CommonService.GetClientDate(Request);

            List<SelectListItem> projects = projectService.GetProjects(0).ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name.ToString(),
                    Value = a.Id.ToString(),
                    Selected = false
                };
            }).ToList();

            var newItem = new SelectListItem { Text = "No Project", Value = "1" };
            projects.Add(newItem);

            List<SelectListItem> statuses = taskTrackerService.GetTaskStatusList().ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.Name.ToString(),
                    Value = a.Id.ToString(),
                    Selected = false
                };
            }).ToList();

            newItem = new SelectListItem { Text = "Pending", Value = "111" };
            statuses.Add(newItem);

            ViewBag.ProjectList = projects;
            ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            ViewBag.MeetingList = meetingService.GetMeetingList();
            ViewBag.EmployeeList = employeeService.GetEmployeeList();
            ViewBag.TaskTypeList = taskTypeService.GetTaskTypeList();
            ViewBag.TaskTrackerCategoryList = taskTrackerCategoryService.GetTaskTrackerCategoryList();
            ViewBag.TaskTrackerStatusList = statuses;
            ViewBag.GeneratedBy = User.Identity.Name;

            ReportsViewModel reportsViewModel = new ReportsViewModel();

            return View(reportsViewModel);

        }


    }
}