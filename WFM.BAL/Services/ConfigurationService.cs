using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ConfigurationService
    {
        public List<WFM_Configuration> GetList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Configuration.ToList();
            }
        }

        public WFM_Configuration GetByName(string name)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Configuration.Where(c => c.Name == name).FirstOrDefault();
            }
        }

        public string GetValue(string name, string defaultValue)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                var config = entities.WFM_Configuration.Where(c => c.Name == name).FirstOrDefault();
                return (config == null) ? defaultValue : config.Value;
            }
        }

        public void SaveOrUpdate(string name, string value)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                var config = entities.WFM_Configuration.Where(c => c.Name == name).FirstOrDefault();
                if (config == null)
                {
                    entities.WFM_Configuration.Add(new WFM_Configuration() { Name = name, Value = value });
                }
                else
                {
                    config.Value = value;
                }
                entities.SaveChanges();
            }
        }
    }
}
