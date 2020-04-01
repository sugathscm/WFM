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
using WFM.UI.ModelsView;
using WFM.UI.Services;

namespace WFM.UI.Controllers
{
    public class PrincipalContactController : Controller
    {
        private ApplicationUserManager _userManager;

        public PrincipalContactController()
        {
        }
        public PrincipalContactController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: PrincipalContact
        public ActionResult Index(int? id)
        {
            PrincipalContact projectSector = new PrincipalContact();

            using (WFMContext entities = new WFMContext())
            {
                if (id != null)
                {
                    projectSector = entities.PrincipalContacts.Where(o => o.Id == id).SingleOrDefault();
                }
                //ViewBag.PrincipalContactList = entities.PrincipalContacts.Where(o => o.ParentId == 0).OrderBy(o => o.Name).ToList();
            }
            return View(projectSector);
        }

        public ActionResult GetList()
        {
            using (WFMContext entities = new WFMContext())
            {
                var list = entities.PrincipalContacts.OrderBy(o => o.Name).ToList();
                List<PrincipalContactView> modelList = new List<PrincipalContactView>();
                foreach (var item in list)
                {
                    modelList.Add(new PrincipalContactView()
                    {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        Name = item.Name
                    });
                }
                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(PrincipalContact model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (WFMContext entities = new WFMContext())
            {
                try
                {
                    int id = model.Id;
                    PrincipalContact projectSector = null;
                    PrincipalContact oldPrincipalContact = null;
                    if (model.Id == 0)
                    {
                        projectSector = new PrincipalContact
                        {
                            Name = model.Name,
                            IsActive = true
                        };

                        entities.PrincipalContacts.Add(projectSector);
                        entities.SaveChanges();

                        oldPrincipalContact = new PrincipalContact();
                        oldData = new JavaScriptSerializer().Serialize(oldPrincipalContact);
                        newData = new JavaScriptSerializer().Serialize(projectSector);
                    }
                    else
                    {
                        projectSector = entities.PrincipalContacts.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldPrincipalContact = entities.PrincipalContacts.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new PrincipalContact()
                        {
                            Id = oldPrincipalContact.Id,
                            Name = oldPrincipalContact.Name,
                            IsActive = oldPrincipalContact.IsActive
                        });

                        projectSector.Name = model.Name;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        projectSector.IsActive = model.IsActive;

                        newData = new JavaScriptSerializer().Serialize(new PrincipalContact()
                        {
                            Id = projectSector.Id,
                            Name = projectSector.Name,
                            IsActive = projectSector.IsActive
                        });

                        entities.Entry(projectSector).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

                    CommonService.SaveDataAudit(new DataAudit()
                    {
                        Entity = "PrincipalContact",
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

            return RedirectToAction("Index", "PrincipalContact");
        }
    }
}