using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WFM.BAL.Helpers;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class ProjectService
    {
        EmailTemplateService emailTemplateService = new EmailTemplateService();

        public List<ProjectViewModel> GetProjects(int projectTypeId)
        {
            Dictionary<string, int> SESMarks = new Dictionary<string, int>(){
                {"Code", 5},
                {"StartDate", 5},
                {"ExpiaryDate", 5},
                {"DivisionId", 1},
                {"ProjectTypeId", 5},
                {"SectorId", 5},
                {"SubSectorId", 5},
                {"ShortDescription", 5},
                {"Requirement", 5},
                {"RDDocsAvailableId", 5},
                {"OrganizationId", 2},
                {"ContinentId", 2},
                {"PriorityId", 2},
                {"ProjectLocation", 2},
                {"FileStatusId", 2},
                {"FileCreatedDate", 2},
                {"LKRValue", 2},
                {"USDValue", 2},
                {"ContactId", 2},
                {"KeyContactPersonId1", 2},
                {"KeyContactPersonId2", 2},
                {"AssigneeId", 2},
                {"AvailabilityOfNDAId", 2},
                {"AvailabilityOfMandateId", 2},
                {"EvaluationScore", 2},
                {"Rating", 2},
                {"PriorityAccordingToRating", 2},
                {"SourceId", 2},
                {"DatePublished", 2},
                {"URL", 2},
                {"ProjectDivisionalStatusId", 2},
                {"HotPickId", 2},
                {"Comment", 2},
            };
            var proj = new WFM_Project();

            try
            {
                var projectList = new List<WFM_Project>();
                var projectViewList = new List<ProjectViewModel>();

                using (LinkManagementEntities entities = new LinkManagementEntities())
                {
                    if (projectTypeId == 0)
                    {
                        projectList = entities.WFM_Project
                            .Include("WFM_ProjectType")
                            .Include("WFM_Organization")
                            .Include("WFM_ProjectSector")
                            .Include("WFM_ProjectSector1")
                            .Include("WFM_ProjectStatus")
                            //.Include("WFM_DocumentTab")
                            .ToList();
                    }
                    else
                    {
                        projectList = entities.WFM_Project
                            .Include("WFM_ProjectType")
                            .Include("WFM_Organization")
                            .Include("WFM_ProjectSector")
                            .Include("WFM_ProjectSector1")
                            .Include("WFM_ProjectStatus")
                            //.Include("WFM_DocumentTab")
                            .Where(p => p.ProjectTypeId == projectTypeId).ToList();
                    }
                }

                foreach (var p in projectList)
                {
                    var SEStatus = StatusValues("SES", p, SESMarks);
                    var RDStatus = 0;// StatusValues("SES", p, SESMarks);
                    var MktStatus = 0;// StatusValues("SES", p, SESMarks);
                    var LegalStatus = 0;// StatusValues("SES", p, SESMarks);
                    proj = p;

                    projectViewList.Add(new ProjectViewModel
                    {
                        Number = (p.Number == null) ? 0 : p.Number.Value,
                        Id = p.Id,
                        ProjectTypeId = (p.ProjectTypeId == null) ? 0 : p.ProjectTypeId.Value,
                        Name = p.Name,
                        ProjectTypeName = (p.ProjectTypeId == null) ? "" : p.WFM_ProjectType.Name,
                        Code = p.Code,
                        SectorId = (p.SectorId == null) ? 0 : p.SectorId.Value,
                        SectorName = (p.SectorId == null) ? "" : p.WFM_ProjectSector.Name,
                        OrganizationId = (p.OrganizationId == null) ? 0 : p.OrganizationId.Value,
                        OrganizationName = (p.OrganizationId == null) ? "" : p.WFM_Organization.Name,
                        SubSectorId = (p.SubSectorId == null) ? 0 : p.SubSectorId.Value,
                        //SubSectorName = (p.SubSectorId == null) ? "" : p.WFM_ProjectSector1.Name,
                        SubSectorName = "",
                        ExpiaryDate = (p.ExpiaryDate == null) ? null : p.ExpiaryDate,
                        StartDate = (p.StartDate == null) ? null : p.StartDate,
                        StartDateString = (p.StartDate == null) ? null : p.StartDate.Value.ToString("dd-MMM-yyyy"),
                        ShortDescription = p.ShortDescription,
                        ////LongDescription = p.LongDescription,
                        StatusId = p.StatusId,
                        StatusName = (p.StatusId == null) ? null : p.WFM_ProjectStatus.Name,
                        ////LKRValue = p.LKRValue.Value.ToString(),
                        //IsActive = p.IsActive,
                        ////DateCreated = p.DateCreated.Value,
                        ////DaysDue = (p.ExpiaryDate.Value - DateTime.Now).Days,
                        ////DaysInCurrentTab = (DateTime.Now - p.CurrentDocumentTabDate.Value).Days,
                        ////CurrentDocumentTabDate = p.CurrentDocumentTabDate.Value,
                        ////CurrentDocumentTabId = p.CurrentDocumentTabId.Value,
                        ////CurrentDocumentTabName = p.WFM_DocumentTab.Name,
                        SEStatus = SEStatus,
                        RDStatus = 0,
                        MktStatus = 0,
                        LegalStatus = 0,
                        FinalStatus = SEStatus
                    });
                }

                return projectViewList.Where(o => o.ProjectTypeName != "").Where(o => o.ProjectTypeName != "<Select>").OrderBy(o => o.Number).ToList();
            }
            catch (Exception ex)
            {

                string error = proj.Id + " " + ex.ToString();
                throw;
            }
        }

        private int StatusValues(string type, WFM_Project project, Dictionary<string, int> SESMarks)
        {
            double percentage = 0;
            int status = 0;
            double total = 0;
            int full_total = 0;

            switch (type)
            {
                case "SES":
                    foreach (var mark in SESMarks)
                    {
                        full_total = full_total + mark.Value;

                        PropertyInfo pi = project.GetType().GetProperty(mark.Key);

                        try
                        {
                            var propertyValue = pi.GetValue(project, null);
                            if (propertyValue != null)
                            {
                                if (!string.IsNullOrEmpty(propertyValue.ToString()))
                                    total = total + mark.Value;
                            }
                        }
                        catch (Exception e)
                        {
                            string s = e.ToString();
                        }
                    }

                    percentage = (total / full_total) * 100;

                    break;
            }

            return (int)percentage;
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

        public void SaveOrUpdate(WFM_Project project)
        {
            bool isNew = true;

            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (project.Id == 0)
                {
                    project.CurrentDocumentTabId = 1;

                    entities.WFM_Project.Add(project);
                }
                else
                {
                    isNew = false;
                    entities.Entry(project).State = System.Data.Entity.EntityState.Modified;
                }
                entities.SaveChanges();

                //if (isNew)
                //{
                //    WFM_EmailTemplates wFM_EmailTemplate = emailTemplateService.GetEmailTemplateById(1);
                //    var body = wFM_EmailTemplate.Template.Replace("[CODE]", project.Code).Replace("[NAME]", project.Name).Replace("[SECTOR]", "");
                //    var message = new MailMessage
                //    {
                //        From = new MailAddress(wFM_EmailTemplate.FromEmail)
                //    };
                //    string[] ToEmails = wFM_EmailTemplate.ToEmails.Split(',');
                //    foreach(string email in ToEmails)
                //    {
                //        message.To.Add(new MailAddress(email));
                //    }                    
                //    message.Subject = wFM_EmailTemplate.Subject.Replace("[Project Code]", project.Code);
                //    message.Body = string.Format(body);
                //    EmailHelper.SendEmail(message);
                //}
            }
        }

        public List<GetDashboardData_Result> GetDashboardData()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.GetDashboardData().ToList();
            }
        }

        public int GetMaxNumber()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_Project.Max(p => p.Number).Value;
            }

        }
    }
}
