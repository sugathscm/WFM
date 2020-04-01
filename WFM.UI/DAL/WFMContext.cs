using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WFM.UI.Models;

namespace WFM.UI.DAL
{
    public class WFMContext : DbContext
    {

        public WFMContext() : base("WFMContext")
        {
            Database.SetInitializer<WFMContext>(new DropCreateDatabaseIfModelChanges<WFMContext>()); //Drop database if changes detected
        }

        public DbSet<TenderDocumentType> TenderDocumentTypes { get; set; }
        public DbSet<ProjectSector> ProjectSectors { get; set; }
        public DbSet<TenderDocumentSection> TenderDocumentSections { get; set; }
        public DbSet<Principal> Principals { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<PrincipalContact> PrincipalContacts { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<SourcingPartner> SourcingPartners { get; set; }
        public DbSet<WFMConfiguration> WFMConfigurations { get; set; }
        public DbSet<DataAudit> DataAudits { get; set; }
        public DbSet<LoginAudit> LoginAudits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}