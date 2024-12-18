using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class FileNoteService
    {
        public List<WFM_FileNote> GetFileNoteList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_FileNote.OrderBy(d => d.Id).ToList();
            }
        }

        public WFM_FileNote GetFileNoteById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_FileNote.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_FileNote meeting)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (meeting.Id == 0)
                {
                    entities.WFM_FileNote.Add(meeting);
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
