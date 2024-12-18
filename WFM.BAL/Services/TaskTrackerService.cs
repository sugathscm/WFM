using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WFM.BAL.ViewModels;
using WFM.DAL;

namespace WFM.BAL.Services
{    
    public class TaskTrackerService
    {
        public TaskTrackerService() { }

        public List<GetTaskList_Result> GetTaskList(int? statusId, int? projectId, int? meetingId, int? taskTypeId, int? assigneeId, int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.GetTaskList(statusId, projectId, meetingId, taskTypeId, assigneeId, id).OrderBy(d => d.Id).ToList();
            }
        }

        public WFM_TaskTracker GetTaskById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskTracker.Where(s => s.Id == id).SingleOrDefault();
            }
        }

        public List<WFM_TaskTracker> GetTaskByProjectId(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskTracker.Where(s => s.ProjectId == id).ToList();
            }
        }

        public List<WFM_TaskTrackerAssignee> GetAssigneesByTaskId(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskTrackerAssignee.Where(s => s.TaskTrackerId == id).ToList();
            }
        }

        public List<WFM_TaskTrackerDocument> GetDocumentsByTaskId(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskTrackerDocument.Where(s => s.TaskId == id).ToList();
            }
        }

        public void SaveOrUpdate(WFM_TaskTracker task)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (task.Id == 0)
                {
                    entities.WFM_TaskTracker.Add(task);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(task).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
        public void SaveOrUpdate(List<WFM_TaskTracker> tasks)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (tasks != null)
                {
                    entities.WFM_TaskTracker.AddRange(tasks);
                    entities.SaveChanges();
                }
                //else
                //{
                //    entities.Entry(task).State = System.Data.Entity.EntityState.Modified;
                //    entities.SaveChanges();
                //}
            }
        }

        public void SaveOrUpdate(WFM_TaskTrackerDocument document)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                entities.WFM_TaskTrackerDocument.Add(document);
                entities.SaveChanges();
            }
        }

        public void RemoveAssignees(List<WFM_TaskTrackerAssignee> assignees)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                foreach(var assignee in assignees)
                {
                    entities.Entry(assignee).State = System.Data.Entity.EntityState.Deleted;
                    entities.WFM_TaskTrackerAssignee.Remove(assignee);
                }
                entities.SaveChanges();
            }
        }

        public void SaveOrUpdate(WFM_TaskTrackerAssignee assignee)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                entities.WFM_TaskTrackerAssignee.Add(assignee);
                entities.SaveChanges();
            }
        }

        public List<WFM_TaskStatus> GetTaskStatusList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.WFM_TaskStatus.Where(d => d.IsActive == true).OrderBy(d => d.Name).ToList();
            }
        }
    }
}
