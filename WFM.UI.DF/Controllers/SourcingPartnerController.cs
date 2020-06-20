using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL;
using WFM.BAL.Enums;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    public class SourcingPartnerController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly SourcingPartnerService sourcingPartnerService = new SourcingPartnerService();


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
            WFM_SourcingPartner sourcingPartner = new WFM_SourcingPartner();

            if (id != null)
            {
                sourcingPartner = sourcingPartnerService.GetSourdingPartnerById(id);
            }

            return View(sourcingPartner);
        }

        public ActionResult GetList()
        {
            var list = sourcingPartnerService.GetSourdingPartnerList();
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
                    SourcingPartnerType = item.SourcingPartnerTypeId,
                });
            }
            return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_SourcingPartner model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_SourcingPartner sourcingPartner = null;
                WFM_SourcingPartner oldSourcingPartner = null;
                if (model.Id == 0)
                {
                    sourcingPartner = new WFM_SourcingPartner
                    {
                        Name = model.Name,
                        Mobile = model.Mobile,
                        Email = model.Email,
                        FixedLine = model.FixedLine,
                        IsActive = true,
                        SourcingPartnerTypeId = model.SourcingPartnerTypeId
                    };

                    oldSourcingPartner = new WFM_SourcingPartner();
                    oldData = new JavaScriptSerializer().Serialize(oldSourcingPartner);
                    newData = new JavaScriptSerializer().Serialize(sourcingPartner);
                }
                else
                {
                    sourcingPartner = sourcingPartnerService.GetSourdingPartnerById(model.Id);
                    oldSourcingPartner = sourcingPartnerService.GetSourdingPartnerById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_SourcingPartner()
                    {
                        Id = oldSourcingPartner.Id,
                        Name = oldSourcingPartner.Name,
                        Mobile = oldSourcingPartner.Mobile,
                        Email = oldSourcingPartner.Email,
                        FixedLine = oldSourcingPartner.FixedLine,
                        SourcingPartnerTypeId = oldSourcingPartner.SourcingPartnerTypeId,
                        IsActive = oldSourcingPartner.IsActive
                    });

                    sourcingPartner.Name = model.Name;
                    sourcingPartner.Mobile = model.Mobile;
                    sourcingPartner.Email = model.Email;
                    sourcingPartner.FixedLine = model.FixedLine;
                    sourcingPartner.SourcingPartnerTypeId = model.SourcingPartnerTypeId;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    sourcingPartner.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_SourcingPartner()
                    {
                        Id = sourcingPartner.Id,
                        Name = sourcingPartner.Name,
                        Mobile = sourcingPartner.Mobile,
                        Email = sourcingPartner.Email,
                        FixedLine = sourcingPartner.FixedLine,
                        SourcingPartnerTypeId = sourcingPartner.SourcingPartnerTypeId,
                        IsActive = sourcingPartner.IsActive
                    });
                }

                sourcingPartnerService.SaveOrUpdate(sourcingPartner);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "WFM_SourcingPartner",
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


            return RedirectToAction("Index", "SourcingPartner");
        }
    }
}