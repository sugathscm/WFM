using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.DAL;
using WFM.UI.DF.Models;

namespace WFM.UI.DF.Controllers
{
    public class TenderDocumentTypeController : Controller
    {
        // GET: WFM_TenderDocumentType
        public ActionResult Index(int? id)
        {
            WFM_TenderDocumentType tenderDocumentType = new WFM_TenderDocumentType();
            if (id != null)
            {
                using (LinkManagementEntities entities = new LinkManagementEntities())
                {
                    tenderDocumentType = entities.WFM_TenderDocumentType.Where(o => o.Id == id).SingleOrDefault();
                }
            }
            return View(tenderDocumentType);
        }

        public ActionResult GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                var list = entities.WFM_TenderDocumentType.OrderBy(o => o.Name).ToList();

                List<WFM_TenderDocumentType> modelList = new List<WFM_TenderDocumentType>();

                foreach (var item in list)
                {
                    modelList.Add(new WFM_TenderDocumentType() { Id = item.Id, IsActive = item.IsActive, Name = item.Name });
                }

                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_TenderDocumentType model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                try
                {
                    int id = model.Id;
                    WFM_TenderDocumentType tenderDocumentType = null;
                    WFM_TenderDocumentType oldTenderDocumentType = null;
                    if (model.Id == 0)
                    {
                        tenderDocumentType = new WFM_TenderDocumentType
                        {
                            Name = model.Name,
                            IsActive = true
                        };

                        entities.WFM_TenderDocumentType.Add(tenderDocumentType);
                        entities.SaveChanges();

                        oldTenderDocumentType = new WFM_TenderDocumentType();
                        oldData = new JavaScriptSerializer().Serialize(oldTenderDocumentType);
                        newData = new JavaScriptSerializer().Serialize(tenderDocumentType);
                    }
                    else
                    {
                        tenderDocumentType = entities.WFM_TenderDocumentType.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldTenderDocumentType = entities.WFM_TenderDocumentType.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new WFM_TenderDocumentType()
                        {
                            Id = oldTenderDocumentType.Id,
                            Name = oldTenderDocumentType.Name,
                            IsActive = oldTenderDocumentType.IsActive
                        });

                        tenderDocumentType.Name = model.Name;
                        bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                        tenderDocumentType.IsActive = model.IsActive;

                        newData = new JavaScriptSerializer().Serialize(new WFM_TenderDocumentType()
                        {
                            Id = tenderDocumentType.Id,
                            Name = tenderDocumentType.Name,
                            IsActive = tenderDocumentType.IsActive
                        });

                        entities.Entry(tenderDocumentType).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }

                    //CommonService.SaveDataAudit(new DataAudit()
                    //{
                    //    Entity = "WFM_TenderDocumentType",
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

            return RedirectToAction("Index", "WFM_TenderDocumentType");
        }
    }
}