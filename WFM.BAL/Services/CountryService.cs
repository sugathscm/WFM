using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class CountryService
    {
        public List<WFM_Country> GetCountryList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Country.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }

        public WFM_Country GetCountryById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Country.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_Country designation)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (designation.Id == 0)
                {
                    entities.WFM_Country.Add(designation);
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
