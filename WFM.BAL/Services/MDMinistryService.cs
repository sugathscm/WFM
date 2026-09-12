using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class MDMinistryService
    {
        public List<GetMinistries_Result> GetMinistryList(int? statusId, int? instituteId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.GetMinistries(statusId, instituteId).OrderBy(d => d.Id).ToList();
            }
        }
        public List<GetMinistryLineAgencies_Result> GetMinistryLineAgencies(int? statusId, int? ministryId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.GetMinistryLineAgencies(statusId, ministryId).OrderBy(d => d.Id).ToList();
            }
        }

    }
}
