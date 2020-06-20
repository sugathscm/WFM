using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class PriorityFrameworkService
    {
        public List<WFM_PriorityFramework> GetPriorityFrameworkList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_PriorityFramework.Where(m => m.IsActive == true).ToList();
            }
        }
    }
}
