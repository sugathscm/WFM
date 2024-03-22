using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF.Models;
using WFM.BAL.ViewModels;
using WFM.UI.DF.ModelsView;
using WFM.BAL.Helpers;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class PublicSectorTenderController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly ProjectService projectService = new ProjectService();
        private readonly ProjectSectorService sectorService = new ProjectSectorService();
        private readonly DocumentService documentService = new DocumentService();
        private readonly ProjectDocumentService projectDocumentService = new ProjectDocumentService();
        private readonly SourcingPartnerService sourcingPartnerService = new SourcingPartnerService();
        private readonly PrincipalService principalService = new PrincipalService();
        private readonly DesignationService designationService = new DesignationService();
        private readonly DivisionService divisionService = new DivisionService();
        private readonly ProjectSectorService projectSectorService = new ProjectSectorService();
        private readonly PublicSectorTenderService publicSectorTenderService = new PublicSectorTenderService();
        private readonly GenericService genericService = new GenericService();

        private int projectTypeId = 4;

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

        public ActionResult Index(int id)
        {
            projectTypeId = id;
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
                ViewBag.Principals = principalService.GetPrincipalList();
                ViewBag.SourcingPartners = sourcingPartnerService.GetSourdingPartnerList();
                ViewBag.DesignationList = designationService.GetDesignationList();
                ViewBag.DivisionList = divisionService.GetDivisionList();
                ViewBag.ProjectSectorList = projectSectorService.GetProjectSectorList();
                ViewBag.DesignationList = designationService.GetDesignationList();

                ProjectTenderViewModel projectTenderViewModel = new ProjectTenderViewModel();
                PropertyCopier<WFM_Project, ProjectTenderViewModel>.Copy(project, projectTenderViewModel);
                
                WFM_Form8B wFM_Form8B = genericService.GetList<WFM_Form8B>().Where(f => f.ProjectId == id).FirstOrDefault();
                if(wFM_Form8B == null)
                {
                    projectTenderViewModel.Form8BViewModel = new Form8BViewModel();
                    projectTenderViewModel.Form8BGenerated = false;
                }
                else
                {
                    Form8BViewModel form8BViewModel = new Form8BViewModel();
                    PropertyCopier<WFM_Form8B, Form8BViewModel>.Copy(wFM_Form8B, form8BViewModel);
                    form8BViewModel.Form8BId = wFM_Form8B.Id;
                    wFM_Form8B.ProjectId = id;
                    projectTenderViewModel.Form8BViewModel = form8BViewModel;
                    projectTenderViewModel.Form8BGenerated = true;
                }

                WFM_Form12B wFM_Form12B = genericService.GetList<WFM_Form12B>().Where(f => f.ProjectId == id).FirstOrDefault();
                if (wFM_Form12B == null)
                {
                    projectTenderViewModel.Form12ViewModel = new Form12ViewModel();
                    projectTenderViewModel.Form12Generated = false;
                }
                else
                {
                    Form12ViewModel form12ViewModel = new Form12ViewModel();
                    PropertyCopier<WFM_Form12B, Form12ViewModel>.Copy(wFM_Form12B, form12ViewModel);
                    form12ViewModel.Form12Id = wFM_Form12B.Id;
                    wFM_Form12B.ProjectId = id;
                    projectTenderViewModel.Form12ViewModel = form12ViewModel;
                    projectTenderViewModel.Form12Generated = true;
                }

                return View(projectTenderViewModel);
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

        public ActionResult SaveOrUpdate(FormCollection formCollection)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    var documents = documentService.GetDocumentsByProjectType(projectTypeId);

                    var docs = documents.Where(d => d.DocumentTabId == 1 && d.HasFields == false).OrderBy(d => d.DisplayOrder).ToList();
                    int i = 0;

                    List<TenderDocumnet> files = new List<TenderDocumnet>();

                    //List<TenderDocumnet> files = Request.Files.AllKeys.Select(x => new TenderDocumnet () { PostedFile = Request.Files[i] }).Where(x => x.PostedFile.ContentLength > 0).ToList();

                    for (i = 0; i < Request.Files.Count; i++)
                    {
                        if (Request.Files[i].ContentLength > 0)
                            files.Add(new TenderDocumnet() { DocumentTypeId = Request.Files.AllKeys[i], PostedFile = Request.Files[i] });
                    }

                    string id = formCollection["projectid"];

                    foreach (var file in files)
                    {
                        string path = Path.Combine(Server.MapPath("~/Docs/" + id + "/" + file.DocumentTypeId));
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (file != null)
                        {
                            string fileName = Path.GetFileName(file.PostedFile.FileName);
                            file.PostedFile.SaveAs(path + "/" + fileName.Replace(" ", "_"));
                            ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                        }
                        //DisplayDocuments(int.Parse(id), int.Parse(file.DocumentTypeId), 0);
                    }

                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        public PartialViewResult DisplayDocuments(int projectid, int documentid)
        {
            List<TenderDocumnet> docs = new List<TenderDocumnet>();

            string path = Path.Combine(Server.MapPath("~/Docs/" + projectid + "/" + documentid));
            if (Directory.Exists(path))
            {
                var fileInfoList = Directory.GetFiles(path);
                foreach (var file in fileInfoList)
                {
                    string filename = Path.GetFileName(file);
                    docs.Add(new TenderDocumnet() { DocumentPath = "../../Docs/" + projectid + "/" + documentid + "/" + filename, Name = filename });
                }
            }

            return PartialView("~/Views/PublicSectorTender/DisplayDocuments.cshtml", docs);
        }

        public PartialViewResult DisplayDocumentsPerTab(int projectid, int tabId)
        {
            List<TenderDocumnet> docs = new List<TenderDocumnet>();

            var documents = documentService.GetDocumentsByProjectType(projectTypeId);
            List<WFM_Document> documentList = documents.Where(d => d.DocumentTabId == tabId && d.HasFields == false).OrderBy(d => d.DisplayOrder).ToList();

            foreach (var document in documentList)
            {
                string path = Path.Combine(Server.MapPath("~/Docs/" + projectid + "/" + document.Id));
                if (Directory.Exists(path))
                {
                    var fileInfoList = Directory.GetFiles(path);
                    foreach (var file in fileInfoList)
                    {
                        string filename = Path.GetFileName(file);
                        docs.Add(new TenderDocumnet() { DocumentPath = "../../Docs/" + projectid + "/" + document.Id + "/" + filename, Name = filename });
                    }
                }
            }

            return PartialView("~/Views/PublicSectorTender/DisplayDocumentsPerTab.cshtml", docs);
        }

        //public ActionResult Form8B(int id)
        //{
        //    Form8BViewModel form8BViewModel = new Form8BViewModel();
        //    //Report model = reportService.GetReportById(id);

        //    //SetData(reportViewModel, model, true);
        //    //reportViewModel.Patient = patientService.GetPatientById(model.PatientId);
        //    ViewBag.DesignationList = designationService.GetDesignationList();
        //    ViewBag.DivisionList = divisionService.GetDivisionList();
        //    return PartialView("_Form8B");
        //}

        public ActionResult SaveOrUpdateForm8B(Form8BViewModel formCollection)
        {
            WFM_Form8B wFM_Form8B = new WFM_Form8B();
            
            PropertyCopier<Form8BViewModel, WFM_Form8B>.Copy(formCollection, wFM_Form8B);

            if (wFM_Form8B.ProjectId == null)
            {
                wFM_Form8B.ProjectId = wFM_Form8B.Id;
                wFM_Form8B.Id = 0;
            }
            else
            {
                wFM_Form8B.Id = formCollection.Form8BId;
            }
            publicSectorTenderService.SaveOrUpdate(wFM_Form8B);

            if (Request.Files.Count > 0)
            {

            }

            return Json("File Uploaded Successfully!");
        }

        public ActionResult SaveOrUpdateForm12(Form12ViewModel formCollection)
        {
            WFM_Form12B wFM_Form12B = new WFM_Form12B();

            PropertyCopier<Form12ViewModel, WFM_Form12B>.Copy(formCollection, wFM_Form12B);

            if (wFM_Form12B.ProjectId == null)
            {
                wFM_Form12B.ProjectId = wFM_Form12B.Id;
                wFM_Form12B.Id = 0;
            }
            else
            {
                wFM_Form12B.Id = formCollection.Form12Id;
            }
            publicSectorTenderService.SaveOrUpdate(wFM_Form12B);

            if (Request.Files.Count > 0)
            {

            }

            return Json("File Uploaded Successfully!");
        }

        public Form8BViewModel GetForm8B(int? id)
        {
            Form8BViewModel form8BViewModel = new Form8BViewModel();

            WFM_Form8B wFM_Form8B = genericService.GetList<WFM_Form8B>().Where(f8 => f8.Id == id).FirstOrDefault();
            PropertyCopier<WFM_Form8B, Form8BViewModel>.Copy(wFM_Form8B, form8BViewModel);

            return form8BViewModel;
        }
    }
}