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
    public class ContactController : Controller
    {
        private ApplicationUserManager _userManager;

        public ContactController()
        {
        }
        public ContactController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
            Contact contact = new Contact();

            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                if (id != null)
                {
                    contact = entities.Contacts.Where(o => o.Id == id).SingleOrDefault();
                }
                //ViewBag.PrincipalContactList = entities.PrincipalContacts.Where(o => o.ParentId == 0).OrderBy(o => o.Name).ToList();
            }
            return View(contact);
        }

        public ActionResult GetList()
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                var list = entities.Contacts.OrderBy(o => o.Name).ToList();
                List<ContactView> modelList = new List<ContactView>();
                foreach (var item in list)
                {
                    modelList.Add(new ContactView()
                    {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        Name = item.Name,
                        Mobile = item.Mobile,
                        Email = item.Email,
                        FixedLine = item.FixedLine,
                        DesignationName = (item.DesignationId == 0) ? "" : entities.Designations.Where(o => o.Id == item.DesignationId).SingleOrDefault().Name,
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
                    Contact contact = null;
                    Contact oldContact = null;
                    if (model.Id == 0)
                    {
                        contact = new Contact
                        {
                            Name = model.Name,
                            IsActive = true
                        };

                        entities.Contacts.Add(contact);
                        entities.SaveChanges();

                        oldContact = new Contact();
                        oldData = new JavaScriptSerializer().Serialize(oldContact);
                        newData = new JavaScriptSerializer().Serialize(contact);
                    }
                    else
                    {
                        contact = entities.Contacts.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldContact = entities.Contacts.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new Contact()
                        {
                            Id = oldContact.Id,
                            Name = oldContact.Name,
                            IsActive = oldContact.IsActive
                        });

                        contact.Name = model.Name;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        contact.IsActive = model.IsActive;

                        newData = new JavaScriptSerializer().Serialize(new Contact()
                        {
                            Id = contact.Id,
                            Name = contact.Name,
                            IsActive = contact.IsActive
                        });

                        entities.Entry(contact).State = System.Data.Entity.EntityState.Modified;
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