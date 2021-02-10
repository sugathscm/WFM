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
                List<ProjectViewModel> projectsWFM = projectService.GetProjects(0);

                var ProjectTypes = projectsWFM.GroupBy(p => p.ProjectTypeName).ToList();

                Dictionary<string, List<ProjectViewModel>> projectListPerType = new Dictionary<string, List<ProjectViewModel>>();

                foreach (var ProjectType in ProjectTypes)
                {
                    projectListPerType.Add(ProjectType.Key, ProjectType.ToList());
                }

                ViewBag.ProjectTypes = projectListPerType;
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