using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.BAL.ViewModels;

namespace WFM.UI.DF.Controllers
{
    public class PublicSectorNonTenderController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly ProjectService projectService = new ProjectService();
        private readonly ProjectSectorService sectorService = new ProjectSectorService();
        private readonly DocumentService documentService = new DocumentService();
        private readonly ProjectDocumentService projectDocumentService = new ProjectDocumentService();
        private readonly SourcingPartnerService sourcingPartnerService = new SourcingPartnerService();
        private readonly PrincipalService principalService = new PrincipalService();
        private readonly int projectTypeId = 3;

        public PublicSectorNonTenderController()
        {
        }
        public PublicSectorNonTenderController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        public ActionResult Details(int? id)
        {
            LoadControls();
            if (id != null)
            {
                var project = projectService.GetProjectById(projectTypeId, id.Value);
                return View(project);
            }

            return View();
        }

        private void LoadControls()
        {
            var documents = documentService.GetDocumentsByProjectType(projectTypeId);
            var projectDocuments = projectDocumentService.GetProjectDocumentsByProjectType(projectTypeId);
            var documentsWithFields = documentService.GetDocumentsByProjectTypeWithFields(projectTypeId);

            ViewBag.SectorList = sectorService.GetProjectSectorParentList();

            ViewBag.Type1DocumentList = documents.Where(d => d.DocumentTabId == 1 && d.HasFields == false).OrderBy(d => d.DisplayOrder).ToList();
            ViewBag.Type2DocumentList = documents.Where(d => d.DocumentTabId == 2 && d.HasFields == false).OrderBy(d => d.DisplayOrder).ToList();
            ViewBag.Type3DocumentList = documents.Where(d => d.DocumentTabId == 3 && d.HasFields == false).OrderBy(d => d.DisplayOrder).ToList();
            ViewBag.Type6DocumentList = documents.Where(d => d.DocumentTabId == 6 && d.HasFields == false).OrderBy(d => d.DisplayOrder).ToList();

            ViewBag.Type1ProjectDocumentList = projectDocuments.Where(d => d.WFM_Document.DocumentTabId == 1 && d.WFM_Document.HasFields == false).OrderBy(d => d.WFM_Document.DisplayOrder).ToList();
            ViewBag.Type2ProjectDocumentList = projectDocuments.Where(d => d.WFM_Document.DocumentTabId == 2 && d.WFM_Document.HasFields == false).OrderBy(d => d.WFM_Document.DisplayOrder).ToList();

            ViewBag.Form8B = documentsWithFields.Where(d => d.DocumentTabId == 4 && d.Id == 22).OrderBy(d => d.DisplayOrder).ToList();

            ViewBag.SourcingPartners = sourcingPartnerService.GetSourdingPartnerList();
            ViewBag.Principals = principalService.GetPrincipalList();
        }
    }
}