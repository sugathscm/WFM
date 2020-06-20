using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ProjectDocumentService
    {
        public List<WFM_ProjectDocument> GetProjectDocumentsByProjectType(int projectTypeId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectDocument.Include("WFM_Document").Where(s => s.WFM_Document.ProjectTypeId == projectTypeId).ToList();
            }
        }
    }
}
