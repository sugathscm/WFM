using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ProjectTypeService
    {
        public List<WFM_ProjectType> GetProjectTypeList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectType.ToList();
            }
        }
    }
}

