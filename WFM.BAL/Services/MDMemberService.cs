using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL.Services
{
    public class MDMemberService
    {
        public List<MD_Member> GetMemberList()
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.MD_Member.OrderBy(o => o.Name).ToList();
            }
        }

        public MD_Member GetMemberById(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.MD_Member.Where(o => o.Id == id).SingleOrDefault();
            }
        }

        public List<GetRelationshipMembersByMember_Result> GetRelationshipMembersByMember(int? id)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.GetRelationshipMembersByMember(id).ToList();
            }
        }

        public void SaveOrUpdate(MD_Member member)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (member.Id == 0)
                {
                    entities.MD_Member.Add(member);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(member).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }

        public void SaveOrUpdate(MD_MemberRelationship memberRelationship)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                if (memberRelationship.Id == 0)
                {
                    entities.MD_MemberRelationship.Add(memberRelationship);
                    entities.SaveChanges();
                }
                else
                {
                    entities.Entry(memberRelationship).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                }
            }
        }
    }
}
