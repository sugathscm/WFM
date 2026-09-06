using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class BidRecommendationService
    {
        public List<WFM_BidRecommendation> GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_BidRecommendation.ToList();
            }
        }

        public WFM_BidRecommendation GetByProjectId(int? projectId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_BidRecommendation.Where(b => b.ProjectId == projectId).FirstOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_BidRecommendation bidRecommendation)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (bidRecommendation.Id == 0)
                {
                    entities.WFM_BidRecommendation.Add(bidRecommendation);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(bidRecommendation).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
