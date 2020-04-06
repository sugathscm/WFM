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
using WFM.UI.DF.Models;
using WFM.UI.ModelsView;

namespace WFM.UI.Controllers
{
    public class PrincipalController : Controller
    {
        private ApplicationUserManager _userManager;

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

        // GET: Principal
        public ActionResult Index(int? id)
        {
            Principal principal = new Principal();

            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                if (id != null)
                {
                    principal = entities.Principals.Where(o => o.Id == id).SingleOrDefault();
                }
                //ViewBag.PrincipalList = entities.Principals.Where(o => o.ParentId == 0).OrderBy(o => o.Name).ToList();
                var listData = entities.Countries.Select(s => new { Id = s.Id, Value = s.Name }).ToList();
                ViewBag.ListObject = new SelectList(listData, "Id", "Value");
            }
            return View(principal);
        }

        public ActionResult GetList()
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                var list = entities.Principals.Include("Country").OrderBy(o => o.Name).ToList();
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
                        CountryName = item.Country.Name,
                        Name = item.Name
                    });
                }
                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(Principal model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                try
                {
                    int id = model.Id;
                    Principal principal = null;
                    Principal oldPrincipal = null;
                    if (model.Id == 0)
                    {
                        principal = new Principal
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

                        entities.Principals.Add(principal);
                        entities.SaveChanges();

                        oldPrincipal = new Principal();
                        oldData = new JavaScriptSerializer().Serialize(oldPrincipal);
                        newData = new JavaScriptSerializer().Serialize(principal);
                    }
                    else
                    {
                        principal = entities.Principals.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldPrincipal = entities.Principals.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new Principal()
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

                        newData = new JavaScriptSerializer().Serialize(new Principal()
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

                        entities.Entry(principal).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

                    CommonService.SaveDataAudit(new DataAudit()
                    {
                        Entity = "Principal",
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

            return RedirectToAction("Index", "Principal");
        }
    }
}