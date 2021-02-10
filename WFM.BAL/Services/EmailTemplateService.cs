using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class EmailTemplateService
    {
        public WFM_EmailTemplates GetEmailTemplateById(int id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {                
                    return entities.WFM_EmailTemplates
                        .Where(p =>  p.Id == id)
                        .SingleOrDefault();
            }
        }
    }
}
