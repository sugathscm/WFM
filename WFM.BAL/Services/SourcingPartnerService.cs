using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class SourcingPartnerService
    {
        public List<WFM_SourcingPartner> GetSourdingPartnerList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_SourcingPartner.Where(s => s.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }

        public WFM_SourcingPartner GetSourdingPartnerById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_SourcingPartner.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_SourcingPartner sourcingPartner)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (sourcingPartner.Id == 0)
                {
                    entities.WFM_SourcingPartner.Add(sourcingPartner);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(sourcingPartner).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

    }
}
