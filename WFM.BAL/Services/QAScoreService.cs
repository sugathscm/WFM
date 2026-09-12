using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class QAScoreService
    {
        public List<WFM_QAScore> GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_QAScore.ToList();
            }
        }

        public WFM_QAScore GetByProjectId(int? projectId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_QAScore.Where(q => q.ProjectId == projectId).FirstOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_QAScore qaScore)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (qaScore.Id == 0)
                {
                    entities.WFM_QAScore.Add(qaScore);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(qaScore).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
