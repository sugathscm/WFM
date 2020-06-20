using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class DivisionService
    {
        public List<WFM_Division> GetDivisionList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Division.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }

        public WFM_Division GetDivisionById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Division.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_Division designation)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (designation.Id == 0)
                {
                    entities.WFM_Division.Add(designation);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(designation).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
