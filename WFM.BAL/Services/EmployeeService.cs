using System;
using System.Collections.Generic;
using System.Linq;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class EmployeeService
    {
        public List<WFM_Employee> GetEmployeeList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Employee.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }
        public List<WFM_Employee> GetAllEmployeeList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Employee.OrderBy(d => d.Name).ToList();
            }
        }

        public WFM_Employee GetEmployeeById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Employee.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public WFM_Employee GetEmployeeByUserId(string userId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Employee.Where(s => s.UserId == userId).OrderByDescending(s => s.Id).FirstOrDefault();
            }
        }

        public WFM_Employee GetEmployeeByCode(string code)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Employee.Where(s => s.Code == code).SingleOrDefault();
            }
        }

        public List<WFM_Employee> GetEmployeeByIdList(List<int?> idList)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Employee.Where(s => idList.Contains(s.Id)).ToList();
            }
        }

        public void SaveOrUpdate(WFM_Employee designation)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (designation.Id == 0)
                {
                    entities.WFM_Employee.Add(designation);
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