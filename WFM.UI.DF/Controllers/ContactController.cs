using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly ContactService contactService = new ContactService();
        private readonly DesignationService designationService = new DesignationService();

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

        // GET: WFM_Contact
        public ActionResult Index(int? id)
        {
            WFM_Contact contact = new WFM_Contact();

            if (id != null)
            {
                contact = contactService.GetContactById(id);
            }
            ViewBag.DesignationList = designationService.GetDesignationList();

            return View(contact);
        }

        public ActionResult GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                var list = contactService.GetContactList();

                List<ContactView> modelList = new List<ContactView>();
                foreach (var item in list)
                {
                    modelList.Add(new ContactView()
                    {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        Title = item.Title,
                        Name = item.Name,
                        Mobile = item.Mobile,
                        Email = item.Email,
                        FixedLine = item.FixedLine,
                        DesignationName = (item.DesignationId == 0) ? "" : entities.WFM_Designation.Where(o => o.Id == item.DesignationId).SingleOrDefault().Name,
                    });
                }
                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_Contact model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_Contact contact = null;
                WFM_Contact oldContact = null;
                if (model.Id == 0)
                {
                    contact = new WFM_Contact
                    {
                        Title = model.Title,
                        Name = model.Name,
                        Mobile = model.Mobile,
                        Email = model.Email,
                        FixedLine = model.FixedLine,
                        IsActive = true,
                        DesignationId = model.DesignationId
                    };

                    oldContact = new WFM_Contact();
                    oldData = new JavaScriptSerializer().Serialize(oldContact);
                    newData = new JavaScriptSerializer().Serialize(contact);
                }
                else
                {
                    contact = contactService.GetContactById(model.Id);
                    oldContact = contactService.GetContactById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_Contact()
                    {
                        Id = oldContact.Id,
                        Title = oldContact.Title,
                        Name = oldContact.Name,
                        Mobile = oldContact.Mobile,
                        Email = oldContact.Email,
                        FixedLine = oldContact.FixedLine,
                        DesignationId = oldContact.DesignationId,
                        IsActive = oldContact.IsActive
                    });

                    contact.Title = model.Title;
                    contact.Name = model.Name;
                    contact.Mobile = model.Mobile;
                    contact.Email = model.Email;
                    contact.FixedLine = model.FixedLine;
                    contact.DesignationId = model.DesignationId;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    contact.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_Contact()
                    {
                        Id = contact.Id,
                        Title = contact.Title,
                        Name = contact.Name,
                        Mobile = contact.Mobile,
                        Email = contact.Email,
                        FixedLine = contact.FixedLine,
                        DesignationId = contact.DesignationId,
                        IsActive = contact.IsActive
                    });
                }

                contactService.SaveOrUpdate(contact);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "WFM_Contact",
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
            return RedirectToAction("Index", "Contact");
        }
    }
}