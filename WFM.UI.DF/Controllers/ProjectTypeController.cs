using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF.Models;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class ProjectTypeController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly ProjectTypeService projectTypeService = new ProjectTypeService();

        public ProjectTypeController()
        {
        }

        public ProjectTypeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: ProjectType
        public ActionResult Index(int? id)
        {
            WFM_ProjectType projectType = new WFM_ProjectType();
            if (id != null)
            {
                projectType = projectTypeService.GetProjectTypeById(id);
            }
            return View(projectType);
        }

        public ActionResult GetList()
        {
            List<WFM_ProjectType> list = projectTypeService.GetProjectTypeList();

            List<BaseViewModel> modelList = new List<BaseViewModel>();

            foreach (var item in list)
            {
                modelList.Add(new BaseViewModel() { Id = item.Id, IsActive = item.IsActive, Name = item.Name });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_ProjectType model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_ProjectType projectType = null;
                WFM_ProjectType oldProjectType = null;
                if (model.Id == 0)
                {
                    projectType = new WFM_ProjectType
                    {
                        Name = model.Name,
                        IsActive = true
                    };

                    oldProjectType = new WFM_ProjectType();
                    oldData = new JavaScriptSerializer().Serialize(oldProjectType);
                    newData = new JavaScriptSerializer().Serialize(projectType);
                }
                else
                {
                    projectType = projectTypeService.GetProjectTypeById(model.Id);
                    oldProjectType = projectTypeService.GetProjectTypeById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_ProjectType()
                    {
                        Id = oldProjectType.Id,
                        Name = oldProjectType.Name,
                        IsActive = oldProjectType.IsActive
                    });

                    projectType.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    projectType.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_ProjectType()
                    {
                        Id = projectType.Id,
                        Name = projectType.Name,
                        IsActive = projectType.IsActive
                    });
                }

                projectTypeService.SaveOrUpdate(projectType);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "ProjectType",
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


            return RedirectToAction("Index", "ProjectType");
        }
    }
}