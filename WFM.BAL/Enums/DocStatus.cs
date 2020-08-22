using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFM.BAL.Enums
{
    public enum DocStatus
    {
        Yes = 1,
        No = 2,
        [Description("Not Applicable")]
        NotApplicable = 3
    }

    public enum Continent
    {
        Foreign = 1,
        Local = 2
    }

    public enum Priority
    {
        High = 1,
        Medium = 2,
        Low = 3,
        [Description("Not Applicable")]
        NA = 4

    }
}
