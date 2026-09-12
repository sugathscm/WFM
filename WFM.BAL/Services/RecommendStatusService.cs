using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class RecommendStatusService
    {
        public List<WFM_RecommendStatus> GetRecommendStatusList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_RecommendStatus.Where(r => r.IsActive == true).OrderBy(r => r.Name).ToList();
            }
        }

        public WFM_RecommendStatus GetRecommendStatusById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_RecommendStatus.Where(r => r.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_RecommendStatus recommendStatus)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (recommendStatus.Id == 0)
                {
                    entities.WFM_RecommendStatus.Add(recommendStatus);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(recommendStatus).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
