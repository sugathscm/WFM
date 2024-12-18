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
using WFM.UI.DF.Models;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class FileNoteController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly FileNoteService fileNoteService = new FileNoteService();
        private readonly ProjectService projectService = new ProjectService();
        private readonly MeetingService meetingService = new MeetingService();
        private readonly EmployeeService employeeService = new EmployeeService();

        public FileNoteController()
        {
        }

        public FileNoteController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: FileNote
        public ActionResult Index(int? id)
        {
            WFM_FileNote fileNote = new WFM_FileNote();
            ViewBag.ProjectList = projectService.GetProjects(0);
            ViewBag.MeetingList = meetingService.GetMeetingList(null, null, null, null, null);
            ViewBag.EmployeeList = employeeService.GetEmployeeList();

            if (id != null)
            {
                fileNote = fileNoteService.GetFileNoteById(id);
            }

            return View(fileNote);
        }

        public ActionResult GetList()
        {
            List<WFM_FileNote> list = fileNoteService.GetFileNoteList();

            List<FileNoteViewModel> fileNoteList = new List<FileNoteViewModel>();

            foreach (WFM_FileNote fileNote in list)
            {
                string projectCode = "", meetingName = "";

                try
                {
                    projectCode = (fileNote.ProjectId != null) ? projectService.GetProjectById(0, fileNote.ProjectId.Value).Code : "";
                    projectCode = projectCode.Substring(projectCode.Length - 4);
                }
                catch (Exception)
                {
                    projectCode = "";
                }

                try
                {
                    meetingName = (fileNote.MeetingId != null) ? meetingService.GetMeetingById(fileNote.MeetingId).Name : "";
                }
                catch (Exception)
                {
                    meetingName = "";
                }

                fileNoteList.Add(new FileNoteViewModel
                {
                    Id = fileNote.Id,
                    Date = fileNote.Date,
                    MeetingId = fileNote.MeetingId,
                    ProjectId = fileNote.ProjectId,
                    NoteGivenBy = fileNote.NoteGivenBy,
                    NoteTakenBy = fileNote.NoteTakenBy,
                    Note = fileNote.Note,
                    DateString = fileNote.Date.Value.ToString("dd-MMM-yyyy"),
                    ProjectName = projectCode,
                    MeetingName = meetingName,
                    NoteTakenByName = employeeService.GetEmployeeById(fileNote.NoteTakenBy).Name,
                    NoteGivenByName = employeeService.GetEmployeeById(fileNote.NoteGivenBy).Name,
                });
            }

            return Json(new { data = fileNoteList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult View(int? id)
        {
            List<WFM_FileNote> list = fileNoteService.GetFileNoteList().Where(p => p.ProjectId == id).ToList();

            List<FileNoteViewModel> fileNoteList = new List<FileNoteViewModel>();

            foreach (WFM_FileNote fileNote in list)
            {
                var projectCode = projectService.GetProjectById(0, fileNote.ProjectId.Value).Code;

                fileNoteList.Add(new FileNoteViewModel
                {
                    Id = fileNote.Id,
                    Date = fileNote.Date,
                    MeetingId = fileNote.MeetingId,
                    ProjectId = fileNote.ProjectId,
                    NoteGivenBy = fileNote.NoteGivenBy,
                    NoteTakenBy = fileNote.NoteTakenBy,
                    Note = fileNote.Note,
                    DateString = fileNote.Date.Value.ToString("dd-MMM-yyyy"),
                    ProjectName = projectCode.Substring(projectCode.Length - 4),
                    MeetingName = (fileNote.MeetingId != null) ? meetingService.GetMeetingById(fileNote.MeetingId).Name : "",
                    NoteTakenByName = employeeService.GetEmployeeById(fileNote.NoteTakenBy).Name,
                    NoteGivenByName = employeeService.GetEmployeeById(fileNote.NoteGivenBy).Name,
                });
            }

            return PartialView(fileNoteList);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(WFM_FileNote model)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                int id = model.Id;
                WFM_FileNote fileNote = null;
                if (model.Id == 0)
                {
                    fileNote = new WFM_FileNote
                    {
                        Date = model.Date,
                        MeetingId = model.MeetingId,
                        ProjectId = model.ProjectId,
                        NoteGivenBy = model.NoteGivenBy,
                        NoteTakenBy = model.NoteTakenBy,
                        Note = model.Note,
                    };
                }
                else
                {
                    fileNote = fileNoteService.GetFileNoteById(model.Id);

                    fileNote.Date = model.Date;
                    fileNote.MeetingId = model.MeetingId;
                    fileNote.ProjectId = model.ProjectId;
                    fileNote.NoteGivenBy = model.NoteGivenBy;
                    fileNote.NoteTakenBy = model.NoteTakenBy;
                    fileNote.Note = model.Note;
                }

                fileNoteService.SaveOrUpdate(fileNote);

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
            }

            return RedirectToAction("Index", "FileNote");
        }
    }
}