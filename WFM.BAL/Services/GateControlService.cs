using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class GateControlService
    {
        public List<WFM_GateControl> GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_GateControl.ToList();
            }
        }

        public WFM_GateControl GetByProjectId(int? projectId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_GateControl.Where(g => g.ProjectId == projectId).FirstOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_GateControl gateControl)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (gateControl.Id == 0)
                {
                    entities.WFM_GateControl.Add(gateControl);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(gateControl).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
