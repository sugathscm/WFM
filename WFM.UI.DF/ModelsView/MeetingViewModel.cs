using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFM.UI.DF.Models;

namespace WFM.UI.DF.ModelsView
{
    public class MeetingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string Location { get; set; }
        public string DateString { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int? MeetingTypeId { get; set; }
        public int? ProjectId { get; set; }
        public string MeetingTypeName { get; set; }
        public string KPI { get; set; }
        public string ParticipantsExternal { get; set; }
        public List<int?> InternalParticipants { get; set; }
        public string InternalParticipantNames { get; set; }
        public string ExternalParticipants { get; set; }
        public string Remarks { get; set; }
        public bool KPIAchieved { get; set; }
        public int? RelatedTaskId { get; set; }

        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }

        public int? ParticipantId { get; set; }

        public List<TaskTrackerDocument> Documents { get; set; }

        public FileNoteViewModel MeetingNote { get; set; }
        public FileNoteViewModel FileNote { get; set; }
        public TaskTrackerViewModel Task { get; set; }

        public List<FileNoteViewModel> MeetingNotes { get; set; }
        public List<FileNoteViewModel> FileNotes { get; set; }
        public List<TaskTrackerViewModel> Tasks { get; set; }
    }
}