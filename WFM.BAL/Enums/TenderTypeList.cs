using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFM.BAL.Enums
{
    public enum TenderTypeList
    {
        [Description("Stand-alone")]
        StandAlone = 1,
        Unsolicited = 2,
        PPP = 3,
        [Description("JV with the State")]
        JVWithTheState = 4,
        BOT = 5,
        BOOT = 6,
        BOO = 7,
        Supply = 8,
        EPCC = 9,
        [Description("Turn-key")]
        Turnkey = 10,
        Procurement = 11,
        Consultancy = 12,
        Investment = 13,
        EOI = 14
    }
}
