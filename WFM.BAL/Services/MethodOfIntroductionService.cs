using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class MethodOfIntroductionService
    {
        public List<WFM_MethodOfIntroduction> GetMethodOfIntroductionList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_MethodOfIntroduction.Where(m => m.IsActive == true).ToList();
            }
        }
    }
}
