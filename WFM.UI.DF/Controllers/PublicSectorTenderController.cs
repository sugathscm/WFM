using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.BAL.ViewModels;

namespace WFM.UI.DF.Controllers
{
    public class PublicSectorTenderController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ProjectService projectService = new ProjectService();
        private readonly ProjectSectorService sectorService = new ProjectSectorService();
        private readonly DocumentService documentService = new DocumentService();
        private readonly ProjectDocumentService projectDocumentService = new ProjectDocumentService();
        private readonly SourcingPartnerService sourcingPartnerService = new SourcingPartnerService();
        private readonly PrincipalService principalService = new PrincipalService();
        private readonly int projectTypeId = 2;

        public PublicSectorTenderController()
        {
        }
        public PublicSectorTenderController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            List<ProjectViewModel> projectsWFM = projectService.GetProjects(projectTypeId);

            var ProjectTypes = projectsWFM.GroupBy(p => p.ProjectTypeName).ToList();

            Dictionary<string, List<ProjectViewModel>> projectListPerType = new Dictionary<string, List<ProjectViewModel>>();

            foreach (var ProjectType in ProjectTypes)
            {
                projectListPerType.Add(ProjectType.Key, ProjectType.ToList());
            }

            ViewBag.ProjectTypes = projectListPerType;
        }

        // GET: Tender
        public ActionResult Details()
        {
            return View();
        }
    }
}