using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WFM.BAL.Services;

namespace WFM.UI.DF.Controllers
{
    [Authorize]
    public class ProposalAnalysisController : BaseController
    {
        private const int ProposalTypeId = 7;

        private readonly ProjectService projectService = new ProjectService();
        private readonly ProposalOutcomeService proposalOutcomeService = new ProposalOutcomeService();
        private readonly ConfigurationService configurationService = new ConfigurationService();

        private class ValueBand
        {
            public string Label;
            public decimal MinThousands;
            public decimal MaxThousands;
        }

        public class SectorWinRateRow
        {
            public string Sector { get; set; }
            public int Won { get; set; }
            public int Lost { get; set; }
            public decimal? WinRate { get; set; }
        }

        public class ValueBandWinRateRow
        {
            public string Band { get; set; }
            public int Won { get; set; }
            public int Lost { get; set; }
            public decimal? WinRate { get; set; }
        }

        private static readonly ValueBand[] ValueBands = new[]
        {
            new ValueBand { Label = "< 10,000", MinThousands = decimal.MinValue, MaxThousands = 10000 },
            new ValueBand { Label = "10,000-25,000", MinThousands = 10000, MaxThousands = 25000 },
            new ValueBand { Label = "25,000-50,000", MinThousands = 25000, MaxThousands = 50000 },
            new ValueBand { Label = "50,000-100,000", MinThousands = 50000, MaxThousands = 100000 },
            new ValueBand { Label = "> 100,000", MinThousands = 100000, MaxThousands = decimal.MaxValue }
        };

        // GET: ProposalAnalysis
        public ActionResult Index()
        {
            int alertLeadTimeDays = int.Parse(configurationService.GetValue("AlertLeadTimeDays", "3"));
            int amberWarningWindowDays = int.Parse(configurationService.GetValue("AmberWarningWindowDays", "5"));

            var projects = projectService.GetProjectList(null, ProposalTypeId, null, null, null, null);
            var outcomes = proposalOutcomeService.GetList();
            var today = DateTime.Today;

            var rows = projects.Select(p =>
            {
                var outcome = outcomes.Where(o => o.ProjectId == p.Id).FirstOrDefault();
                string outcomeName = (outcome == null || string.IsNullOrEmpty(outcome.Outcome)) ? "Pending" : outcome.Outcome;

                string delayStatus = "";
                if (p.ExpiaryDate != null)
                {
                    int daysLeft = (p.ExpiaryDate.Value.Date - today).Days;
                    if (daysLeft < 0)
                        delayStatus = "OVERDUE";
                    else if (daysLeft <= alertLeadTimeDays)
                        delayStatus = "CRITICAL (<=" + alertLeadTimeDays + "d)";
                    else if (daysLeft <= amberWarningWindowDays)
                        delayStatus = "AT RISK";
                    else
                        delayStatus = "ON TRACK";
                }

                // SectorName from the stored proc embeds "<br />" + assignee for two-line grid display elsewhere; strip it for plain-text grouping here.
                string sectorName = string.IsNullOrEmpty(p.SectorName) ? "(none)" : p.SectorName.Split(new[] { "<br" }, StringSplitOptions.None)[0].Trim();

                return new
                {
                    sector = string.IsNullOrEmpty(sectorName) ? "(none)" : sectorName,
                    valueThousands = (p.LKRValue ?? 0) / 1000m,
                    outcomeName,
                    delayStatus
                };
            }).ToList();

            // Win rate by sector
            var sectors = rows.Select(r => r.sector).Distinct().OrderBy(s => s).ToList();
            var winRateBySector = sectors.Select(s =>
            {
                int won = rows.Count(r => r.sector == s && r.outcomeName == "Won");
                int lost = rows.Count(r => r.sector == s && r.outcomeName == "Lost");
                int decided = won + lost;
                decimal? winRate = (decided == 0) ? (decimal?)null : System.Math.Round((decimal)won / decided * 100, 1);
                return new SectorWinRateRow { Sector = s, Won = won, Lost = lost, WinRate = winRate };
            }).ToList();

            // Win rate by value band
            var winRateByBand = ValueBands.Select(b =>
            {
                var inBand = rows.Where(r => r.valueThousands >= b.MinThousands && r.valueThousands < b.MaxThousands).ToList();
                int won = inBand.Count(r => r.outcomeName == "Won");
                int lost = inBand.Count(r => r.outcomeName == "Lost");
                int decided = won + lost;
                decimal? winRate = (decided == 0) ? (decimal?)null : System.Math.Round((decimal)won / decided * 100, 1);
                return new ValueBandWinRateRow { Band = b.Label, Won = won, Lost = lost, WinRate = winRate };
            }).ToList();

            // Delay analysis (live pipeline) - only undecided/active proposals
            string[] delayLabels = { "ON TRACK", "AT RISK", "BEHIND PLAN", "CRITICAL (<=" + alertLeadTimeDays + "d)", "OVERDUE" };
            var delayCounts = delayLabels.Select(label =>
                rows.Count(r => r.delayStatus == label && (r.outcomeName == "Pending" || r.outcomeName == "Shortlisted"))
            ).ToList();

            ViewBag.WinRateBySector = winRateBySector;
            ViewBag.WinRateByBand = winRateByBand;
            ViewBag.DelayLabels = delayLabels;
            ViewBag.DelayCounts = delayCounts;

            ViewBag.SectorLabels = winRateBySector.Select(r => r.Sector).ToList();
            ViewBag.SectorWinRates = winRateBySector.Select(r => r.WinRate ?? 0).ToList();

            return View();
        }
    }
}
