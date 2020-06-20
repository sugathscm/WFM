using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class DesignationService
    {
        public List<WFM_Designation> GetDesignationList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Designation.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }

        public WFM_Designation GetDesignationById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Designation.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_Designation designation)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (designation.Id == 0)
                {
                    entities.WFM_Designation.Add(designation);
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
