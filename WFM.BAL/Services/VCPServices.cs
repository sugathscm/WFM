using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class VCPService
    {
        public List<GetVCPList_Result> GetVCPList(int statusId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.GetVCPList(statusId).OrderBy(d => d.Id).ToList();
            }
        }

        public WFM_VCP GetVCPById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_VCP.Where(o => o.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_VCP vcp)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (vcp.Id == 0)
                {
                    entities.WFM_VCP.Add(vcp);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(vcp).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

    }
}
