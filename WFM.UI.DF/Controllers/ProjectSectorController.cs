using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.DAL;
using WFM.UI.DF;
using WFM.UI.ModelsView;

namespace WFM.UI.Controllers
{
    //[Authorize]
    public class ProjectSectorController : Controller
    {
        private ApplicationUserManager _userManager;

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

        // GET: ProjectSector
        public ActionResult Index(int? id)
        {
            ProjectSector projectSector = new ProjectSector();

            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                if (id != null)
                {
                    projectSector = entities.ProjectSectors.Where(o => o.Id == id).SingleOrDefault();
                }
                ViewBag.ProjectSectorList = entities.ProjectSectors.Where(o => o.ParentId == 0).OrderBy(o => o.Name).ToList();
            }
            return View(projectSector);
        }

        public ActionResult GetList()
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                var list = entities.ProjectSectors.OrderBy(o => o.Name).ToList();
                List<ProjectSectorView> modelList = new List<ProjectSectorView>();
                foreach (var item in list)
                {
                    modelList.Add(new ProjectSectorView() {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        Name = item.Name,
                        ParentName = (item.ParentId == 0) ? "" : entities.ProjectSectors.Where(o => o.Id == item.ParentId).SingleOrDefault().Name });
                }
                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(ProjectSector model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                try
                {
                    int id = model.Id;
                    ProjectSector projectSector = null;
                    ProjectSector oldProjectSector = null;
                    if (model.Id == 0)
                    {
                        projectSector = new ProjectSector
                        {
                            Name = model.Name,
                            IsActive = true,
                            ParentId = (model.ParentId == null) ? 0 : model.ParentId
                        };

                        entities.ProjectSectors.Add(projectSector);
                        entities.SaveChanges();

                        oldProjectSector = new ProjectSector();
                        oldData = new JavaScriptSerializer().Serialize(oldProjectSector);
                        newData = new JavaScriptSerializer().Serialize(projectSector);
                    }
                    else
                    {
                        projectSector = entities.ProjectSectors.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldProjectSector = entities.ProjectSectors.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new ProjectSector()
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

                        newData = new JavaScriptSerializer().Serialize(new ProjectSector()
                        {
                            Id = projectSector.Id,
                            Name = projectSector.Name,
                            IsActive = projectSector.IsActive,
                            ParentId = projectSector.ParentId
                        });

                        entities.Entry(projectSector).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

                    //CommonService.SaveDataAudit(new DataAudit()
                    //{
                    //    Entity = "ProjectSector",
                    //    NewData = newData,
                    //    OldData = oldData,
                    //    UpdatedOn = DateTime.Now,
                    //    UserId = new Guid(User.Identity.GetUserId())
                    //});

                    TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
                }
            }

            return RedirectToAction("Index", "ProjectSector");
        }
    }
}