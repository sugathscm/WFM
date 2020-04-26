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
    public class CountryController : Controller
    {
        private ApplicationUserManager _userManager;

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
            Country country = new Country();
            if (id != null)
            {
                using (LinkManagementEntities entities = new LinkManagementEntities())
                {
                    country = entities.Countries.Where(o => o.Id == id).SingleOrDefault();
                }
            }
            return View(country);
        }

        public ActionResult GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                var list = entities.Countries.OrderBy(o => o.Name).ToList();

                List<Country> modelList = new List<Country>();

                foreach (var item in list)
                {
                    modelList.Add(new Country() { Id = item.Id, IsActive = item.IsActive, Name = item.Name });
                }

                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(Country model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                try
                {
                    int id = model.Id;
                    Country country = null;
                    Country oldCountry = null;
                    if (model.Id == 0)
                    {
                        country = new Country
                        {
                            Name = model.Name,
                            IsActive = true
                        };

                        entities.Countries.Add(country);
                        entities.SaveChanges();

                        oldCountry = new Country();
                        oldData = new JavaScriptSerializer().Serialize(oldCountry);
                        newData = new JavaScriptSerializer().Serialize(country);
                    }
                    else
                    {
                        country = entities.Countries.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldCountry = entities.Countries.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new Country()
                        {
                            Id = oldCountry.Id,
                            Name = oldCountry.Name,
                            IsActive = oldCountry.IsActive
                        });

                        country.Name = model.Name;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        country.IsActive = model.IsActive;

                        newData = new JavaScriptSerializer().Serialize(new Country()
                        {
                            Id = country.Id,
                            Name = country.Name,
                            IsActive = country.IsActive
                        });

                        entities.Entry(country).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

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
            }

            return RedirectToAction("Index", "Country");
        }
    }
}