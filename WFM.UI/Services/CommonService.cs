using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFM.UI.DAL;
using WFM.UI.Models;

namespace WFM.UI.Services
{
    public static class CommonService
    {
        public static int SaveLoginAudit(LoginAudit loginAudit)
        {
            using (WFMContext entities = new WFMContext())
            {
                entities.LoginAudits.Add(loginAudit);
                entities.SaveChanges();
            }
            return 1;
        }

        public static int SaveDataAudit(DataAudit dataAudit)
        {
            using (WFMContext entities = new WFMContext())
            {
                entities.DataAudits.Add(dataAudit);
                entities.SaveChanges();
            }
            return 1;
        }

        //public static List<GetDataAuditByUser_Result> GetDataAuditByUser(Guid userId)
        //{
        //    using (WFMContext entities = new WFMContext())
        //    {
        //        return entities.GetDataAuditByUser(userId).OrderByDescending(o => o.UpdatedOn).ToList();
        //    }
        //}

        //public static List<GetLoginAuditByUser_Result> GetLoginAuditByUser(Guid userId)
        //{
        //    using (WFMContext entities = new WFMContext())
        //    {
        //        return entities.GetLoginAuditByUser(userId).OrderByDescending(o => o.DateLogged).ToList();
        //    }
        //}

    }
}