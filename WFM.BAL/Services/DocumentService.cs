using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class DocumentService
    {
        public List<WFM_Document> GetDocumentsByProjectType(int projectTypeId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Document.Where(s => s.ProjectTypeId == projectTypeId).ToList();
            }
        }

        public List<WFM_Document> GetDocumentsByProjectTypeWithFields(int projectTypeId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Document.Include("WFM_DocumentField").Where(s => s.ProjectTypeId == projectTypeId && s.HasFields == true).ToList();
            }
        }

    }
}
