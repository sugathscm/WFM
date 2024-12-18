using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class MeetingTypeService
    {
        public List<WFM_MeetingType> GetMeetingList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_MeetingType.OrderBy(d => d.Topic).ToList();
            }
        }

        public WFM_MeetingType GetMeetingById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_MeetingType.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public WFM_MeetingType GetMeetingByName(string name)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_MeetingType.Where(s => s.Topic == name).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_MeetingType meeting)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (meeting.Id == 0)
                {
                    entities.WFM_MeetingType.Add(meeting);
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
