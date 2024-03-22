using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WFM.DAL;

namespace WFM.UI.DF.ModelsView
{
    public class ProjectTenderViewModel : WFM_Project
    {
        public Form8BViewModel Form8BViewModel { get; set; }
        public Form12ViewModel Form12ViewModel { get; set; }
        public bool Form8BGenerated { get; set; }
        public bool Form12Generated { get; set; }
    }
}