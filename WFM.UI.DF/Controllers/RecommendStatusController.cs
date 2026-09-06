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
    [Authorize]
    public class RecommendStatusController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly RecommendStatusService recommendStatusService = new RecommendStatusService();

        public RecommendStatusController()
        {
        }

        public RecommendStatusController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: RecommendStatus
        public ActionResult Index(int? id)
        {
            WFM_RecommendStatus recommendStatus = new WFM_RecommendStatus();
            if (id != null)
            {
                recommendStatus = recommendStatusService.GetRecommendStatusById(id);
            }
            return View(recommendStatus);
        }

        public ActionResult GetList()
        {
            List<WFM_RecommendStatus> list = recommendStatusService.GetRecommendStatusList();

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
        public ActionResult SaveOrUpdate(WFM_RecommendStatus model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                WFM_RecommendStatus recommendStatus = null;
                WFM_RecommendStatus oldRecommendStatus = null;
                if (model.Id == 0)
                {
                    recommendStatus = new WFM_RecommendStatus
                    {
                        Name = model.Name,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    oldRecommendStatus = new WFM_RecommendStatus();
                    oldData = new JavaScriptSerializer().Serialize(oldRecommendStatus);
                    newData = new JavaScriptSerializer().Serialize(recommendStatus);
                }
                else
                {
                    recommendStatus = recommendStatusService.GetRecommendStatusById(model.Id);
                    oldRecommendStatus = recommendStatusService.GetRecommendStatusById(model.Id);

                    oldData = new JavaScriptSerializer().Serialize(new WFM_RecommendStatus()
                    {
                        Id = oldRecommendStatus.Id,
                        Name = oldRecommendStatus.Name,
                        IsActive = oldRecommendStatus.IsActive
                    });

                    recommendStatus.Name = model.Name;
                    recommendStatus.IsActive = model.IsActive;

                    newData = new JavaScriptSerializer().Serialize(new WFM_RecommendStatus()
                    {
                        Id = recommendStatus.Id,
                        Name = recommendStatus.Name,
                        IsActive = recommendStatus.IsActive
                    });
                }

                recommendStatusService.SaveOrUpdate(recommendStatus);

                CommonService.SaveDataAudit(new DataAudit()
                {
                    Entity = "RecommendStatus",
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

            return RedirectToAction("Index", "RecommendStatus");
        }
    }
}
