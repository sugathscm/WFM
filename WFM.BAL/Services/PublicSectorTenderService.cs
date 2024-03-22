using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.ViewModels;
using WFM.DAL;


namespace WFM.BAL.Services
{
    public class PublicSectorTenderService
    {
        public void SaveOrUpdate(WFM_Form8B form8B)
        {
            try
            {
                using (LinkManagementEntities entities = new LinkManagementEntities())
                {
                    if (form8B.Id == 0)
                    {
                        entities.WFM_Form8B.Add(form8B);
                        entities.SaveChanges();
                    }
                    else
                    {
                        entities.Entry(form8B).State = System.Data.Entity.EntityState.Modified;
                        entities.SaveChanges();
                    }
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
        
        public void SaveOrUpdate(WFM_Form12B form12B)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (form12B.Id == 0)
                {
                    entities.WFM_Form12B.Add(form12B);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(form12B).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
