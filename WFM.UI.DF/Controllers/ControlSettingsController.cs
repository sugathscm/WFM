using System;
using System.Web.Mvc;
using WFM.BAL.Services;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class ControlSettingsController : BaseController
    {
        private readonly ConfigurationService configurationService = new ConfigurationService();

        // GET: ControlSettings
        public ActionResult Index()
        {
            ControlSettingsViewModel model = new ControlSettingsViewModel()
            {
                AlertLeadTimeDays = int.Parse(configurationService.GetValue("AlertLeadTimeDays", "3")),
                AmberWarningWindowDays = int.Parse(configurationService.GetValue("AmberWarningWindowDays", "5")),
                BidGoThreshold = int.Parse(configurationService.GetValue("BidGoThreshold", "80")),
                QAPassMark = int.Parse(configurationService.GetValue("QAPassMark", "80")),
                ProjectHealthOnTrackAt = int.Parse(configurationService.GetValue("ProjectHealthOnTrackAt", "80")),
                ProjectHealthAttentionAt = int.Parse(configurationService.GetValue("ProjectHealthAttentionAt", "60"))
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(ControlSettingsViewModel model)
        {
            try
            {
                configurationService.SaveOrUpdate("AlertLeadTimeDays", model.AlertLeadTimeDays.ToString());
                configurationService.SaveOrUpdate("AmberWarningWindowDays", model.AmberWarningWindowDays.ToString());
                configurationService.SaveOrUpdate("BidGoThreshold", model.BidGoThreshold.ToString());
                configurationService.SaveOrUpdate("QAPassMark", model.QAPassMark.ToString());
                configurationService.SaveOrUpdate("ProjectHealthOnTrackAt", model.ProjectHealthOnTrackAt.ToString());
                configurationService.SaveOrUpdate("ProjectHealthAttentionAt", model.ProjectHealthAttentionAt.ToString());

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.Message;
            }

            return RedirectToAction("Index", "ControlSettings");
        }
    }
}
