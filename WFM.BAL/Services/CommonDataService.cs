using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class CommonDataService
    {
        public List<WFM_CommonData> GetCommonData(int type)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_CommonData.Where(o => o.Type == type).OrderBy(o => o.DisplayOrder).ToList();
            }
        }
    }
}
