using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL;
using WFM.DAL;
using WFM.UI.DF;

namespace WFM.UI.DF.Controllers
{
    //[Authorize]
    public class DesignationController : Controller
    {
        private ApplicationUserManager _userManager;

        public DesignationController()
        {
        }

        public DesignationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Designation
        public ActionResult Index(int? id)
        {
            WFM_Designation designation = new WFM_Designation();
            if (id != null)
            {
                using (LinkManagementEntities entities = new LinkManagementEntities())
                {
                    designation = entities.WFM_Designation.Where(o => o.Id == id).SingleOrDefault();
                }
            }
            return View(designation);
        }

        public ActionResult GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                var list = entities.WFM_Designation.OrderBy(o => o.Name).ToList();

                List<WFM_Designation> modelList = new List<WFM_Designation>();

                foreach (var item in list)
                {
                    modelList.Add(new WFM_Designation() { Id = item.Id, IsActive = item.IsActive, Name = item.Name });
                }

                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(Designation model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                try
                {
                    int id = model.Id;
                    WFM_Designation designation = null;
                    WFM_Designation oldDesignation = null;
                    if (model.Id == 0)
                    {
                        designation = new WFM_Designation
                        {
                            Name = model.Name,
                            IsActive = true
                        };

                        entities.WFM_Designation.Add(designation);
                        entities.SaveChanges();

                        oldDesignation = new WFM_Designation();
                        oldData = new JavaScriptSerializer().Serialize(oldDesignation);
                        newData = new JavaScriptSerializer().Serialize(designation);
                    }
                    else
                    {
                        designation = entities.WFM_Designation.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldDesignation = entities.WFM_Designation.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new Designation()
                        {
                            Id = oldDesignation.Id,
                            Name = oldDesignation.Name,
                            IsActive = oldDesignation.IsActive
                        });

                        designation.Name = model.Name;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        designation.IsActive = model.IsActive.Value;

                        newData = new JavaScriptSerializer().Serialize(new Designation()
                        {
                            Id = designation.Id,
                            Name = designation.Name,
                            IsActive = designation.IsActive
                        });

                        entities.Entry(designation).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

                    CommonService.SaveDataAudit(new DataAudit()
                    {
                        Entity = "Designation",
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
            }

            return RedirectToAction("Index", "Designation");
        }
    }
}