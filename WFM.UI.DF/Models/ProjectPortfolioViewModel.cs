using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFM.UI.DF.Models
{
    public class ProjectPortfolioViewModel
    {
        public int Delayed { get; set; }
        public int NeedsAttention { get; set; }
        public int OnTrack { get; set; }

        public List<ProjectCategoryViewModel> Categories { get; set; }
    }

    public class ProjectCategoryViewModel
    {
        public string Name { get; set; }

        public int Delayed { get; set; }
        public int Attention { get; set; }
        public int OnTrack { get; set; }

        public int Total
        {
            get { return Delayed + Attention + OnTrack; }
        }
    }
}