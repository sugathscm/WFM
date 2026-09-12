using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace WFM.BAL.Services
{
    public class MeetingService
    {
        public List<GetMeetingList_Result> GetMeetingList(DateTime? fromDate, DateTime? toDate,int? typeId,int? participantId,int? Id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.GetMeetingList(fromDate, toDate, typeId, participantId, Id).ToList();
            }
        }

        public WFM_Meeting GetMeetingById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Meeting.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public List<WFM_FileNote> GetFileNotesByMeetingId(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_FileNote.Where(s => s.MeetingId == id).ToList();
            }
        }

        public List<WFM_TaskTracker> GetTasksByMeetingId(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskTracker.Where(s => s.MeetingId == id).ToList();
            }
        }

        public List<WFM_MeetingNote> GetMeetingNotesByMeetingId(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_MeetingNote.Where(s => s.MeetingId == id).ToList();
            }
        }

        public void SaveOrUpdate(WFM_Meeting meeting)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (meeting.Id == 0)
                {
                    entities.WFM_Meeting.Add(meeting);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(meeting).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

        public void SaveOrUpdate(WFM_MeetingIP meetingIP)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (meetingIP.Id == 0)
                {
                    entities.WFM_MeetingIP.Add(meetingIP);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(meetingIP).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

    }
}
