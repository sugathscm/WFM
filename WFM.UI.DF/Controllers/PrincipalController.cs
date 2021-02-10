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
using WFM.UI.DF.Models;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    public class PrincipalController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly PrincipalService principalService = new PrincipalService();
        private readonly CountryService countryService = new CountryService();


        public PrincipalController()
        {
        }
        public PrincipalController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: WFM_Principal
        public ActionResult Index(int? id)
        {
            WFM_Principal principal = new WFM_Principal();
            if (id != null)
            {
                principal = principalService.GetPrincipalById(id);
            }

            ViewBag.CountryList = countryService.GetCountryList();

            return View(principal);
        }

        public ActionResult GetList()
        {
            var list = principalService.GetPrincipalList();
            List<PrincipalView> modelList = new List<PrincipalView>();
            foreach (var item in list)
            {
                modelList.Add(new PrincipalView()
                {
                    Id = item.Id,
                    AddressLine1 = item.AddressLine1,
                    AddressLine2 = item.AddressLine2,
                    City = item.City,
                    Province = item.Province,
                    Postcode = item.Postcode,
                    Email = item.Email,
                    Website = item.Website,
                    IsActive = item.IsActive,
                    CountryName = item.WFM_Country.Name,
                    Name = item.Name
                });
            }

            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_Principal model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_Principal principal = null;
                WFM_Principal oldPrincipal = null;
                if (model.Id == 0)
                {
                    principal = new WFM_Principal
                    {
                        Name = model.Name,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        City = model.City,
                        Province = model.Province,
                        Postcode = model.Postcode,
                        CountryId = model.CountryId,
                        Email = model.Email,
                        Website = model.Website,
                        IsActive = true
                    };

                    oldPrincipal = new WFM_Principal();
                    oldData = new JavaScriptSerializer().Serialize(oldPrincipal);
                    newData = new JavaScriptSerializer().Serialize(principal);
                }
                else
                {
                    principal = principalService.GetPrincipalById(model.Id);
                    oldPrincipal = principalService.GetPrincipalById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_Principal()
                    {
                        Id = oldPrincipal.Id,
                        Name = oldPrincipal.Name,
                        AddressLine1 = oldPrincipal.AddressLine1,
                        AddressLine2 = oldPrincipal.AddressLine2,
                        City = oldPrincipal.City,
                        Province = oldPrincipal.Province,
                        Postcode = oldPrincipal.Postcode,
                        CountryId = oldPrincipal.CountryId,
                        Email = oldPrincipal.Email,
                        Website = oldPrincipal.Website,
                        IsActive = oldPrincipal.IsActive
                    });

                    principal.Name = model.Name;
                    principal.AddressLine1 = model.AddressLine1;
                    principal.AddressLine2 = model.AddressLine2;
                    principal.City = model.City;
                    principal.Postcode = model.Postcode;
                    principal.Province = model.Province;
                    principal.CountryId = model.CountryId;
                    principal.Website = model.Website;
                    principal.Email = model.Email;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    principal.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_Principal()
                    {
                        Id = principal.Id,
                        Name = principal.Name,
                        AddressLine1 = principal.AddressLine1,
                        AddressLine2 = principal.AddressLine2,
                        City = principal.City,
                        Province = principal.Province,
                        Postcode = principal.Postcode,
                        CountryId = principal.CountryId,
                        Email = principal.Email,
                        Website = principal.Website,
                        IsActive = principal.IsActive
                    });
                }

                principalService.SaveOrUpdate(principal);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "WFM_Principal",
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
            return RedirectToAction("Index", "Principal");
        }
    }
}