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

    public class OrganizationController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly OrganizationService organizationService = new OrganizationService();
        private readonly DesignationService designationService = new DesignationService();

        public OrganizationController()
        {
        }
        public OrganizationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: WFM_Organization
        public ActionResult Index(int? id)
        {
            WFM_Organization organization = new WFM_Organization();

            if (id != null)
            {
                organization = organizationService.GetOrganizationById(id);
            }
            ViewBag.DesignationList = designationService.GetDesignationList();

            return View(organization);
        }

        public ActionResult GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                var list = organizationService.GetOrganizationList();

                List<OrganizationView> modelList = new List<OrganizationView>();
                foreach (var item in list)
                {
                    modelList.Add(new OrganizationView()
                    {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        AddressLine1 = item.AddressLine1,
                        AddressLine2 = item.AddressLine2,
                        Name = item.Name,
                        LandLine  = item.LandLine,
                        Email = item.Email,
                        FixedLine = item.FixedLine
                    });
                }
                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_Organization model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_Organization organization = null;
                WFM_Organization oldOrganization = null;
                if (model.Id == 0)
                {
                    organization = new WFM_Organization
                    {
                        AddressLine1 = model.AddressLine1,
                        Name = model.Name,
                        AddressLine2 = model.AddressLine2,
                        Email = model.Email,
                        FixedLine = model.FixedLine,
                        IsActive = true,
                        LandLine = model.LandLine,
                        DateCreated  = DateTime.Now
                    };

                    oldOrganization = new WFM_Organization();
                    oldData = new JavaScriptSerializer().Serialize(oldOrganization);
                    newData = new JavaScriptSerializer().Serialize(organization);
                }
                else
                {
                    organization = organizationService.GetOrganizationById(model.Id);
                    oldOrganization = organizationService.GetOrganizationById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_Organization()
                    {
                        Id = oldOrganization.Id,
                        AddressLine1 = oldOrganization.AddressLine1,
                        Name = oldOrganization.Name,
                        AddressLine2 = oldOrganization.AddressLine2,
                        Email = oldOrganization.Email,
                        FixedLine = oldOrganization.FixedLine,
                        LandLine = oldOrganization.LandLine,
                        IsActive = oldOrganization.IsActive
                    });

                    organization.AddressLine1 = model.AddressLine1;
                    organization.Name = model.Name;
                    organization.AddressLine2 = model.AddressLine2;
                    organization.Email = model.Email;
                    organization.FixedLine = model.FixedLine;
                    organization.LandLine = model.LandLine;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    organization.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_Organization()
                    {
                        AddressLine1 = model.AddressLine1,
                        Name = model.Name,
                        AddressLine2 = model.AddressLine2,
                        Email = model.Email,
                        FixedLine = model.FixedLine,
                        IsActive = true,
                        LandLine = model.LandLine
                    });
                }

                organizationService.SaveOrUpdate(organization);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "WFM_Organization",
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
            return RedirectToAction("Index", "Organization");
        }
    }

}