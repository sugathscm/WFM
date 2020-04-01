using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.UI.DAL;
using WFM.UI.Models;
using WFM.UI.Services;

namespace WFM.UI.Controllers
{
    [Authorize]
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
            Designation designation = new Designation();
            if (id != null)
            {
                using (WFMContext entities = new WFMContext())
                {
                    designation = entities.Designations.Where(o => o.Id == id).SingleOrDefault();
                }
            }
            return View(designation);
        }

        public ActionResult GetList()
        {
            using (WFMContext entities = new WFMContext())
            {
                var list = entities.Designations.OrderBy(o => o.Name).ToList();

                List<Designation> modelList = new List<Designation>();

                foreach (var item in list)
                {
                    modelList.Add(new Designation() { Id = item.Id, IsActive = item.IsActive, Name = item.Name });
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
            using (WFMContext entities = new WFMContext())
            {
                try
                {
                    int id = model.Id;
                    Designation designation = null;
                    Designation oldDesignation = null;
                    if (model.Id == 0)
                    {
                        designation = new Designation
                        {
                            Name = model.Name,
                            IsActive = true
                        };

                        entities.Designations.Add(designation);
                        entities.SaveChanges();

                        oldDesignation = new Designation();
                        oldData = new JavaScriptSerializer().Serialize(oldDesignation);
                        newData = new JavaScriptSerializer().Serialize(designation);
                    }
                    else
                    {
                        designation = entities.Designations.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldDesignation = entities.Designations.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new Designation()
                        {
                            Id = oldDesignation.Id,
                            Name = oldDesignation.Name,
                            IsActive = oldDesignation.IsActive
                        });

                        designation.Name = model.Name;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        designation.IsActive = model.IsActive;

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