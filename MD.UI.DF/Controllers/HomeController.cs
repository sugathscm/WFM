using log4net;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.BAL.ViewModels;
using WFM.DAL;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Net.Configuration;
using WFM.UI.DF.ModelsView;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.Remoting.Lifetime;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Services.Description;
using System;
using WFM.BAL.Enums;
using Microsoft.AspNet.Identity;

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

        public ActionResult Index()
        {
            //ViewBag.ProjectList = projectService.GetProjects(0);
            //ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            //ViewBag.MeetingList = meetingService.GetMeetingList();
            //ViewBag.EmployeeList = employeeService.GetEmployeeList();
            //ViewBag.TaskTypeList = taskTypeService.GetTaskTypeList();
            //ViewBag.TaskTrackerCategoryList = taskTrackerCategoryService.GetTaskTrackerCategoryList();
            //ViewBag.TaskTrackerStatusList = taskTrackerService.GetTaskStatusList();

            //var UserId = User.Identity.GetUserId();
            //var empId = employeeService.GetEmployeeByUserId(UserId);

            //if (empId != null)
            //{
            //    ViewBag.EmployeeId = empId.Id;
            //}

            return View();
        }
    }
}