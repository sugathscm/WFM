using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class OrganizationService
    {
        public List<WFM_Organization> GetAllOrganizationList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Organization.ToList();
            }
        }
        public List<WFM_Organization> GetOrganizationList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Organization.Where(s => s.IsActive == true).ToList();
            }
        }

        public WFM_Organization GetOrganizationById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Organization.Where(o => o.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_Organization organization)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (organization.Id == 0)
                {
                    entities.WFM_Organization.Add(organization);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(organization).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

    }
}
