using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class TaskTrackerCategoryService
    {
        public List<WFM_TaskTrackerCategory> GetTaskTrackerCategoryList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskTrackerCategory.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }

        public WFM_TaskTrackerCategory GetTaskTrackerCategoryById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskTrackerCategory.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public WFM_TaskTrackerCategory GetTaskTrackerCategoryByName(string name)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskTrackerCategory.Where(s => s.Name == name).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_TaskTrackerCategory taskTrackerCategory)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (taskTrackerCategory.Id == 0)
                {
                    entities.WFM_TaskTrackerCategory.Add(taskTrackerCategory);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(taskTrackerCategory).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
