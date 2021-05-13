using log4net;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.BAL.ViewModels;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly ProjectService projectService = new ProjectService();
        private readonly ProjectTypeService projectTypeService = new ProjectTypeService();
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            PrepareDashboardProjectList();

            //CommonService.SaveLoginAudit(new LoginAudit()
            //{
            //    DateLogged = DateTime.Now,
            //    UserId = new Guid(User.Identity.GetUserId()),
            //    IPAddress = Request.UserHostAddress
            //});

            return View();
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
    }
}