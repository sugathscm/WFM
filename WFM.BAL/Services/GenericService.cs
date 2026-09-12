using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class GenericService
    {
        public List<T> GetList<T>() where T : class
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.Set<T>().ToList();
            }
        }

        public void SaveOrUpdate<T>(T t, int id) where T : class
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                try
                {
                    if (id == 0)
                    {
                        entities.Set<T>().Add(t);
                        entities.SaveChanges();
                    }
                    else
                    {
                        entities.Entry(t).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }

            }
        }
    }
}
