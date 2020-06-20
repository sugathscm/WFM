using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ContactService
    {
        public List<WFM_Contact> GetContactList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Contact.Where(s => s.IsActive == true).ToList();
            }
        }

        public WFM_Contact GetContactById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Contact.Include("WFM_Country").Where(o => o.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_Contact contact)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (contact.Id == 0)
                {
                    entities.WFM_Contact.Add(contact);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(contact).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

    }
}
