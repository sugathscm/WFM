using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using WFM.BAL.Enums;
using WFM.BAL.Helpers;
using WFM.BAL.Services;
using WFM.DAL;
using WFM.UI.DF.Models;
using WFM.UI.DF.ModelsView;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class TaskTrackerController : BaseController
    {
        private ApplicationUserManager _userManager;
        private readonly TaskTrackerService taskTrackerService = new TaskTrackerService();
        private readonly ProjectService projectService = new ProjectService();
        private readonly CommonDataService commonDataService = new CommonDataService();
        private readonly MeetingService meetingService = new MeetingService();
        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly TaskTypeService taskTypeService = new TaskTypeService();
        private readonly TaskTrackerCategoryService taskTrackerCategoryService = new TaskTrackerCategoryService();
        private readonly DivisionService divideService = new DivisionService();

        // GET: TaskTracker
        public TaskTrackerController()
        {

        }

        public TaskTrackerController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public ActionResult Index()
        {
            var clientDate = CommonService.GetClientDate(Request);
            ViewBag.ProjectList = projectService.GetProjects(0);
            ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            ViewBag.MeetingList = meetingService.GetMeetingList(null, null, null, null, null);
            ViewBag.EmployeeList = employeeService.GetEmployeeList();
            ViewBag.TaskTypeList = taskTypeService.GetTaskTypeList();
            ViewBag.TaskTrackerCategoryList = taskTrackerCategoryService.GetTaskTrackerCategoryList();
            ViewBag.TaskTrackerStatusList = taskTrackerService.GetTaskStatusList();
            ViewBag.GeneratedBy = User.Identity.Name;
            return View();
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Id = id;

            var taskViewModel = new TaskTrackerViewModel();
            var employeeList = employeeService.GetEmployeeList().ToList();

            ViewBag.ProjectList = projectService.GetProjects(0);
            ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            ViewBag.MeetingList = meetingService.GetMeetingList(null, null, null, null, null);
            ViewBag.EmployeeList = employeeList;
            ViewBag.TaskTypeList = taskTypeService.GetTaskTypeList();
            ViewBag.TaskTrackerCategoryList = divideService.GetDivisionList();

            List<SelectListItem> StatusList = new List<SelectListItem>();
            StatusList.Add(new SelectListItem() { Text = "Not Started", Value = "1" });
            StatusList.Add(new SelectListItem() { Text = "In-Progress", Value = "2" });
            StatusList.Add(new SelectListItem() { Text = "Completed", Value = "3" });
            StatusList.Add(new SelectListItem() { Text = "Not Relavent", Value = "4" });

            ViewBag.StatusList = new SelectList(StatusList, "Value", "Text");


            List<SelectListItem> FrequencyList = new List<SelectListItem>();
            FrequencyList.Add(new SelectListItem() { Text = "Daily", Value = "1" });
            FrequencyList.Add(new SelectListItem() { Text = "Weekly", Value = "7" });
            FrequencyList.Add(new SelectListItem() { Text = "Every Other Week", Value = "14" });
            FrequencyList.Add(new SelectListItem() { Text = "Every Month", Value = "30" });

            ViewBag.FrequencyList = new SelectList(FrequencyList, "Value", "Text");

            List<SelectListItem> Hrs = new List<SelectListItem>();
            for (int i = 9; i < 20; i++)
            {
                Hrs.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.ScheduleTimeHrs = Hrs;

            List<SelectListItem> Mins = new List<SelectListItem>();
            for (int i = 0; i < 61; i = i + 5)
            {
                Mins.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.ScheduleTimeMins = Mins;

            if (id != null)
            {
                //var task = taskTrackerService.GetTaskById(id);
                var task = taskTrackerService.GetTaskList(null, null, null, null, null, id).FirstOrDefault();
                taskViewModel = new TaskTrackerViewModel()
                {
                    ProjectId = task.ProjectId,
                    ProjectCode = task.ProjectCode,
                    MeetingId = task.MeetingId,
                    DateOfMeeting = task.DateOfMeeting,
                    TaskDescription = task.TaskDescription,
                    TaskDescriptionDB = task.TaskDescriptionDB,
                    CurrentProgressNote = task.CurrentProgressNote,
                    CurrentProgressNoteView = task.CurrentProgressNoteView,
                    AddToAgenda = task.AddToAgenda,
                    PriorityId = task.PriorityId,
                    AuthorityId = task.AuthorityId,
                    AssigneeId = task.AssigneeId,
                    DateOfAssignment = task.DateOfAssignment,
                    DueDate = task.DueDate,
                    TakenNextDueDate = task.TakenNextDueDate,
                    EmailTo = task.EmailTo,
                    EmailSubject = task.EmailSubject,
                    EmailAddress = task.EmailAddress,
                    TaskTypeId = task.TaskTypeId,
                    StatusId = task.StatusId,
                    TaskTrackerCategoryId = task.TaskTrackerCategoryId,
                    AssigneeIdList = (task.AssigneeIds != null)?task.AssigneeIds.Select(x => (int?)x).ToList():null,
                    Assignees = task.Assignees,
                    ScheduleDate = task.scheduleDate,
                    ScheduleTimeHrs = (task.ScheduleTime != null) ? task.ScheduleTime.Value.Hour : 0,
                    ScheduleTimeMins = (task.ScheduleTime != null) ? task.ScheduleTime.Value.Minute : 0,
                    NoOfHrs = task.NoofHrs,
                };

                List<TaskTrackerDocument> docs = new List<TaskTrackerDocument>();

                string path = Path.Combine(Server.MapPath("~/Docs/Tasks/" + id));

                //foreach (var document in documents)
                //{
                if (Directory.Exists(path))
                {
                    var fileInfoList = Directory.GetFiles(path);
                    foreach (var file in fileInfoList)
                    {
                        string filename = Path.GetFileNameWithoutExtension(file);
                        string extension = Path.GetExtension(file);
                        if (filename.Length > 20)
                            filename = filename.Substring(0, 20);

                        docs.Add(new TaskTrackerDocument() { DocumentPath = "../../Docs/Tasks/" + id + "/" + filename, DocumentName = filename + extension });
                    }
                }

                taskViewModel.Documents = docs;
            }

            return View(taskViewModel);
        }

        public ActionResult View(int? id)
        {
            ViewBag.Id = id;
            var taskViewModel = new TaskTrackerViewModel();
            ViewBag.ProjectList = projectService.GetProjects(0);
            ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            ViewBag.MeetingList = meetingService.GetMeetingList(null, null, null, null, null);
            ViewBag.EmployeeList = employeeService.GetEmployeeList();
            ViewBag.TaskTypeList = taskTypeService.GetTaskTypeList();
            ViewBag.TaskTrackerCategoryList = taskTrackerCategoryService.GetTaskTrackerCategoryList();

            List<SelectListItem> StatusList = new List<SelectListItem>();
            StatusList.Add(new SelectListItem() { Text = "Not Started", Value = "1" });
            StatusList.Add(new SelectListItem() { Text = "In-Progress", Value = "2" });
            StatusList.Add(new SelectListItem() { Text = "Completed", Value = "3" });

            this.ViewBag.StatusList = new SelectList(StatusList, "Value", "Text");

            if (id != null)
            {
                var task = taskTrackerService.GetTaskById(id);
                taskViewModel = new TaskTrackerViewModel()
                {
                    ProjectId = task.ProjectId,
                    ProjectCode = task.ProjectCode,
                    MeetingId = task.MeetingId,
                    DateOfMeeting = task.DateOfMeeting,
                    TaskDescription = task.TaskDescription,
                    CurrentProgressNote = task.CurrentProgressNote,
                    AddToAgenda = task.AddToAgenda,
                    PriorityId = task.PriorityId,
                    AuthorityId = task.AuthorityId,
                    AssigneeId = task.AssigneeId,
                    DateOfAssignment = task.DateOfAssignment,
                    DueDate = task.DueDate,
                    TakenNextDueDate = task.TakenNextDueDate,
                    EmailTo = task.EmailTo,
                    EmailSubject = task.EmailSubject,
                    EmailAddress = task.EmailAddress,
                    TaskTypeId = task.TaskTypeId,
                    StatusId = task.StatusId,
                    TaskTrackerCategoryId = task.TaskTrackerCategoryId,
                };
            }

            return View(taskViewModel);

        }

        public ActionResult Update(int? id)
        {
            var taskViewModel = new TaskTrackerViewModel();
            ViewBag.ProjectList = projectService.GetProjects(0);
            ViewBag.PriorityList = commonDataService.GetCommonData((int)CommonDataType.Priority);
            ViewBag.MeetingList = meetingService.GetMeetingList(null, null, null, null, null);
            ViewBag.EmployeeList = employeeService.GetEmployeeList();
            ViewBag.TaskTypeList = taskTypeService.GetTaskTypeList();

            if (id != null)
            {
                var task = taskTrackerService.GetTaskById(id);
                taskViewModel = new TaskTrackerViewModel()
                {
                    ProjectId = task.ProjectId,
                    ProjectCode = task.ProjectCode,
                    MeetingId = task.MeetingId,
                    DateOfMeeting = task.DateOfMeeting,
                    TaskDescription = task.TaskDescription,
                    CurrentProgressNote = task.CurrentProgressNote,
                    AddToAgenda = task.AddToAgenda,
                    PriorityId = task.PriorityId,
                    AuthorityId = task.AuthorityId,
                    AssigneeId = task.AssigneeId,
                    DateOfAssignment = task.DateOfAssignment,
                    DueDate = task.DueDate,
                    TakenNextDueDate = task.TakenNextDueDate,
                    EmailTo = task.EmailTo,
                    EmailSubject = task.EmailSubject,
                    EmailAddress = task.EmailAddress,
                    TaskTypeId = task.TaskTypeId,
                };
            }

            return View(taskViewModel);
        }

        public ActionResult GetList(
            int? StatusId, int? ProjectId,
            int? MeetingId, int? TaskTypeId,
            int? AssigneeId, int? PriorityId, int? Id)
        {
            List<GetTaskList_Result> modelList = new List<GetTaskList_Result>();

            modelList = taskTrackerService.GetTaskList(StatusId, ProjectId, MeetingId, TaskTypeId, AssigneeId, Id).ToList();
            if ((PriorityId > 0) || (PriorityId != null))
                modelList = modelList.Where(t => t.PriorityId == PriorityId).ToList();

            JsonResult jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult = Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult GetListForReport(
            int? StatusId, int? ProjectId,
            int? MeetingId, int? TaskTypeId,
            int? AssigneeId, int? PriorityId, int? Id, string FromDate, string ToDate, string DateType)
        {
            List<GetTaskList_Result> modelList = new List<GetTaskList_Result>();
            var newProjectId = ProjectId;
            var newStatusId = StatusId;

            if (ProjectId == 1)
                ProjectId = null;

            if (StatusId == 111)
                StatusId = null;

            modelList = taskTrackerService.GetTaskList(StatusId, ProjectId, MeetingId, TaskTypeId, AssigneeId, Id).ToList();

            if (newProjectId == 1)
                modelList = modelList.Where(t => t.ProjectId == null).ToList();

            if ((PriorityId > 0) || (PriorityId != null))
                modelList = modelList.Where(t => t.PriorityId == PriorityId).ToList();

            int[] statusIds = { 1, 2 };

            if (newStatusId == 111)
            {
                modelList = modelList.Where(t => statusIds.ToList().Contains(t.StatusId.Value)).ToList();
            }

            if (DateType == "0")
                modelList = modelList.Where(t => t.DateOfAssignment >= DateTime.Parse(FromDate) && t.DateOfAssignment <= DateTime.Parse(ToDate)).ToList();
            else if (DateType == "1")
                modelList = modelList.Where(t => t.DueDate >= DateTime.Parse(FromDate) && t.DueDate <= DateTime.Parse(ToDate)).ToList();
            else if (DateType == "2")
                modelList = modelList.Where(t => t.ScheduleTime >= DateTime.Parse(FromDate) && t.scheduleDate <= DateTime.Parse(ToDate)).ToList();

            JsonResult jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult = Json(new { data = modelList }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult TaskView()
        {
            return View();
        }

        public PartialViewResult ScheduleDate(int? StatusId, int? ProjectId,
            int? MeetingId, int? TaskTypeId,
            int? AssigneeId, int? PriorityId, int? Id, string FromDate)
        {
            List<GetTaskList_Result> modelList = new List<GetTaskList_Result>();
            StringBuilder sbTable = new StringBuilder();

            if (FromDate != null)
            {
                var newProjectId = ProjectId;
                var newStatusId = StatusId;

                if (ProjectId == 1)
                    ProjectId = null;

                if (StatusId == 111)
                    StatusId = null;

                modelList = taskTrackerService.GetTaskList(StatusId, ProjectId, MeetingId, TaskTypeId, AssigneeId, null).ToList();

                if (newProjectId == 1)
                    modelList = modelList.Where(t => t.ProjectId == null).ToList();

                if ((PriorityId > 0) || (PriorityId != null))
                    modelList = modelList.Where(t => t.PriorityId == PriorityId).ToList();

                int[] statusIds = { 1, 2 };

                if (newStatusId == 111)
                {
                    modelList = modelList.Where(t => statusIds.ToList().Contains(t.StatusId.Value)).ToList();
                }

                modelList = modelList.Where(t => t.scheduleDate >= DateTime.Parse(FromDate) && t.scheduleDate <= DateTime.Parse(FromDate).AddDays(7)).ToList();


                sbTable.Append("<table class=\"table table-striped table-bordered\" style=\"width:100%\">");
                sbTable.Append("<tr>");
                var today = CommonService.GetClientDate(Request);
                for (int i = 0; i < 7; i++)
                {
                    DateTime dateTime = DateTime.Parse(FromDate).AddDays(i);

                    if (dateTime == today.Date)
                    {
                        sbTable.Append("<td style=\"width:14%!important\" class='today'>" + DateTime.Parse(FromDate).AddDays(i).ToString("dd/MM/yyyy")
                        + "<br />" + DateTime.Parse(FromDate).AddDays(i).DayOfWeek + "</td>");
                    }
                    else
                    {
                        sbTable.Append("<td style=\"width:14%!important\" class='th" + DateTime.Parse(FromDate).AddDays(i).DayOfWeek + "'>" + DateTime.Parse(FromDate).AddDays(i).ToString("dd/MM/yyyy")
                        + "<br />" + DateTime.Parse(FromDate).AddDays(i).DayOfWeek + "</td>");
                    }
                }
                sbTable.Append("</tr>");

                sbTable.Append("<tr>");
                for (int i = 0; i < 7; i++)
                {
                    DateTime dateTime = DateTime.Parse(FromDate).AddDays(i);
                    if (dateTime == today.Date)
                    {
                        sbTable.Append("<td style='padding:10px' class='today'>");
                    }
                    else
                    {
                        sbTable.Append("<td style='padding:10px'>");
                    }
                    var dateList = modelList.Where(t => t.scheduleDate == dateTime).OrderBy(t => t.ScheduleTime);
                    if (dateList != null)
                    {
                        foreach (var task in dateList)
                        {
                            var time = (task.ScheduleTime != null) ? task.ScheduleTime.Value.ToString("MM/dd/yyyy HH:mm") + "<br />" : "";

                            if (String.IsNullOrEmpty(time))
                            {
                                time = (task.scheduleDate != null) ? task.scheduleDate.Value.AddHours(10).ToString("MM/dd/yyyy HH:mm") + "<br />" : "";
                            }

                            //sbTable.Append("<div class='" + task.ST + " p-2'>" + time + "<a class='view' href='#'>" + task.TaskDescription + "</a><br /><div class='" + task.ST + "SR " + task.ST + "'>" + task.ST + "</div></div><hr />");
                            sbTable.Append("<div class='" + task.ST + " p-2'>(<a class='taskedit' target='_blank' href='/TaskTracker/Details/" + task.Id + "'>" + task.Id + "</a>)<br />" + time + "<a class='taskedit' target = '_blank' href = '/TaskTracker/Details/" + task.Id + "' > " + task.TaskDescription + " </a><br /><div class='" + task.ST + "SR " + task.ST + "'><b>" + task.ST + "</b></div></div><hr />");
                        }
                    }
                    sbTable.Append("</td>");
                }
                sbTable.Append("</tr>");

                sbTable.Append("</table>");
            }

            return PartialView("~/Views/TaskTracker/ScheduleDate.cshtml", sbTable.ToString());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(TaskTrackerViewModel model)
        {
            string newData = string.Empty, oldData = string.Empty;
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            try
            {
                var userId = User.Identity.GetUserId();
                var userName = User.Identity.Name;

                int id = model.Id;
                WFM_TaskTracker taskTracker = null;
                List<WFM_TaskTracker> taskTrackerList = new List<WFM_TaskTracker>();
                DateTime? scheduleTime = null;

                var clientDate = CommonService.GetClientDate(Request);

                if (model.Id == 0)
                {
                    if (model.IsRecurring)
                    {
                        List<DateTime> recurrence = new List<DateTime>();
                        int gap = model.Frequency.Value;
                        int upto = 1;
                        if (model.RecurringEndDate != null)
                            upto = (model.RecurringEndDate - model.ScheduleDate).Value.Days;

                        for (int i = 0; i <= upto; i = i + gap)
                        {
                            var scheduleDate = model.ScheduleDate.Value.AddDays(i);
                            if (scheduleDate.DayOfWeek == DayOfWeek.Saturday)
                            {
                                continue;
                            }
                            else if (scheduleDate.DayOfWeek == DayOfWeek.Sunday)
                            {
                                continue;
                            }

                            scheduleTime = Convert.ToDateTime(scheduleDate.ToString("MM/dd/yyyy") + " " + model.ScheduleTimeHrs + ":" + model.ScheduleTimeMins + ":00");

                            taskTracker = new WFM_TaskTracker
                            {
                                ProjectId = model.ProjectId,
                                ProjectCode = model.ProjectCode,
                                MeetingId = model.MeetingId,
                                //MeetingTypeId = model.MeetingTypeId,
                                AddToAgendaMeetingId = model.AddToAgendaMeetingId,
                                DateOfMeeting = model.DateOfMeeting,
                                TaskDescription = "[RT]-" + model.TaskDescriptionDB,
                                CurrentProgressNote = model.CurrentProgressNote,
                                PriorityId = model.PriorityId,
                                AuthorityId = model.AuthorityId,
                                AssigneeId = model.AssigneeId,
                                DateOfAssignment = model.DateOfAssignment,
                                DueDate = model.DueDate,
                                TakenNextDueDate = model.TakenNextDueDate,
                                EmailTo = model.EmailTo,
                                EmailSubject = model.EmailSubject,
                                EmailAddress = model.EmailAddress,
                                StatusId = model.StatusId,
                                CreatedBy = userId,
                                CreatedDate = clientDate,
                                TaskTypeId = model.TaskTypeId,
                                TaskTrackerCategoryId = model.TaskTrackerCategoryId,
                                ScheduleDate = scheduleDate,
                                ScheduleTime = scheduleTime,
                                NoOfHrs = model.NoOfHrs,
                                ActuallyStartedDate = model.ActuallyStartedDate, //Meeting Date
                            };

                            taskTrackerService.SaveOrUpdate(taskTracker);
                            UpdateTaskDetails(taskTracker, model, smtpSection, userName);
                        }
                    }
                    else
                    {
                        if (model.ScheduleDate != null)
                            scheduleTime = Convert.ToDateTime(model.ScheduleDate.Value.ToString("MM/dd/yyyy") + " " + model.ScheduleTimeHrs + ":" + model.ScheduleTimeMins + ":00");
                        else
                            scheduleTime = null;

                        taskTracker = new WFM_TaskTracker
                        {
                            ProjectId = model.ProjectId,
                            ProjectCode = model.ProjectCode,
                            MeetingId = model.MeetingId,
                            AddToAgendaMeetingId = model.AddToAgendaMeetingId,
                            DateOfMeeting = model.DateOfMeeting,
                            TaskDescription = model.TaskDescriptionDB,
                            CurrentProgressNote = model.CurrentProgressNote,
                            PriorityId = model.PriorityId,
                            AuthorityId = model.AuthorityId,
                            AssigneeId = model.AssigneeId,
                            DateOfAssignment = model.DateOfAssignment,
                            DueDate = model.DueDate,
                            TakenNextDueDate = model.TakenNextDueDate,
                            EmailTo = model.EmailTo,
                            EmailSubject = model.EmailSubject,
                            EmailAddress = model.EmailAddress,
                            StatusId = model.StatusId,
                            CreatedBy = userId,
                            CreatedDate = clientDate,
                            TaskTypeId = model.TaskTypeId,
                            TaskTrackerCategoryId = model.TaskTrackerCategoryId,
                            ScheduleDate = model.ScheduleDate,
                            ScheduleTime = scheduleTime,
                            NoOfHrs = model.NoOfHrs,
                            ActuallyStartedDate = model.ActuallyStartedDate, //Meeting Date
                        };

                        taskTrackerService.SaveOrUpdate(taskTracker);
                        UpdateTaskDetails(taskTracker, model, smtpSection, userName);
                    }
                }
                else
                {
                    if (model.ScheduleDate != null)
                        scheduleTime = Convert.ToDateTime(model.ScheduleDate.Value.ToString("MM/dd/yyyy"));
                    else
                        scheduleTime = null;

                    taskTracker = taskTrackerService.GetTaskById(model.Id);

                    taskTracker.ProjectId = model.ProjectId;
                    taskTracker.ProjectCode = model.ProjectCode;
                    taskTracker.MeetingId = model.MeetingId;
                    taskTracker.DateOfMeeting = model.DateOfMeeting;
                    taskTracker.TaskDescription = model.TaskDescriptionDB;
                    taskTracker.CurrentProgressNote = model.CurrentProgressNote;
                    taskTracker.AddToAgendaMeetingId = model.AddToAgendaMeetingId;
                    taskTracker.PriorityId = model.PriorityId;
                    taskTracker.AuthorityId = model.AuthorityId;
                    taskTracker.AssigneeId = model.AssigneeId;
                    taskTracker.DateOfAssignment = model.DateOfAssignment;
                    taskTracker.DueDate = model.DueDate;
                    taskTracker.TakenNextDueDate = model.TakenNextDueDate;
                    taskTracker.EmailTo = model.EmailTo;
                    taskTracker.EmailSubject = model.EmailSubject;
                    taskTracker.EmailAddress = model.EmailAddress;
                    taskTracker.StatusId = model.StatusId;
                    taskTracker.UpdatedBy = userId;
                    taskTracker.UpdatedDate = clientDate;
                    taskTracker.TaskTypeId = model.TaskTypeId;
                    taskTracker.TaskTrackerCategoryId = model.TaskTrackerCategoryId;
                    taskTracker.ScheduleDate = model.ScheduleDate;
                    taskTracker.ScheduleTime = scheduleTime;
                    taskTracker.NoOfHrs = model.NoOfHrs;
                    taskTracker.ActuallyStartedDate = model.ActuallyStartedDate;

                    taskTrackerService.SaveOrUpdate(taskTracker);
                    UpdateTaskDetails(taskTracker, model, smtpSection, userName);
                }

                TempData["Message"] = "<div id='flash-success'>Record Saved Successfully.</div>";
            }
            catch (Exception ex)
            {
                TempData["Message"] = "<span id='flash-error'>Error.</span>" + ex.Message;
            }


            return RedirectToAction("Index", "TaskTracker");
        }

        private void UpdateTaskDetails(WFM_TaskTracker taskTracker, TaskTrackerViewModel model, SmtpSection smtpSection, string userName)
        {
            if (model.AssigneeIdList != null)
            {
                var assignees = taskTrackerService.GetAssigneesByTaskId(taskTracker.Id);
                taskTrackerService.RemoveAssignees(assignees);
            }

            string status = "Not Started";
            if (taskTracker.StatusId == 3)
                status = "Completed";

            #region Sending Emails
            //Set Assignee List
            var assigneeList = model.AssigneeIdList;

            string body = string.Empty;
            string newAssignees = string.Empty;

            if (assigneeList != null)
            {
                var employeeList = employeeService.GetEmployeeByIdList(assigneeList);

                foreach (var employee in employeeList)
                {
                    if (employee != null)
                    {
                        newAssignees += employee.Name + ", ";
                    }
                }
            }

            var authorityOf = employeeService.GetEmployeeById(taskTracker.AuthorityId);

            if (assigneeList != null)
            {
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
                    "<td style='padding:5px'>" + ((taskTracker.TaskTrackerCategoryId == null) ? "" : divideService.GetDivisionById(taskTracker.TaskTrackerCategoryId).Name) + "</td>" +
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

                foreach (var assignee in assigneeList)
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
                                "info@emlconsultants.com",
                                "TTS-" + taskTracker.Id.ToString("0000") + " ~ [" + status + "] : " + taskTracker.TaskDescription,
                                body);
                    }
                }
            }
            #endregion

            HttpFileCollectionBase taskDocuments = Request.Files;

            for (int i = 0; i < taskDocuments.Count; i++)
            {
                HttpPostedFileBase file = taskDocuments[i];
                try
                {
                    if (!String.IsNullOrEmpty(file.FileName))
                    {
                        string strFilePath = string.Format("{0}\\{1}", Server.MapPath("~/Docs/Tasks/" + taskTracker.Id), file.FileName);
                        if (!Directory.Exists(strFilePath))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Docs/Tasks/" + taskTracker.Id));
                        }

                        file.SaveAs(strFilePath);

                        WFM_TaskTrackerDocument taskTrackerDocumnet = new WFM_TaskTrackerDocument();
                        taskTrackerDocumnet.DocumentName = file.FileName;
                        taskTrackerDocumnet.TaskId = taskTracker.Id;
                        taskTrackerService.SaveOrUpdate(taskTrackerDocumnet);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public PartialViewResult Documents(int taskid)
        {

            //var documents = taskTrackerService.GetDocumentsByTaskId(taskid);

            List<TaskTrackerDocument> docs = new List<TaskTrackerDocument>();

            string path = Path.Combine(Server.MapPath("~/Docs/Tasks/" + taskid));

            //foreach (var document in documents)
            //{
            if (Directory.Exists(path))
            {
                var fileInfoList = Directory.GetFiles(path);
                foreach (var file in fileInfoList)
                {
                    string filename = Path.GetFileNameWithoutExtension(file);
                    string extension = Path.GetExtension(file);

                    string displayFilename = string.Empty;
                    if (filename.Length > 20)
                        displayFilename = filename.Substring(0, 20);

                    docs.Add(new TaskTrackerDocument() { DocumentPath = "../../Docs/Tasks/" + taskid + "/" + filename + extension, DocumentName = displayFilename + extension });
                }
            }
            //}
            return PartialView("~/Views/TaskTracker/Documents.cshtml", docs);
        }

        [HttpPost]
        public async Task<ActionResult> ImportTasks()
        {
            HttpFileCollectionBase formFiles = Request.Files;

            var userId = User.Identity.GetUserId();

            if (formFiles == null) return Json(new { Status = 0, Message = "No File Selected" });
            ExcelService excelService = new ExcelService();
            DataTable dtSheet1 = new DataTable();
            string message = null;

            string taskDescription = null;

            try
            {
                for (int i = 0; i < formFiles.Count; i++)
                {
                    HttpPostedFileBase file = formFiles[i];

                    try
                    {
                        string strFilePath = string.Format("{0}\\{1}", Server.MapPath("~/Files/Tasks"), file.FileName);
                        if (!Directory.Exists(strFilePath))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Files/Tasks"));
                        }

                        if (System.IO.File.Exists(strFilePath))
                        {
                            System.IO.File.Delete(strFilePath);
                        }

                        file.SaveAs(strFilePath);

                        string extension = System.IO.Path.GetExtension(file.FileName).ToLower();

                        string connString = "";

                        if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                        }

                        dtSheet1 = excelService.ImportExceltoDatabase(strFilePath, connString, "Sheet1");

                        WFM_TaskTracker taskTracker = null;
                        List<WFM_TaskTracker> taskTrackerList = new List<WFM_TaskTracker>();
                        DateTime? scheduleTime = null;
                        DateTime? scheduleDate = null;
                        DateTime? assignDate = null;
                        DateTime? dueDate = null;
                        var clientDate = CommonService.GetClientDate(Request);

                        int? projectId = null;
                        int? meetingId = null;
                        int? categoryId = null;
                        int? priorityId = null;
                        int? authorityId = null;
                        int? typeId = null;
                        int? noOfDays = null;

                        foreach (DataRow dr in dtSheet1.Rows)
                        {
                            try
                            {
                                scheduleTime = null; // Convert.ToDateTime(dr["ScheduleDate"].ToString());//.AddHours(int.Parse(dr["ScheduleTimeHrs"].ToString()));// + " " +  + ":" + dr["ScheduleTimeMins"].ToString() + ":00");
                            }
                            catch (Exception)
                            {
                                scheduleTime = null;
                            }

                            try
                            {
                                assignDate = Convert.ToDateTime(dr["DateOfAssignment"].ToString());
                            }
                            catch (Exception)
                            {
                                assignDate = null;
                            }

                            try
                            {
                                scheduleDate = Convert.ToDateTime(dr["ScheduleDate"].ToString());
                            }
                            catch (Exception)
                            {
                                scheduleDate = null;
                            }

                            try
                            {
                                dueDate = Convert.ToDateTime(dr["DueDate"].ToString());
                            }
                            catch (Exception)
                            {
                                dueDate = null;
                            }

                            try
                            {
                                noOfDays = int.Parse(dr["NoOfDays"].ToString());
                            }
                            catch (Exception)
                            {
                                noOfDays = null;
                            }

                            try
                            {
                                if (string.IsNullOrEmpty(dueDate?.ToString()))
                                {
                                    dueDate = scheduleDate.Value.AddDays(noOfDays.Value);
                                }
                            }
                            catch (Exception)
                            {
                                dueDate = null;
                            }

                            try
                            {
                                projectId = projectService.GetProjectByCode(dr["ProjectCode"].ToString()).Id;
                            }
                            catch (Exception)
                            {
                                projectId = null;
                            }


                            try
                            {
                                meetingId = null; // meetingService.GetMeetingByName(dr["Meeting"].ToString()).Id;
                            }
                            catch (Exception)
                            {
                                meetingId = null;
                            }

                            try
                            {
                                categoryId = divideService.GetDivisionByName(dr["Division"].ToString()).Id;
                            }
                            catch (Exception)
                            {
                                categoryId = null;
                            }

                            try
                            {
                                priorityId = commonDataService.GetCommonDataByName(dr["Priority"].ToString()).Id;
                            }
                            catch (Exception)
                            {
                                priorityId = null;
                            }

                            try
                            {
                                authorityId = employeeService.GetEmployeeByCode(dr["AuthorityEmployeeCode"].ToString()).Id;
                            }
                            catch (Exception)
                            {
                                authorityId = null;
                            }

                            try
                            {
                                typeId = taskTypeService.GetTaskTypeByName(dr["Type"].ToString()).Id;
                            }
                            catch (Exception)
                            {
                                typeId = null;
                            }

                            taskDescription = dr["Description"].ToString();

                            if(!string.IsNullOrEmpty(taskDescription))
                            {
                                taskTracker = new WFM_TaskTracker
                                {
                                    ProjectId = projectId,
                                    ProjectCode = null, //dr["ProjectCode"].ToString(),
                                    MeetingId = meetingId,
                                    AddToAgendaMeetingId = null,
                                    DateOfMeeting = null, //Convert.ToDateTime(dr["DateOfMeeting"].ToString()),
                                    TaskDescription = dr["Description"].ToString(),
                                    CurrentProgressNote = "",
                                    PriorityId = priorityId,
                                    AuthorityId = authorityId,
                                    AssigneeId = null,
                                    DateOfAssignment = assignDate,
                                    DueDate = dueDate,
                                    StatusId = 1,
                                    CreatedBy = userId,
                                    CreatedDate = clientDate,
                                    TaskTypeId = typeId,
                                    TaskTrackerCategoryId = categoryId,
                                    ScheduleDate = scheduleDate,
                                    ScheduleTime = scheduleTime,
                                    NoOfHrs = null, //dr["NoOfHrs"].ToString(),
                                    ActuallyStartedDate = null
                                };//

                                taskTrackerService.SaveOrUpdate(taskTracker);

                                foreach (var assignee in dr["AssigneesEmployeeCodes"].ToString().Split(','))
                                {
                                    try
                                    {
                                        int assigneeId = employeeService.GetEmployeeByCode(assignee).Id;

                                        taskTrackerService.SaveOrUpdate(new WFM_TaskTrackerAssignee()
                                        {
                                            TaskTrackerId = taskTracker.Id,
                                            EmployeeId = assigneeId
                                        });
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                //UpdateTaskDetails(taskTracker, model, smtpSection, userName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        message += string.Format("\n{0}-{2}-Error: {1}", file.FileName, ex.InnerException, taskDescription);
                    }
                }

                //message = "File Imported Successfully";
                //Request.Files.
                return Json(new { Status = 1, Message = message });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = ex.Message });
            }
        }
    }
}