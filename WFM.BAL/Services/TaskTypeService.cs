using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class TaskTypeService
    {
        public List<WFM_TaskType> GetTaskTypeList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskType.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }

        public WFM_TaskType GetTaskTypeById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskType.Where(s => s.Id == id).SingleOrDefault();
            }
        }
        public WFM_TaskType GetTaskTypeByName(string name)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskType.Where(s => s.Name == name).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_TaskType designation)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (designation.Id == 0)
                {
                    entities.WFM_TaskType.Add(designation);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(designation).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
