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
        StandAlone,
        Unsolicited,
        PPP,
        [Description("JV with the State")]
        JVWithTheState,
        BOT,
        BOOT,
        BOO,
        Supply,
        EPCC,
        [Description("Turn-key")]
        Turnkey,
        Procurement,
        Consultancy,
        Investment,
        EOI
    }
}
