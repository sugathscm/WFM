using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.ModelsView
{
    public class MeetingNoteViewModel
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string DateString { get; set; }
        public string NoteGivenBy { get; set; }
        public Nullable<int> NoteTakenBy { get; set; }
        public string Note { get; set; }
        public Nullable<int> MeetingId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string MeetingName { get; set; }
        public string NoteGivenByName { get; set; }
        public string NoteTakenByName { get; set; }
    }
}