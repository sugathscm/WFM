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
    public class SourcingPartnerController : Controller
    {
        private ApplicationUserManager _userManager;

        public SourcingPartnerController()
        {

        }
        public SourcingPartnerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: SourcingPartners
        public ActionResult Index(int? id)
        {
            SourcingPartner sourcingPartner = new SourcingPartner();

            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                if (id != null)
                {
                    sourcingPartner = entities.SourcingPartners.Where(o => o.Id == id).SingleOrDefault();
                }
                //var listData = entities.Designations.Select(s => new { Id = s.Id, Value = s.Name }).ToList();
                //ViewBag.ListObject = new SelectList(listData, "Id", "Value");
            }
            return View(sourcingPartner);
        }

        public ActionResult GetList()
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                var list = entities.SourcingPartners.OrderBy(o => o.Name).ToList();
                List<SourcingPartnerView> modelList = new List<SourcingPartnerView>();
                foreach (var item in list)
                {
                    modelList.Add(new SourcingPartnerView()
                    {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        Name = item.Name,
                        Mobile = item.Mobile,
                        Email = item.Email,
                        FixedLine = item.FixedLine,
                        SourcingPartnerType = item.SourcingPartnerType,
                    });
                }
                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(SourcingPartner model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                try
                {
                    int id = model.Id;
                    SourcingPartner sourcingPartner = null;
                    SourcingPartner oldSourcingPartner = null;
                    if (model.Id == 0)
                    {
                        sourcingPartner = new SourcingPartner
                        {
                            Name = model.Name,
                            Mobile = model.Mobile,
                            Email = model.Email,
                            FixedLine = model.FixedLine,
                            IsActive = true,
                            SourcingPartnerType = model.SourcingPartnerType
                        };

                        entities.SourcingPartners.Add(sourcingPartner);
                        entities.SaveChanges();

                        oldSourcingPartner = new SourcingPartner();
                        oldData = new JavaScriptSerializer().Serialize(oldSourcingPartner);
                        newData = new JavaScriptSerializer().Serialize(sourcingPartner);
                    }
                    else
                    {
                        sourcingPartner = entities.SourcingPartners.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldSourcingPartner = entities.SourcingPartners.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new SourcingPartner()
                        {
                            Id = oldSourcingPartner.Id,
                            Name = oldSourcingPartner.Name,
                            Mobile = oldSourcingPartner.Mobile,
                            Email = oldSourcingPartner.Email,
                            FixedLine = oldSourcingPartner.FixedLine,
                            SourcingPartnerType = oldSourcingPartner.SourcingPartnerType,
                            IsActive = oldSourcingPartner.IsActive
                        });
                        
                        sourcingPartner.Name = model.Name;
                        sourcingPartner.Mobile = model.Mobile;
                        sourcingPartner.Email = model.Email;
                        sourcingPartner.FixedLine = model.FixedLine;
                        sourcingPartner.SourcingPartnerType = model.SourcingPartnerType;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        sourcingPartner.IsActive = model.IsActive;

                        newData = new JavaScriptSerializer().Serialize(new SourcingPartner()
                        {
                            Id = sourcingPartner.Id,
                            Name = sourcingPartner.Name,
                            Mobile = sourcingPartner.Mobile,
                            Email = sourcingPartner.Email,
                            FixedLine = sourcingPartner.FixedLine,
                            SourcingPartnerType = sourcingPartner.SourcingPartnerType,
                            IsActive = sourcingPartner.IsActive
                        });

                        entities.Entry(sourcingPartner).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

                    CommonService.SaveDataAudit(new DataAudit()
                    {
                        Entity = "SourcingPartner",
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

            return RedirectToAction("Index", "SourcingPartner");
        }
    }
}