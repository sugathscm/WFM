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
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class MeetingTypeController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly MeetingTypeService meetingTypeService = new MeetingTypeService();

        public MeetingTypeController()
        {
        }

        public MeetingTypeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Meeting
        public ActionResult Index(int? id)
        {
            WFM_MeetingType meeting = new WFM_MeetingType();
            if (id != null)
            {
                meeting = meetingTypeService.GetMeetingById(id);
            }
            return View(meeting);
        }

        public ActionResult GetList()
        {
            List<WFM_MeetingType> list = meetingTypeService.GetMeetingList();

            List<MeetingViewModel> meetingList = new List<MeetingViewModel>();

            foreach(WFM_MeetingType meeting in list)
            {
                meetingList.Add(new MeetingViewModel
                {
                    Id = meeting.Id,
                    Location = meeting.Location,
                    Topic = meeting.Topic,
                });
            }

            return Json(new { data = meetingList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_MeetingType model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_MeetingType meeting = null;
                if (model.Id == 0)
                {
                    meeting = new WFM_MeetingType
                    {
                        Topic = model.Topic,
                        Location = model.Location,
                        //Date = model.Date,
                    };
                }
                else
                {
                    meeting = meetingTypeService.GetMeetingById(model.Id);

                    meeting.Topic = model.Topic;
                    meeting.Location = model.Location;
                    //meeting.Date = model.Date;
                }

                meetingTypeService.SaveOrUpdate(meeting);

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
            }


            return RedirectToAction("Index", "Meeting");
        }
    }
}