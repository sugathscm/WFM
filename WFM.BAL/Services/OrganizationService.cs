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
    }
}
