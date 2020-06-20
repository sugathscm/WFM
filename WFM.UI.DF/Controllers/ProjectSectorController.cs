using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    //[Authorize]
    public class ProjectSectorController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ProjectSectorService projectSectorService = new ProjectSectorService();

        public ProjectSectorController()
        {
        }
        public ProjectSectorController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: WFM_ProjectSector
        public ActionResult Index(int? id)
        {
            WFM_ProjectSector projectSector = new WFM_ProjectSector();

            if (id != null)
            {
                projectSector = projectSectorService.GetProjectSectorById(id);
            }

            ViewBag.ProjectSectorList = projectSectorService.GetProjectSectorList().Where(o => o.ParentId == 0).OrderBy(o => o.Name).ToList();

            return View(projectSector);
        }

        public ActionResult GetList()
        {
            var list = projectSectorService.GetProjectSectorList();
            List<ProjectSectorView> modelList = new List<ProjectSectorView>();
            foreach (var item in list)
            {
                modelList.Add(new ProjectSectorView()
                {
                    Id = item.Id,
                    IsActive = item.IsActive,
                    Name = item.Name,
                    ParentName = (item.ParentId == 0) ? "" : list.Where(o => o.Id == item.ParentId).SingleOrDefault().Name
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_ProjectSector model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_ProjectSector projectSector = null;
                WFM_ProjectSector oldProjectSector = null;
                if (model.Id == 0)
                {
                    projectSector = new WFM_ProjectSector
                    {
                        Name = model.Name,
                        IsActive = true,
                        ParentId = (model.ParentId == null) ? 0 : model.ParentId
                    };

                    oldProjectSector = new WFM_ProjectSector();
                    oldData = new JavaScriptSerializer().Serialize(oldProjectSector);
                    newData = new JavaScriptSerializer().Serialize(projectSector);
                }
                else
                {
                    projectSector = projectSectorService.GetProjectSectorById(model.Id);
                    oldProjectSector = projectSectorService.GetProjectSectorById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_ProjectSector()
                    {
                        Id = oldProjectSector.Id,
                        Name = oldProjectSector.Name,
                        IsActive = oldProjectSector.IsActive,
                        ParentId = oldProjectSector.ParentId
                    });

                    projectSector.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    projectSector.IsActive = model.IsActive;
                    projectSector.ParentId = (model.ParentId == null) ? 0 : model.ParentId;

                    newData = new JavaScriptSerializer().Serialize(new WFM_ProjectSector()
                    {
                        Id = projectSector.Id,
                        Name = projectSector.Name,
                        IsActive = projectSector.IsActive,
                        ParentId = projectSector.ParentId
                    });
                }

                projectSectorService.SaveOrUpdate(projectSector);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "WFM_ProjectSector",
                    NewData = newData,
                    OldData = oldData,
                    UpdatedOn = DateTime.Now,
                    UserId = new Guid(User.Identity.GetUserId())
                });

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
            }

            return RedirectToAction("Index", "ProjectSector");
        }
    }
}