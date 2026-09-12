using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ProposalOutcomeService
    {
        public List<WFM_ProposalOutcome> GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProposalOutcome.ToList();
            }
        }

        public WFM_ProposalOutcome GetByProjectId(int? projectId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProposalOutcome.Where(o => o.ProjectId == projectId).FirstOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_ProposalOutcome proposalOutcome)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (proposalOutcome.Id == 0)
                {
                    entities.WFM_ProposalOutcome.Add(proposalOutcome);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(proposalOutcome).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
