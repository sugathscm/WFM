using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.DAL;
using WFM.UI.DF;
using WFM.UI.DF.Controllers;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.Controllers
{
    public class TenderDocumentSectionController : BaseController
    {
        private ApplicationUserManager _userManager;

        public TenderDocumentSectionController()
        {
        }
        public TenderDocumentSectionController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: WFM_TenderDocumentSection
        public ActionResult Index(int? id)
        {
            WFM_TenderDocumentSection tenderDocumentSection = new WFM_TenderDocumentSection();
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (id != null)
                {
                    tenderDocumentSection = entities.WFM_TenderDocumentSection.Where(o => o.Id == id).SingleOrDefault();
                }

                ViewBag.TenderDocumentSectionList = entities.WFM_TenderDocumentSection.Where(o => o.ParentId == 0).OrderBy(o => o.Name).ToList();
            }

            return View(tenderDocumentSection);
        }

        public ActionResult GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                var list = entities.WFM_ProjectSector.OrderBy(o => o.Name).ToList();
                List<TenderDocumentSectionView> modelList = new List<TenderDocumentSectionView>();
                foreach (var item in list)
                {
                    modelList.Add(new TenderDocumentSectionView()
                    {
                        Id = item.Id,
                        IsActive = item.IsActive,
                        Name = item.Name,
                        ParentName = (item.ParentId == 0) ? "" : entities.WFM_ProjectSector.Where(o => o.Id == item.ParentId).SingleOrDefault().Name
                    });
                }
                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_TenderDocumentSection model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                try
                {
                    int id = model.Id;
                    WFM_TenderDocumentSection tenderDocumentSection = null;
                    WFM_TenderDocumentSection oldTenderDocumentSection = null;
                    if (model.Id == 0)
                    {
                        tenderDocumentSection = new WFM_TenderDocumentSection
                        {
                            Name = model.Name,
                            ParentId = model.ParentId,
                            IsActive = true
                        };

                        entities.WFM_TenderDocumentSection.Add(tenderDocumentSection);
                        entities.SaveChanges();

                        oldTenderDocumentSection = new WFM_TenderDocumentSection();
                        oldData = new JavaScriptSerializer().Serialize(oldTenderDocumentSection);
                        newData = new JavaScriptSerializer().Serialize(tenderDocumentSection);
                    }
                    else
                    {
                        tenderDocumentSection = entities.WFM_TenderDocumentSection.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldTenderDocumentSection = entities.WFM_TenderDocumentSection.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new WFM_TenderDocumentSection()
                        {
                            Id = oldTenderDocumentSection.Id,
                            Name = oldTenderDocumentSection.Name,
                            ParentId = oldTenderDocumentSection.ParentId,
                            IsActive = oldTenderDocumentSection.IsActive
                        });

                        tenderDocumentSection.Name = model.Name;
                        tenderDocumentSection.ParentId = model.ParentId;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        tenderDocumentSection.IsActive = model.IsActive;

                        newData = new JavaScriptSerializer().Serialize(new WFM_TenderDocumentSection()
                        {
                            Id = tenderDocumentSection.Id,
                            Name = tenderDocumentSection.Name,
                            ParentId = tenderDocumentSection.ParentId,
                            IsActive = tenderDocumentSection.IsActive
                        });

                        entities.Entry(tenderDocumentSection).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

                    //CommonService.SaveDataAudit(new DataAudit()
                    //{
                    //    Entity = "WFM_TenderDocumentSection",
                    //    NewData = newData,
                    //    OldData = oldData,
                    //    UpdatedOn = DateTime.Now,
                    //    UserId = new Guid(User.Identity.GetUserId())
                    //});

                    TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
                }
            }

            return RedirectToAction("Index", "WFM_TenderDocumentSection");
        }
    }
}