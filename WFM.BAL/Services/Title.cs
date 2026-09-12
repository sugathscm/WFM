using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class TitleService
    {
        public List<WFM_Title> GetTitleList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Title.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }

        public WFM_Title GetTitleById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Title.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_Title Title)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (Title.Id == 0)
                {
                    entities.WFM_Title.Add(Title);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(Title).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
