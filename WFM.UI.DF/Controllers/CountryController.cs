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
    //[Authorize]
    public class CountryController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly CountryService countryService = new CountryService();

        public CountryController()
        {
        }

        public CountryController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Country
        public ActionResult Index(int? id)
        {
            WFM_Country country = new WFM_Country();
            if (id != null)
            {
                country = countryService.GetCountryById(id);
            }
            return View(country);
        }

        public ActionResult GetList()
        {
            List<WFM_Country> list = countryService.GetCountryList();

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
        public ActionResult SaveOrUpdate(WFM_Country model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_Country country = null;
                WFM_Country oldCountry = null;
                if (model.Id == 0)
                {
                    country = new WFM_Country
                    {
                        Name = model.Name,
                        IsActive = true
                    };

                    oldCountry = new WFM_Country();
                    oldData = new JavaScriptSerializer().Serialize(oldCountry);
                    newData = new JavaScriptSerializer().Serialize(country);
                }
                else
                {
                    country = countryService.GetCountryById(model.Id);
                    oldCountry = countryService.GetCountryById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_Country()
                    {
                        Id = oldCountry.Id,
                        Name = oldCountry.Name,
                        IsActive = oldCountry.IsActive
                    });

                    country.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    country.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_Country()
                    {
                        Id = country.Id,
                        Name = country.Name,
                        IsActive = country.IsActive
                    });
                }

                countryService.SaveOrUpdate(country);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "Country",
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


            return RedirectToAction("Index", "Country");
        }
    }
}