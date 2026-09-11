using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ProjectHandoverService
    {
        public List<WFM_ProjectHandover> GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectHandover.ToList();
            }
        }

        public WFM_ProjectHandover GetByProjectId(int? projectId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectHandover.Where(h => h.ProjectId == projectId).FirstOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_ProjectHandover handover)
        {
            bool[] items = { handover.H1 == true, handover.H2 == true, handover.H3 == true, handover.H4 == true,
                              handover.H5 == true, handover.H6 == true, handover.H7 == true, handover.H8 == true };
            handover.ItemsDone = items.Count(x => x);
            handover.HandoverStatus = (handover.ItemsDone == 8) ? "READY" : "NOT READY";
            handover.HandoverDone = handover.ItemsDone == 8;

            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (handover.Id == 0)
                {
                    entities.WFM_ProjectHandover.Add(handover);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(handover).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
