using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class PrincipalService
    {
        public List<WFM_Principal> GetPrincipalList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Principal.Include("WFM_Country").Where(o => o.IsActive == true).OrderBy(o => o.Name).ToList();
            }
        }

        public WFM_Principal GetPrincipalById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Principal.Include("WFM_Country").Where(o => o.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_Principal principal)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (principal.Id == 0)
                {
                    entities.WFM_Principal.Add(principal);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(principal).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
