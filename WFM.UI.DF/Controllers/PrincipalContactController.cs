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
using WFM.UI.ModelsView;

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

        // GET: Contact
        public ActionResult Index(int? id)
        {
            Contact projectSector = new Contact();

            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                if (id != null)
                {
                    projectSector = entities.Contacts.Where(o => o.Id == id).SingleOrDefault();
                }
                //ViewBag.PrincipalContactList = entities.PrincipalContacts.Where(o => o.ParentId == 0).OrderBy(o => o.Name).ToList();
            }
            return View(projectSector);
        }

        public ActionResult GetList()
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                var list = entities.Contacts.OrderBy(o => o.Name).ToList();
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
        public ActionResult SaveOrUpdate(Contact model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                try
                {
                    int id = model.Id;
                    Contact projectSector = null;
                    Contact oldPrincipalContact = null;
                    if (model.Id == 0)
                    {
                        projectSector = new Contact
                        {
                            Name = model.Name,
                            IsActive = true
                        };

                        entities.Contacts.Add(projectSector);
                        entities.SaveChanges();

                        oldPrincipalContact = new Contact();
                        oldData = new JavaScriptSerializer().Serialize(oldPrincipalContact);
                        newData = new JavaScriptSerializer().Serialize(projectSector);
                    }
                    else
                    {
                        projectSector = entities.Contacts.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldPrincipalContact = entities.Contacts.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new Contact()
                        {
                            Id = oldPrincipalContact.Id,
                            Name = oldPrincipalContact.Name,
                            IsActive = oldPrincipalContact.IsActive
                        });

                        projectSector.Name = model.Name;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        projectSector.IsActive = model.IsActive;

                        newData = new JavaScriptSerializer().Serialize(new Contact()
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
                        Entity = "Contact",
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

            return RedirectToAction("Index", "Contact");
        }
    }
}