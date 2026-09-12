using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFM.DAL;

namespace WFM.UI.DF.Models
{
    public class MinistryViewModel : GetMinistries_Result
    {
        public List<GetMinistryLineAgencies_Result> LineAgencies { get; set; }    
    }
}