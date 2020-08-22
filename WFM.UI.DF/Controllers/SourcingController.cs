using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL.Enums;
using WFM.BAL.Services;
using WFM.BAL.ViewModels;
using WFM.DAL;
using WFM.UI.DF.Models;

namespace WFM.UI.DF.Controllers
{
    public class SourcingController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ProjectService projectService = new ProjectService();
        private readonly ProjectSectorService sectorService = new ProjectSectorService();
        private readonly DocumentService documentService = new DocumentService();
        private readonly ProjectDocumentService projectDocumentService = new ProjectDocumentService();
        private readonly SourcingPartnerService sourcingPartnerService = new SourcingPartnerService();
        private readonly PrincipalService principalService = new PrincipalService();
        private readonly StatusService statusService = new StatusService();
        private readonly ProjectTypeService projectTypeService = new ProjectTypeService();
        private readonly MethodOfIntroductionService methodOfIntroductionService = new MethodOfIntroductionService();
        private readonly PriorityFrameworkService priorityFrameworkService = new PriorityFrameworkService();
        private readonly OrganizationService organizationService = new OrganizationService();
        private readonly ContactService contactService = new ContactService();
        private readonly DivisionService divisionService = new DivisionService();
        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly CommonDataService commonDataService = new CommonDataService();
        private readonly int projectTypeId = 0; // All types

        public SourcingController()
        {
        }
        public SourcingController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

            ViewBag.ProjectTypes = projectsWFM;
        }


        // GET: Sourcing
        public ActionResult Details(int? id)
        {
            LoadControls();
            if (id != null)
            {
                var project = projectService.GetProjectById(projectTypeId, id.Value);
                ProjectViewModel projectView = project.Cast<ProjectViewModel>();
                ViewBag.SubSectorList = GetSubSectorList(project.SectorId.Value);
                return View(projectView);
            }

            return View();
        }

        private void LoadControls()
        {
            var documents = documentService.GetDocumentsByProjectType(projectTypeId);
            var projectDocuments = projectDocumentService.GetProjectDocumentsByProjectType(projectTypeId);
            var documentsWithFields = documentService.GetDocumentsByProjectTypeWithFields(projectTypeId);

            ViewBag.SectorList = sectorService.GetProjectSectorParentList();
            ViewBag.SubSectorList = "";

            ViewBag.Type1DocumentList = documents.Where(d => d.DocumentTabId == 1 && d.HasFields == false).OrderBy(d => d.DisplayOrder).ToList();
            ViewBag.StatusList = statusService.GetStatusList();
            ViewBag.ProjectTypeList = projectTypeService.GetProjectTypeList();
            ViewBag.MethodOfIntroductionList = methodOfIntroductionService.GetMethodOfIntroductionList();
            ViewBag.PriorityFrameworkList = priorityFrameworkService.GetPriorityFrameworkList();
            ViewBag.OrganizationList = organizationService.GetOrganizationList();
            ViewBag.ContactList = contactService.GetContactList();
            ViewBag.DivisionList = divisionService.GetDivisionList();
            ViewBag.EmployeeList = employeeService.GetEmployeeList();

            ViewBag.TenderTypeList = commonDataService.GetCommonData((int)CommonDataType.TenderType);
            ViewBag.TypeOfSaleList = commonDataService.GetCommonData((int)CommonDataType.TypeOfSale);
            ViewBag.DocStatusList = commonDataService.GetCommonData((int)CommonDataType.DocStatus);
            ViewBag.ContinentList = commonDataService.GetCommonData((int)CommonDataType.Continent);
            ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            ViewBag.FileStatusList = commonDataService.GetCommonData((int)CommonDataType.FileStatus);
            ViewBag.SLICCopyList = commonDataService.GetCommonData((int)CommonDataType.SLICCopy);
            ViewBag.DocStatusExtendedList = commonDataService.GetCommonData((int)CommonDataType.DocStatusExtended);
            ViewBag.SourceList = commonDataService.GetCommonData((int)CommonDataType.Source);
            ViewBag.ProceedStatusList = commonDataService.GetCommonData((int)CommonDataType.ProceedStatus);
            ViewBag.ProjectDivisionalStatusList = commonDataService.GetCommonData((int)CommonDataType.ProjectDivisionalStatus);
            ViewBag.HotPickList = commonDataService.GetCommonData((int)CommonDataType.HotPick);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(FormCollection formCollection, WFM_Project model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_Project project = null;
                WFM_Project oldProject = null;

                project = model;
                project.IsActive = true;
                project.DateCreated = DateTime.Now;

                if (formCollection["StartDate"] != "")
                    project.StartDate = DateTime.Parse(formCollection["StartDate"]);
                if (formCollection["ExpiaryDate"] != "")
                    project.ExpiaryDate = DateTime.Parse(formCollection["ExpiaryDate"]);

                if (formCollection["FileCreatedDate"] != "")
                    project.FileCreatedDate = DateTime.Parse(formCollection["FileCreatedDate"]);
                if (formCollection["DatePublished"] != "")
                    project.DatePublished = DateTime.Parse(formCollection["DatePublished"]);

                projectService.SaveOrUpdate(project);
            }
            catch (System.Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Sourcing");
        }


        public string GetSubSectors(string id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            int parentId = int.Parse(id);
            List<BaseViewModel> SubSectorList = GetSubSectorList(parentId);
            return js.Serialize(SubSectorList);
        }

        private List<BaseViewModel> GetSubSectorList(int parentId)
        {
            return sectorService.GetSubProjectSectorsByParentId(parentId).Select(s => new BaseViewModel()
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }
    }
}