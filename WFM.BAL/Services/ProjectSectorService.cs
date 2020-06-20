using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ProjectSectorService
    {
        public List<WFM_ProjectSector> GetSubProjectSectorsByParentId(int sectorId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectSector.Where(s => s.ParentId == sectorId && s.IsActive == true).ToList();
            }
        }

        public List<WFM_ProjectSector> GetProjectSectorList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectSector.Where(s => s.IsActive == true).ToList();
            }
        }

        public List<WFM_ProjectSector> GetProjectSectorParentList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectSector.Where(s => s.IsActive == true && s.ParentId == 0).ToList();
            }
        }

        public WFM_ProjectSector GetProjectSectorById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_ProjectSector.Where(s => s.IsActive == true && s.Id == id).SingleOrDefault();
            }
        }

        public void SaveOrUpdate(WFM_ProjectSector projectSector)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (projectSector.Id == 0)
                {
                    entities.WFM_ProjectSector.Add(projectSector);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(projectSector).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

    }
}
