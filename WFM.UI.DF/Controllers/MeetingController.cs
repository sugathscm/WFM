using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WFM.BAL.Enums;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF.Models;
using WFM.UI.DF.ModelsView;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class MeetingController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly MeetingService meetingService = new MeetingService();
        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly MeetingTypeService meetingTypeService = new MeetingTypeService();
        private readonly TaskTrackerService taskTrackerService = new TaskTrackerService();
        private readonly FileNoteService fileNoteService = new FileNoteService();
        private readonly MeetingNoteService meetingNoteService = new MeetingNoteService();
        private readonly ProjectService projectService = new ProjectService();
        private readonly TaskTypeService taskTypeService = new TaskTypeService();
        private readonly CommonDataService commonDataService = new CommonDataService();
        private readonly TaskTrackerCategoryService taskTrackerCategoryService = new TaskTrackerCategoryService();

        public MeetingController()
        {
        }

        public MeetingController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        public ActionResult Index()
        {
            ViewBag.MeetingTypeList = meetingTypeService.GetMeetingList();
            ViewBag.ParticipantIdList = employeeService.GetEmployeeList();

            return View(new MeetingViewModel());
        }

        public ActionResult GetList(string fromDate, string toDate, int? typeId, int? participantId, int? Id)
        {
            List<GetMeetingList_Result> list = new List<GetMeetingList_Result>();

            if ((!string.IsNullOrEmpty(fromDate)) && (!string.IsNullOrEmpty(toDate)))
            {
                list = meetingService.GetMeetingList(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), typeId, participantId, Id);
            }
            else
            {
                list = meetingService.GetMeetingList(null, null, typeId, participantId, Id);
            }


            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(MeetingViewModel model)
        {
            string newData = string.Empty, oldData = string.Empty;
            string body = string.Empty;

            var userId = User.Identity.GetUserId();
            var userName = User.Identity.Name;
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            var meetingType = meetingTypeService.GetMeetingById(model.MeetingTypeId);

            var clientDate = CommonService.GetClientDate(Request);

            try
            {
                int id = model.Id;
                WFM_Meeting meeting = null;
                if (model.Id == 0)
                {
                    meeting = new WFM_Meeting
                    {
                        Location = model.Location,
                        Date = model.Date,
                        ExternalParticipants = model.ExternalParticipants,
                        KPI = model.KPI,
                        MeetingTypeId = model.MeetingTypeId,
                        KPIAchieved = model.KPIAchieved,
                        Remarks = model.Remarks,
                        Name = model.Name + " : " + meetingType.Topic + "-(" + model.Date.Value.ToString("MM/dd/yyyy") + ")",
                        ProjectId = model.ProjectId,
                    };
                }
                else
                {
                    meeting = meetingService.GetMeetingById(model.Id);

                    meeting.Location = model.Location;
                    meeting.Date = model.Date;
                    meeting.ExternalParticipants = model.ExternalParticipants;
                    meeting.KPI = model.KPI;
                    meeting.MeetingTypeId = model.MeetingTypeId;
                    meeting.KPIAchieved = model.KPIAchieved;
                    meeting.Remarks = model.Remarks;
                    meeting.Name = model.Name + " : " + meetingType.Topic + "-(" + model.Date.Value.ToString("MM/dd/yyyy") + ")";
                    meeting.ProjectId = model.ProjectId;
                }

                meetingService.SaveOrUpdate(meeting);
                if (model.InternalParticipants != null)
                {
                    foreach (var participant in model.InternalParticipants)
                    {
                        meetingService.SaveOrUpdate(new WFM_MeetingIP()
                        {
                            MeetingId = meeting.Id,
                            EmployeeId = participant
                        });
                    }
                }

                //Generate Related Task
                WFM_TaskTracker taskTracker = null;

                taskTracker = new WFM_TaskTracker
                {
                    ProjectId = model.ProjectId,
                    //ProjectCode = model.ProjectCode,
                    MeetingId = meeting.Id,
                    //MeetingTypeId = model.MeetingTypeId,
                    //AddToAgendaMeetingId = model.AddToAgendaMeetingId,
                    DateOfMeeting = meeting.Date,
                    TaskDescription = meeting.Name,
                    //CurrentProgressNote = model.CurrentProgressNote,
                    PriorityId = 26, //High
                    AuthorityId = 3, // CS
                    //AssigneeId = model.AssigneeId,
                    DateOfAssignment = clientDate,
                    DueDate = meeting.Date,
                    //TakenNextDueDate = model.TakenNextDueDate,
                    //EmailTo = model.EmailTo,
                    //EmailSubject = model.EmailSubject,
                    //EmailAddress = model.EmailAddress,
                    StatusId = 1, //Not Started
                    CreatedBy = userId,
                    CreatedDate = clientDate,
                    TaskTypeId = 6, //General
                    TaskTrackerCategoryId = 9, //PCC 1 & 2
                    ScheduleDate = meeting.Date,
                    ScheduleTime = Convert.ToDateTime(meeting.Date.Value.ToString()).AddHours(10),
                    //NoOfHrs = model.NoOfHrs,
                    //ActuallyStartedDate = model.ActuallyStartedDate, //Meeting Date
                };

                taskTrackerService.SaveOrUpdate(taskTracker);
                var authorityOf = employeeService.GetEmployeeById(taskTracker.AuthorityId);

                string newAssignees = string.Empty;

                if (model.InternalParticipants != null)
                {
                    var employeeList = employeeService.GetEmployeeByIdList(model.InternalParticipants);

                    foreach (var employee in employeeList)
                    {
                        if (employee != null)
                        {
                            newAssignees += employee.Name + ", ";
                        }
                    }
                }


                body += "<table border='1' style='border-collapse:collapse'>";
                body += "<tr>" +
                    "<th style='padding:5px'>Assigned Date</th>" +
                    "<td style='padding:5px'>" + taskTracker.DateOfAssignment.Value.ToString("dd-MMM-yyyy") + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Type</th>" +
                    "<td style='padding:5px'>" + ((taskTracker.TaskTypeId == null) ? "" : taskTypeService.GetTaskTypeById(taskTracker.TaskTypeId).Name) + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Category</th>" +
                    "<td style='padding:5px'>" + ((taskTracker.TaskTrackerCategoryId == null) ? "" : taskTrackerCategoryService.GetTaskTrackerCategoryById(taskTracker.TaskTrackerCategoryId).Name) + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Priority</th>" +
                    "<td style='padding:5px'>" + ((taskTracker.PriorityId == null) ? "" : commonDataService.GetCommonDataById(taskTracker.PriorityId).Name) + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Project Name</th>" +
                    "<td style='padding:5px'>" + ((taskTracker.ProjectId == null) ? "" : projectService.GetProjectById(0, taskTracker.ProjectId).Name) + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Description</th>" +
                    "<td style='padding:5px'>" + taskTracker.TaskDescription + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Progress Note</th>" +
                    "<td style='padding:5px'>" + taskTracker.CurrentProgressNote + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Due Date</th>" +
                    "<td style='padding:5px'>" + taskTracker.DueDate.Value.ToString("dd-MMM-yyyy") + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Under the Authority Of</th>" +
                    "<td style='padding:5px'>" + ((authorityOf == null) ? "" : authorityOf.Name) + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Assignees</th>" +
                    "<td style='padding:5px'>" + newAssignees + "</td>" +
                    "</tr>";
                body += "<tr>" +
                    "<th style='padding:5px'>Created By</th>" +
                    "<td style='padding:5px'>" + userName + "</td>" +
                    "</tr>";
                body += "</table>";

                foreach (var assignee in model.InternalParticipants)
                {
                    taskTrackerService.SaveOrUpdate(new WFM_TaskTrackerAssignee()
                    {
                        TaskTrackerId = taskTracker.Id,
                        EmployeeId = assignee
                    });

                    var employee = employeeService.GetEmployeeById(assignee);

                    string ccList = userName + "," + authorityOf.Email;

                    if (employee != null)
                    {
                        if (taskTracker.StatusId != 2)
                            CommonService.SendEmail(smtpSection,
                                employee.Email,
                                ccList,
                                "sugath.office@gmail.com",
                                "TTS-" + taskTracker.Id.ToString("0000") + " ~ [Not Started] : " + taskTracker.TaskDescription,
                                body);
                    }
                }

                meeting.RelatedTaskId = taskTracker.Id;
                meetingService.SaveOrUpdate(meeting);

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
            }

            return RedirectToAction("Index", "Meeting");
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Id = id;

            var meetingViewModel = new MeetingViewModel();

            ViewBag.MeetingTypeList = meetingTypeService.GetMeetingList();
            ViewBag.EmployeeList = employeeService.GetEmployeeList();
            ViewBag.ProjectList = projectService.GetProjects(0);

            if (id != null)
            {
                var meeting = meetingService.GetMeetingById(id);

                meetingViewModel = new MeetingViewModel()
                {
                    Location = meeting.Location,
                    Date = meeting.Date,
                    ExternalParticipants = meeting.ExternalParticipants,
                    KPI = meeting.KPI,
                    MeetingTypeId = meeting.MeetingTypeId,
                    KPIAchieved = meeting.KPIAchieved.Value,
                    Remarks = meeting.Remarks,
                };

                List<TaskTrackerDocument> docs = new List<TaskTrackerDocument>();

                string path = Path.Combine(Server.MapPath("~/Docs/Meetings/" + id));

                if (Directory.Exists(path))
                {
                    var fileInfoList = Directory.GetFiles(path);
                    foreach (var file in fileInfoList)
                    {
                        string filename = Path.GetFileNameWithoutExtension(file);
                        string extension = Path.GetExtension(file);
                        if (filename.Length > 20)
                            filename = filename.Substring(0, 20);

                        docs.Add(new TaskTrackerDocument() { DocumentPath = "../../Docs/Meetings/" + id + "/" + filename, DocumentName = filename + extension });
                    }
                }

                meetingViewModel.Documents = docs;

            }

            return View(meetingViewModel);
        }

        public ActionResult View(int? id)
        {
            ViewBag.Id = id;

            var meetingViewModel = new MeetingViewModel();

            ViewBag.MeetingTypeList = meetingTypeService.GetMeetingList();
            ViewBag.EmployeeList = employeeService.GetEmployeeList();

            if (id != null)
            {
                var meeting = meetingService.GetMeetingList(null, null, null, null, id).SingleOrDefault();

                meetingViewModel = new MeetingViewModel()
                {
                    Location = meeting.Location,
                    Date = meeting.Date,
                    ExternalParticipants = meeting.ExternalParticipants,
                    KPI = meeting.KPI,
                    MeetingTypeId = meeting.MeetingTypeId,
                    KPIAchieved = meeting.KPIAchieved.Value,
                    Remarks = meeting.Remarks,
                    Name = meeting.Name,
                    MeetingTypeName = meeting.TypeName,
                    InternalParticipantNames = meeting.Participants
                };

                List<TaskTrackerDocument> docs = new List<TaskTrackerDocument>();

                string path = Path.Combine(Server.MapPath("~/Docs/Meetings/" + id));

                if (Directory.Exists(path))
                {
                    var fileInfoList = Directory.GetFiles(path);
                    foreach (var file in fileInfoList)
                    {
                        string filename = Path.GetFileNameWithoutExtension(file);
                        string extension = Path.GetExtension(file);
                        if (filename.Length > 20)
                            filename = filename.Substring(0, 20);

                        docs.Add(new TaskTrackerDocument() { DocumentPath = "../../Docs/Meetings/" + id + "/" + filename, DocumentName = filename + extension });
                    }
                }

                meetingViewModel.Documents = docs;
            }

            return View(meetingViewModel);
        }

        public ActionResult Minutes(int? id)
        {
            ViewBag.Id = id;

            var meetingViewModel = new MeetingViewModel();

            ViewBag.MeetingTypeList = meetingTypeService.GetMeetingList();
            ViewBag.EmployeeList = employeeService.GetEmployeeList();
            ViewBag.ProjectList = projectService.GetProjects(0);

            if (id != null)
            {
                var meeting = meetingService.GetMeetingList(null,null,null,null,id).SingleOrDefault();

                meetingViewModel = new MeetingViewModel()
                {
                    Location = meeting.Location,
                    Date = meeting.Date,
                    ExternalParticipants = meeting.ExternalParticipants,
                    KPI = meeting.KPI,
                    MeetingTypeId = meeting.MeetingTypeId,
                    KPIAchieved = meeting.KPIAchieved.Value,
                    Remarks = meeting.Remarks,
                    Name = meeting.Name,
                    MeetingTypeName = meeting.TypeName,
                    InternalParticipantNames = meeting.Participants
                };

                List<TaskTrackerDocument> docs = new List<TaskTrackerDocument>();

                string path = Path.Combine(Server.MapPath("~/Docs/Meetings/" + id));

                if (Directory.Exists(path))
                {
                    var fileInfoList = Directory.GetFiles(path);
                    foreach (var file in fileInfoList)
                    {
                        string filename = Path.GetFileNameWithoutExtension(file);
                        string extension = Path.GetExtension(file);
                        if (filename.Length > 20)
                            filename = filename.Substring(0, 20);

                        docs.Add(new TaskTrackerDocument() { DocumentPath = "../../Docs/Meetings/" + id + "/" + filename, DocumentName = filename + extension });
                    }
                }

                meetingViewModel.Documents = docs;
            }

            return View(meetingViewModel);
        }

        public PartialViewResult DisplayFileNotes(int? id)
        {
            List<WFM_FileNote> list = meetingService.GetFileNotesByMeetingId(id);

            List<FileNoteViewModel> fileNoteList = new List<FileNoteViewModel>();

            foreach (WFM_FileNote fileNote in list)
            {
                fileNoteList.Add(new FileNoteViewModel
                {
                    Id = fileNote.Id,
                    Date = fileNote.Date,
                    MeetingId = fileNote.MeetingId,
                    ProjectId = fileNote.ProjectId,
                    NoteGivenBy = fileNote.NoteGivenBy,
                    NoteTakenBy = fileNote.NoteTakenBy,
                    Note = fileNote.Note,
                    DateString = fileNote.Date.Value.ToString("dd/mm/yyyy"),
                    //ProjectName = projectCode.Substring(projectCode.Length - 4),
                    MeetingName = (fileNote.MeetingId != null) ? meetingService.GetMeetingById(fileNote.MeetingId).Name : "",
                    NoteTakenByName = employeeService.GetEmployeeById(fileNote.NoteTakenBy).Name,
                    NoteGivenByName = employeeService.GetEmployeeById(fileNote.NoteGivenBy).Name,
                });
            }
            return PartialView("_ViewFileNotes", fileNoteList);
        }

        public PartialViewResult DisplayMeetingNotes(int? id)
        {
            List<WFM_MeetingNote> list = meetingService.GetMeetingNotesByMeetingId(id);

            List<MeetingNoteViewModel> meetingNoteList = new List<MeetingNoteViewModel>();

            foreach (WFM_MeetingNote fileNote in list)
            {
                meetingNoteList.Add(new MeetingNoteViewModel
                {
                    Id = fileNote.Id,
                    //Date = fileNote.Date,
                    MeetingId = fileNote.MeetingId,
                    NoteGivenBy = fileNote.NoteGivenBy,
                    NoteTakenBy = fileNote.NoteTakenBy,
                    Note = fileNote.Note,
                    //DateString = fileNote.Date.Value.ToString("dd/mm/yyyy"),
                    //MeetingName = (fileNote.MeetingId != null) ? meetingService.GetMeetingById(fileNote.MeetingId).Name : "",
                    NoteTakenByName = employeeService.GetEmployeeById(fileNote.NoteTakenBy).Name,
                    NoteGivenByName = fileNote.NoteGivenBy,
                });
            }
            return PartialView("_ViewMeetingNotes", meetingNoteList);
        }

        public PartialViewResult DisplayTasks(int? id)
        {
            List<GetTaskList_Result> list = taskTrackerService.GetTaskList(null, null, id, null, null, null);

            return PartialView("_ViewTasks", list);
        }

        [HttpPost]
        public ActionResult SaveFileNote(int? meetingId, string note, int? noteGivenBy, int? noteTakenBy, string date, int? projectId)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                //int id = model.Id;
                WFM_FileNote fileNote = new WFM_FileNote();

                fileNote = new WFM_FileNote
                {
                    MeetingId = meetingId,
                    Note = note,
                    NoteGivenBy = noteGivenBy,
                    NoteTakenBy = noteTakenBy,
                    Date = Convert.ToDateTime(date),
                    ProjectId = projectId
                };

                fileNoteService.SaveOrUpdate(fileNote);


                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.InnerException;
            }

            return RedirectToAction("Index", "Meeting");
        }

        [HttpPost]
        public ActionResult SaveMeetingNote(string meetingId, string note, string noteGivenBy, string noteTakenBy)
        {
            string newData = string.Empty, oldData = string.Empty;

            try
            {
                //int id = model.Id;
                WFM_MeetingNote meetingNote = new WFM_MeetingNote();

                meetingNote = new WFM_MeetingNote
                {
                    MeetingId = int.Parse(meetingId),
                    Note = note,
                    NoteGivenBy = noteGivenBy,
                    NoteTakenBy = int.Parse(noteTakenBy),
                };

                meetingNoteService.SaveOrUpdate(meetingNote);

                var meeting = meetingService.GetMeetingById(int.Parse(meetingId));

                if (meeting != null)
                {
                    var task = taskTrackerService.GetTaskById(meeting.RelatedTaskId);

                    if (task != null)
                    {
                        task.CurrentProgressNote = task.CurrentProgressNote + "\n" + note;
                        task.StatusId = 3;
                        taskTrackerService.SaveOrUpdate(task);
                    }
                }

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