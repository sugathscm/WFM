using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public static class CommonService
    {
        public static int SaveLoginAudit(LoginAudit loginAudit)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                entities.LoginAudits.Add(loginAudit);
                entities.SaveChanges();
            }
            return 1;
        }

        public static int SaveDataAudit(DataAudit dataAudit)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                entities.DataAudits.Add(dataAudit);
                entities.SaveChanges();
            }
            return 1;
        }


        public static List<DataAudit> GetDataAuditByUser(Guid userId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.DataAudits.Include("").Where(l => l.UserId == userId).OrderByDescending(o => o.UpdatedOn).ToList();
            }
        }

        public static List<LoginAudit> GetLoginAuditByUser(Guid userId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.LoginAudits.Where(l => l.UserId == userId).OrderByDescending(o => o.DateLogged).ToList();
            }
        }

        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                try
                {
                    propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                    value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                    propertyInfo.SetValue(x, value, null);
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                }
 
            }
            return (T)x;
        }

    }
}
