using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WFM.BAL.Services;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class WinLossDashboardController : BaseController
    {
        private const int ProposalTypeId = 7;

        private static readonly string[] LossReasons = { "Price", "Technical score", "Experience", "Team", "Late/non-compliant", "Client relationship", "Lost to incumbent" };

        private readonly ProjectService projectService = new ProjectService();
        private readonly ProposalOutcomeService proposalOutcomeService = new ProposalOutcomeService();

        // GET: WinLossDashboard
        public ActionResult Index()
        {
            var projects = projectService.GetProjectList(null, ProposalTypeId, null, null, null, null);
            var outcomes = proposalOutcomeService.GetList();

            // Pair each proposal with its outcome (null Outcome / no record both mean "Pending")
            var rows = projects.Select(p =>
            {
                var outcome = outcomes.Where(o => o.ProjectId == p.Id).FirstOrDefault();
                string outcomeName = (outcome == null || string.IsNullOrEmpty(outcome.Outcome)) ? "Pending" : outcome.Outcome;
                string reason = (outcome == null) ? null : outcome.ReasonForLoss;
                decimal value = p.LKRValue ?? 0;
                return new { outcomeName, reason, value };
            }).ToList();

            int won = rows.Count(r => r.outcomeName == "Won");
            int lost = rows.Count(r => r.outcomeName == "Lost");
            int shortlisted = rows.Count(r => r.outcomeName == "Shortlisted");
            int pending = rows.Count(r => r.outcomeName == "Pending");
            int withdrawn = rows.Count(r => r.outcomeName == "Withdrawn");
            int decided = won + lost;

            ViewBag.Decided = decided;
            ViewBag.Won = won;
            ViewBag.Lost = lost;
            ViewBag.WinRate = (decided == 0) ? 0 : System.Math.Round((decimal)won / decided * 100, 1);

            ViewBag.ValueWon = rows.Where(r => r.outcomeName == "Won").Sum(r => r.value) / 1000m;
            ViewBag.ValueLost = rows.Where(r => r.outcomeName == "Lost").Sum(r => r.value) / 1000m;
            ViewBag.ValuePending = rows.Where(r => r.outcomeName == "Pending").Sum(r => r.value) / 1000m;
            ViewBag.ValueShortlisted = rows.Where(r => r.outcomeName == "Shortlisted").Sum(r => r.value) / 1000m;

            ViewBag.OutcomeWon = won;
            ViewBag.OutcomeLost = lost;
            ViewBag.OutcomeShortlisted = shortlisted;
            ViewBag.OutcomePending = pending;
            ViewBag.OutcomeWithdrawn = withdrawn;

            var lossReasonCounts = new List<int>();
            foreach (var reason in LossReasons)
            {
                lossReasonCounts.Add(rows.Count(r => r.outcomeName == "Lost" && r.reason == reason));
            }
            ViewBag.LossReasonLabels = LossReasons;
            ViewBag.LossReasonCounts = lossReasonCounts;

            return View();
        }
    }
}
