using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class MeetingNoteController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly MeetingNoteService meetingNoteService = new MeetingNoteService();
        private readonly ProjectService projectService = new ProjectService();
        private readonly MeetingTypeService meetingService = new MeetingTypeService();
        private readonly EmployeeService employeeService = new EmployeeService();

        public MeetingNoteController()
        {
        }

        public MeetingNoteController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: MeetingNote
        public ActionResult Index(int? id)
        {
            WFM_MeetingNote meetingNote = new WFM_MeetingNote();
            ViewBag.ProjectList = projectService.GetProjects(0);
            ViewBag.MeetingList = meetingService.GetMeetingList();
            ViewBag.EmployeeList = employeeService.GetEmployeeList();

            if (id != null)
            {
                meetingNote = meetingNoteService.GetMeetingNoteById(id);
            }

            return View(meetingNote);
        }

        public ActionResult GetList()
        {
            List<WFM_MeetingNote> list = meetingNoteService.GetMeetingNoteList();

            List<MeetingNoteViewModel> meetingNoteList = new List<MeetingNoteViewModel>();

            foreach (WFM_MeetingNote meetingNote in list)
            {
                meetingNoteList.Add(new MeetingNoteViewModel
                {
                    Id = meetingNote.Id,
                    Date = meetingNote.Date,
                    MeetingId = meetingNote.MeetingId,
                    NoteGivenBy = meetingNote.NoteGivenBy,
                    NoteTakenBy = meetingNote.NoteTakenBy,
                    Note = meetingNote.Note,
                    //DateString = meetingNote.Date.Value.ToString("dd-MMM-yyyy"),
                    MeetingName = (meetingNote.MeetingId != null) ? meetingService.GetMeetingById(meetingNote.MeetingId).Topic : "",
                    NoteTakenByName = employeeService.GetEmployeeById(meetingNote.NoteTakenBy).Name,
                    NoteGivenByName = meetingNote.NoteGivenBy,
                });
            }

            return Json(new { data = meetingNoteList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View(int? id)
        {
            List<WFM_MeetingNote> list = meetingNoteService.GetMeetingNoteList();//.Where(p => p.ProjectId == id).ToList();

            List<MeetingNoteViewModel> meetingNoteList = new List<MeetingNoteViewModel>();

            foreach (WFM_MeetingNote meetingNote in list)
            {

                meetingNoteList.Add(new MeetingNoteViewModel
                {
                    Id = meetingNote.Id,
                    Date = meetingNote.Date,
                    MeetingId = meetingNote.MeetingId,
                    NoteGivenBy = meetingNote.NoteGivenBy,
                    NoteTakenBy = meetingNote.NoteTakenBy,
                    Note = meetingNote.Note,
                    DateString = meetingNote.Date.Value.ToString("dd-MMM-yyyy"),
                    MeetingName = (meetingNote.MeetingId != null) ? meetingService.GetMeetingById(meetingNote.MeetingId).Topic : "",
                    NoteTakenByName = employeeService.GetEmployeeById(meetingNote.NoteTakenBy).Name,
                    NoteGivenByName = meetingNote.NoteGivenBy,
                });
            }

            return PartialView(meetingNoteList);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_MeetingNote model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_MeetingNote meetingNote = null;
                if (model.Id == 0)
                {
                    meetingNote = new WFM_MeetingNote
                    {
                        Date = model.Date,
                        MeetingId = model.MeetingId,
                        NoteGivenBy = model.NoteGivenBy,
                        NoteTakenBy = model.NoteTakenBy,
                        Note = model.Note,
                    };
                }
                else
                {
                    meetingNote = meetingNoteService.GetMeetingNoteById(model.Id);

                    meetingNote.Date = model.Date;
                    meetingNote.MeetingId = model.MeetingId;
                    meetingNote.NoteGivenBy = model.NoteGivenBy;
                    meetingNote.NoteTakenBy = model.NoteTakenBy;
                    meetingNote.Note = model.Note;
                }

                meetingNoteService.SaveOrUpdate(meetingNote);

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
            }

            return RedirectToAction("Index", "MeetingNote");
        }
    }
}