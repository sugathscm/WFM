using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public class TaskTrackerViewModel
    {
        public int Id { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public Nullable<int> MeetingId { get; set; }
        public Nullable<int> AddToAgendaMeetingId { get; set; }
        public Nullable<System.DateTime> DateOfMeeting { get; set; }
        public string TaskDescription { get; set; }
        public string TaskDescriptionDB { get; set; }
        public string CurrentProgressNote { get; set; }
        public string CurrentProgressNoteView { get; set; }
        public Nullable<bool> AddToAgenda { get; set; }
        public Nullable<int> PriorityId { get; set; }
        public Nullable<int> AuthorityId { get; set; }
        public Nullable<int> AssigneeId { get; set; }
        public List<Nullable<int>> AssigneeIdList { get; set; }
        public Nullable<System.DateTime> DateOfAssignment { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> TakenNextDueDate { get; set; }
        public Nullable<int> EmailTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> TaskTypeId { get; set; }
        public Nullable<int> TaskTrackerCategoryId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> ActuallyStartedDate { get; set; }
        public Nullable<System.DateTime> ActuallyCompletedDate { get; set; }
        public Nullable<System.DateTime> RecurringEndDate { get; set; }
        public string DateString { get; set; }
        public string ProjectName { get; set; }
        public string MeetingName { get; set; }
        public string Note { get; set; }
        public string Assignees { get; set; }
        public string AssigneeIds { get; set; }
        public Nullable<System.DateTime> ScheduleDate { get; set; }
        public Nullable<System.DateTime> ScheduleTime { get; set; }
        public int ScheduleTimeHrs { get; set; }
        public int ScheduleTimeMins { get; set; }
        public string NoOfHrs { get; set; }
        public bool IsRecurring { get; set; }
        public int? Frequency { get; set; }
        public List<TaskTrackerDocument> Documents { get; set; }
    }
}