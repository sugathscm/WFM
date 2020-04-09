using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.DAL;
using WFM.UI.DF.Models;

namespace WFM.UI.Controllers
{
    public class TenderDocumentSectionController : Controller
    {
        // GET: TenderDocumentSection
        public ActionResult Index(int? id)
        {
            TenderDocumentSection tenderDocumentSection = new TenderDocumentSection();
            if (id != null)
            {
                using (WorkFlowEntities entities = new WorkFlowEntities())
                {
                    tenderDocumentSection = entities.TenderDocumentSections.Where(o => o.Id == id).SingleOrDefault();
                }
            }
            return View(tenderDocumentSection);
        }

        public ActionResult GetList()
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                var list = entities.TenderDocumentSections.OrderBy(o => o.Name).ToList();

                List<TenderDocumentSection> modelList = new List<TenderDocumentSection>();

                foreach (var item in list)
                {
                    modelList.Add(new TenderDocumentSection() { Id = item.Id, IsActive = item.IsActive, Name = item.Name,ParentId = item.ParentId});
                }

                return Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(TenderDocumentSection model)
        {
            string newData = string.Empty, oldData = string.Empty;
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                try
                {
                    int id = model.Id;
                    TenderDocumentSection tenderDocumentSection = null;
                    TenderDocumentSection oldTenderDocumentSection = null;
                    if (model.Id == 0)
                    {
                        tenderDocumentSection = new TenderDocumentSection
                        {
                            Name = model.Name,
                            ParentId = model.ParentId,
                            IsActive = true
                        };

                        entities.TenderDocumentSections.Add(tenderDocumentSection);
                        entities.SaveChanges();

                        oldTenderDocumentSection = new TenderDocumentSection();
                        oldData = new JavaScriptSerializer().Serialize(oldTenderDocumentSection);
                        newData = new JavaScriptSerializer().Serialize(tenderDocumentSection);
                    }
                    else
                    {
                        tenderDocumentSection = entities.TenderDocumentSections.Where(o => o.Id == model.Id).SingleOrDefault();
                        oldTenderDocumentSection = entities.TenderDocumentSections.Where(o => o.Id == model.Id).SingleOrDefault();

                        oldData = new JavaScriptSerializer().Serialize(new TenderDocumentSection()
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

                        newData = new JavaScriptSerializer().Serialize(new TenderDocumentSection()
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
                    //    Entity = "TenderDocumentSection",
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

            return RedirectToAction("Index", "TenderDocumentSection");
        }
    }
}