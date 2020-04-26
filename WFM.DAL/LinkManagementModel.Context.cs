﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WFM.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LinkManagementEntities : DbContext
    {
        public LinkManagementEntities()
            : base("name=LinkManagementEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DataAudit> DataAudits { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<LoginAudit> LoginAudits { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationType> OrganizationTypes { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PersonHistory> PersonHistories { get; set; }
        public virtual DbSet<PersonRelationship> PersonRelationships { get; set; }
        public virtual DbSet<PersonRelationshipDetail> PersonRelationshipDetails { get; set; }
        public virtual DbSet<Principal> Principals { get; set; }
        public virtual DbSet<PrincipalContact> PrincipalContacts { get; set; }
        public virtual DbSet<ProjectSector> ProjectSectors { get; set; }
        public virtual DbSet<RelationshipType> RelationshipTypes { get; set; }
        public virtual DbSet<SourcingPartner> SourcingPartners { get; set; }
        public virtual DbSet<SubOrganization> SubOrganizations { get; set; }
        public virtual DbSet<TenderDocumentSection> TenderDocumentSections { get; set; }
        public virtual DbSet<TenderDocumentType> TenderDocumentTypes { get; set; }
        public virtual DbSet<Title> Titles { get; set; }
        public virtual DbSet<WFM_Configuration> WFM_Configuration { get; set; }
        public virtual DbSet<WFM_Designation> WFM_Designation { get; set; }
    
        public virtual ObjectResult<GetDataAuditByUser_Result> GetDataAuditByUser(Nullable<System.Guid> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDataAuditByUser_Result>("GetDataAuditByUser", userIdParameter);
        }
    
        public virtual ObjectResult<GetLoginAuditByUser_Result> GetLoginAuditByUser(Nullable<System.Guid> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetLoginAuditByUser_Result>("GetLoginAuditByUser", userIdParameter);
        }
    
        public virtual ObjectResult<GetOrganizationList_Result> GetOrganizationList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetOrganizationList_Result>("GetOrganizationList");
        }
    
        public virtual ObjectResult<GetPersonHistoryList_Result> GetPersonHistoryList(Nullable<int> personId)
        {
            var personIdParameter = personId.HasValue ?
                new ObjectParameter("PersonId", personId) :
                new ObjectParameter("PersonId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPersonHistoryList_Result>("GetPersonHistoryList", personIdParameter);
        }
    
        public virtual ObjectResult<GetPersonList_Result> GetPersonList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPersonList_Result>("GetPersonList");
        }
    
        public virtual ObjectResult<GetRelationshipListReport_Result> GetRelationshipListReport(Nullable<int> personId)
        {
            var personIdParameter = personId.HasValue ?
                new ObjectParameter("PersonId", personId) :
                new ObjectParameter("PersonId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetRelationshipListReport_Result>("GetRelationshipListReport", personIdParameter);
        }
    
        public virtual int ImportCSV()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ImportCSV");
        }
    
        public virtual int SetRelationshipText()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SetRelationshipText");
        }
    }
}
