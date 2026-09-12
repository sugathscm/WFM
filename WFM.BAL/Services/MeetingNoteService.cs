using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class MeetingNoteService
    {
        public List<WFM_MeetingNote> GetMeetingNoteList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_MeetingNote.OrderBy(d => d.Id).ToList();
            }
        }

        public WFM_MeetingNote GetMeetingNoteById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_MeetingNote.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_MeetingNote meeting)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (meeting.Id == 0)
                {
                    entities.WFM_MeetingNote.Add(meeting);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(meeting).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
