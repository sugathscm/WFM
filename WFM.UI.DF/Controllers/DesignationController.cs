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
    public class DesignationController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly DesignationService designationService = new DesignationService();

        public DesignationController()
        {
        }

        public DesignationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Designation
        public ActionResult Index(int? id)
        {
            WFM_Designation designation = new WFM_Designation();
            if (id != null)
            {
                designation = designationService.GetDesignationById(id);
            }
            return View(designation);
        }

        public ActionResult GetList()
        {
            List<WFM_Designation> list = designationService.GetDesignationList();

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
        public ActionResult SaveOrUpdate(WFM_Designation model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_Designation designation = null;
                WFM_Designation oldDesignation = null;
                if (model.Id == 0)
                {
                    designation = new WFM_Designation
                    {
                        Name = model.Name,
                        IsActive = true
                    };                   

                    oldDesignation = new WFM_Designation();
                    oldData = new JavaScriptSerializer().Serialize(oldDesignation);
                    newData = new JavaScriptSerializer().Serialize(designation);
                }
                else
                {
                    designation = designationService.GetDesignationById(model.Id);
                    oldDesignation = designationService.GetDesignationById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_Designation()
                    {
                        Id = oldDesignation.Id,
                        Name = oldDesignation.Name,
                        IsActive = oldDesignation.IsActive
                    });

                    designation.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    designation.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_Designation()
                    {
                        Id = designation.Id,
                        Name = designation.Name,
                        IsActive = designation.IsActive
                    });
                }

                designationService.SaveOrUpdate(designation);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "Designation",
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


            return RedirectToAction("Index", "Designation");
        }
    }
}