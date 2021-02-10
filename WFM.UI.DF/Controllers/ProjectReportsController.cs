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
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    public class ProjectReportsController : BaseController
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

        public ProjectReportsController()
        {
        }
        public ProjectReportsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            ViewBag.ProjectTypeList = projectTypeService.GetProjectTypeList().Where(pt => pt.ShowInMenu == true);
            ViewBag.ProjectList = projectService.GetProjects(0).Where(p => p.Code != null).ToList();

            return View();
        }

        public PartialViewResult OEFReport()
        {
            OEFReportViewModel oEF_ReportViewModel = new OEFReportViewModel();

            return PartialView("OEFReport", oEF_ReportViewModel);
        }

        public PartialViewResult EvalReport()
        {
            EvalReportViewModel evalReportViewModel = new EvalReportViewModel();

            return PartialView("EvalReport", evalReportViewModel);
        }

        public PartialViewResult FileNotesReport()
        {
            FileNotesReportViewModel fileNotesReportViewModel = new FileNotesReportViewModel();

            return PartialView("FileNotesReport", fileNotesReportViewModel);
        }

        public PartialViewResult LabelReport()
        {
            LabelReportViewModel labelReportViewModel = new LabelReportViewModel();

            return PartialView("LabelReport", labelReportViewModel);
        }

        public PartialViewResult PTTReport()
        {
            PTTReportViewModel pTTReportViewModel = new PTTReportViewModel();

            return PartialView("PTTReport", pTTReportViewModel);
        }

        public PartialViewResult TAFReport()
        {
            TAFReportViewModel tAFReportViewModel = new TAFReportViewModel();

            return PartialView("TAFReport", tAFReportViewModel);
        }
        public PartialViewResult TMNReport()
        {
            TMNReportViewModel tMNReportViewModel = new TMNReportViewModel();

            return PartialView("TMNReport", tMNReportViewModel);
        }
    }
}