using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WFM.UI.Models;

namespace WFM.UI.DAL
{
    public class WFMInitializer : DropCreateDatabaseIfModelChanges<WFMContext>
    {
        public override void InitializeDatabase(WFMContext context)
        {
            //context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction
            //    , string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));

            base.InitializeDatabase(context);
        }

        protected override void Seed(WFMContext context)
        {


        }
    }
}