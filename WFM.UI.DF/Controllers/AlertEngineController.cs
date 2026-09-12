using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Web.Mvc;
using WFM.BAL.Services;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class AlertEngineController : BaseController
    {
        private const int ProposalTypeId = 7;

        private readonly ProjectService projectService = new ProjectService();
        private readonly EmployeeService employeeService = new EmployeeService();
        private readonly ConfigurationService configurationService = new ConfigurationService();

        // GET: AlertEngine
        public ActionResult Index()
        {
            ViewBag.AlertWindowDays = int.Parse(configurationService.GetValue("AlertLeadTimeDays", "3"));
            return View();
        }

        public ActionResult GetList()
        {
            int alertWindowDays = int.Parse(configurationService.GetValue("AlertLeadTimeDays", "3"));

            var list = projectService.GetProjectList(null, ProposalTypeId, null, null, null, null);
            var employeeList = employeeService.GetEmployeeList();
            var today = DateTime.Today;

            var alerts = list
                .Where(p => p.ExpiaryDate != null && (p.ExpiaryDate.Value.Date - today).Days <= alertWindowDays)
                .Select(p =>
                {
                    int daysLeft = (p.ExpiaryDate.Value.Date - today).Days;
                    bool overdue = daysLeft < 0;
                    var owner = (p.AssigneeId == null) ? null : employeeList.Where(e => e.Id == p.AssigneeId).FirstOrDefault();

                    string subject = "[PROPOSAL ALERT] " + p.Name + " due " + p.ExpiaryDate.Value.ToString("dd-MMM");
                    string body = "Dear " + ((owner == null) ? "" : owner.Name) + ", the proposal '" + p.Name
                        + "' is due in " + daysLeft + " day(s) (deadline " + p.ExpiaryDate.Value.ToString("dd-MMM-yyyy") + "). "
                        + "Internal deadline is 24h earlier. Please confirm gate status and submission readiness. - EML PQMS Monitor";

                    return new
                    {
                        p.Id,
                        p.ProjectCode,
                        ProjectName = p.Name,
                        OwnerName = (owner == null) ? "" : owner.Name,
                        OwnerEmail = (owner == null) ? "" : owner.Email,
                        Deadline = p.ExpiaryDate.Value.ToString("yyyy-MM-dd"),
                        DaysLeft = daysLeft,
                        Overdue = overdue,
                        Subject = subject,
                        Body = body
                    };
                })
                .OrderBy(a => a.DaysLeft)
                .ToList();

            JsonResult jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult = Json(new { data = alerts }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendAlert(int id)
        {
            try
            {
                var project = projectService.GetProjectById(ProposalTypeId, id);
                if (project == null || project.ExpiaryDate == null)
                    return Json(new { Status = 0, Message = "Proposal not found or has no deadline." });

                var owner = (project.AssigneeId == null) ? null : employeeService.GetEmployeeById(project.AssigneeId);
                if (owner == null || string.IsNullOrEmpty(owner.Email))
                    return Json(new { Status = 0, Message = "No owner email on file for this proposal." });

                int daysLeft = (project.ExpiaryDate.Value.Date - DateTime.Today).Days;

                string subject = "[PROPOSAL ALERT] " + project.Name + " due " + project.ExpiaryDate.Value.ToString("dd-MMM");
                string body = "Dear " + owner.Name + ", the proposal '" + project.Name
                    + "' is due in " + daysLeft + " day(s) (deadline " + project.ExpiaryDate.Value.ToString("dd-MMM-yyyy") + "). "
                    + "Internal deadline is 24h earlier. Please confirm gate status and submission readiness. - EML PQMS Monitor";

                SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                CommonService.SendEmail(smtpSection, owner.Email, subject, body);

                return Json(new { Status = 1, Message = "Alert sent to " + owner.Email });
            }
            catch (Exception ex)
            {
                return Json(new { Status = 0, Message = ex.Message });
            }
        }
    }
}
