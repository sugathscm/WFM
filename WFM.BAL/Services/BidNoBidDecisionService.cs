using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class BidNoBidDecisionService
    {
        public List<WFM_BidNoBidDecision> GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_BidNoBidDecision.ToList();
            }
        }

        public WFM_BidNoBidDecision GetByProjectId(int? projectId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_BidNoBidDecision.Where(b => b.ProjectId == projectId).FirstOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_BidNoBidDecision bidNoBidDecision)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (bidNoBidDecision.Id == 0)
                {
                    entities.WFM_BidNoBidDecision.Add(bidNoBidDecision);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(bidNoBidDecision).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
