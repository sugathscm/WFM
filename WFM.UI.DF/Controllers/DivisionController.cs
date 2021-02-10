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
    public class DivisionController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly DivisionService divisionService = new DivisionService();

        public DivisionController()
        {
        }

        public DivisionController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Division
        public ActionResult Index(int? id)
        {
            WFM_Division division = new WFM_Division();
            if (id != null)
            {
                division = divisionService.GetDivisionById(id);
            }
            return View(division);
        }

        public ActionResult GetList()
        {
            List<WFM_Division> list = divisionService.GetDivisionList();

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
        public ActionResult SaveOrUpdate(WFM_Division model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_Division division = null;
                WFM_Division oldDivision = null;
                if (model.Id == 0)
                {
                    division = new WFM_Division
                    {
                        Name = model.Name,
                        IsActive = true
                    };

                    oldDivision = new WFM_Division();
                    oldData = new JavaScriptSerializer().Serialize(oldDivision);
                    newData = new JavaScriptSerializer().Serialize(division);
                }
                else
                {
                    division = divisionService.GetDivisionById(model.Id);
                    oldDivision = divisionService.GetDivisionById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_Division()
                    {
                        Id = oldDivision.Id,
                        Name = oldDivision.Name,
                        IsActive = oldDivision.IsActive
                    });

                    division.Name = model.Name;
                    bool Example = Convert.ToBoolean(Request.Form["IsActive.Value"]);
                    division.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_Division()
                    {
                        Id = division.Id,
                        Name = division.Name,
                        IsActive = division.IsActive
                    });
                }

                divisionService.SaveOrUpdate(division);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "Division",
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


            return RedirectToAction("Index", "Division");
        }
    }
}