using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ProjectService
    {
        public List<ProjectViewModel> GetProjects(int projectTypeId)
        {
            try
            {
                var projectList = new List<WFM_Project>();
                using (LinkManagementEntities entities = new LinkManagementEntities())
                {
                    if (projectTypeId == 0)
                    {
                        projectList = entities.WFM_Project
                            .Include("WFM_ProjectType")
                            .Include("WFM_ProjectType")
                            .Include("WFM_Organization")
                            .Include("WFM_ProjectSector")
                            .Include("WFM_ProjectSector1")
                            .Include("WFM_ProjectStatus")
                            .Include("WFM_DocumentTab")
                            .ToList();
                    }
                    else
                    {
                        projectList = entities.WFM_Project
                            .Include("WFM_ProjectType")
                            .Include("WFM_ProjectType")
                            .Include("WFM_Organization")
                            .Include("WFM_ProjectSector")
                            .Include("WFM_ProjectSector1")
                            .Include("WFM_ProjectStatus")
                            .Include("WFM_DocumentTab")
                            .Where(p => p.ProjectTypeId == projectTypeId).ToList();
                    }
                }

                var projects = projectList.Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    ProjectTypeId = p.ProjectTypeId,
                    ProjectTypeName = p.WFM_ProjectType.Name,
                    Code = p.Code,
                    SectorId = p.SectorId.Value,
                    SectorName = p.WFM_ProjectSector.Name,
                    OrganizationId = p.OrganizationId.Value,
                    OrganizationName = p.WFM_Organization.Name,
                    SubSectorId = p.SubSectorId.Value,
                    SubSectorName = p.WFM_ProjectSector1.Name,
                    ExpiaryDate = p.ExpiaryDate.Value,
                    StartDate = p.StartDate.Value,
                    ShortDescription = p.ShortDescription,
                    LongDescription = p.LongDescription,
                    StatusId = p.StatusId.Value,
                    StatusName = p.WFM_ProjectStatus.Name,
                    Value = p.Value.Value.ToString(),
                    IsActive = p.IsActive,
                    DateCreated = p.DateCreated.Value,
                    DaysDue = (p.ExpiaryDate.Value - DateTime.Now).Days,
                    DaysInCurrentTab = (DateTime.Now - p.CurrentDocumentTabDate.Value).Days,
                    CurrentDocumentTabDate = p.CurrentDocumentTabDate.Value,
                    CurrentDocumentTabId = p.CurrentDocumentTabId.Value,
                    CurrentDocumentTabName = p.WFM_DocumentTab.Name,
                });

                return projects.Where(p => p.IsActive == true).ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public WFM_Project GetProjectById(int projectTypeId, int id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (projectTypeId == 0)
                    return entities.WFM_Project
                        .Include("WFM_ProjectType")
                        .Include("WFM_ProjectType")
                        .Include("WFM_Organization")
                        .Include("WFM_ProjectSector")
                        .Include("WFM_ProjectSector1")
                        .Include("WFM_ProjectStatus")
                        .Include("WFM_DocumentTab")
                        .Where(p => p.Id == id)
                        .SingleOrDefault();

                else
                    return entities.WFM_Project
                        .Include("WFM_ProjectType")
                        .Include("WFM_ProjectType")
                        .Include("WFM_Organization")
                        .Include("WFM_ProjectSector")
                        .Include("WFM_ProjectSector1")
                        .Include("WFM_ProjectStatus")
                        .Include("WFM_DocumentTab")
                        .Where(p => p.ProjectTypeId == projectTypeId && p.Id == id)
                        .SingleOrDefault();
            }
        }
    }
}
