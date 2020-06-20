using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class StatusService
    {
        public List<WFM_ProjectStatus> GetStatusList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectStatus.ToList();
            }
        }
    }
}
