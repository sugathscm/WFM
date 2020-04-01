﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL
{
    public static class CommonService
    {
        public static int SaveLoginAudit(LoginAudit loginAudit)
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                entities.LoginAudits.Add(loginAudit);
                entities.SaveChanges();
            }
            return 1;
        }

        public static int SaveDataAudit(DataAudit dataAudit)
        {
            using (WorkFlowEntities entities = new WorkFlowEntities())
            {
                entities.DataAudits.Add(dataAudit);
                entities.SaveChanges();
            }
            return 1;
        }

        //public static List<GetDataAuditByUser_Result> GetDataAuditByUser(Guid userId)
        //{
        //    using (WorkFlowEntities entities = new WorkFlowEntities())
        //    {
        //        return entities.GetDataAuditByUser(userId).OrderByDescending(o => o.UpdatedOn).ToList();
        //    }
        //}

        //public static List<GetLoginAuditByUser_Result> GetLoginAuditByUser(Guid userId)
        //{
        //    using (WorkFlowEntities entities = new WorkFlowEntities())
        //    {
        //        return entities.GetLoginAuditByUser(userId).OrderByDescending(o => o.DateLogged).ToList();
        //    }
        //}

    }
}
