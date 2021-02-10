using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFM.DAL;

namespace WFM.BAL.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public int Number { get; set; }

        [Required(ErrorMessage = "Please enter project type.")]
        public int ProjectTypeId { get; set; }
        [Required(ErrorMessage = "Please enter code prefix.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please enter organization.")]
        public Nullable<int> OrganizationId { get; set; }
        [Required(ErrorMessage = "Please enter sector.")]
        public Nullable<int> SectorId { get; set; }

        public Nullable<int> SubSectorId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ExpiaryDate { get; set; }
        [Required(ErrorMessage = "Please enter start date."), DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> StartDate { get; set; }
        public string StartDateString { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        [Required(ErrorMessage = "Please enter division.")]
        public Nullable<int> DivisionId { get; set; }
        [Required(ErrorMessage = "Please enter status.")]
        public Nullable<int> StatusId { get; set; }
        public Nullable<decimal> LKRValue { get; set; }
        public Nullable<decimal> USDValue { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> CurrentDocumentTabId { get; set; }
        public Nullable<System.DateTime> CurrentDocumentTabDate { get; set; }
        public Nullable<int> BondTypeId { get; set; }
        public string PresentedByOwner { get; set; }
        public Nullable<int> ContactId { get; set; }
        public string RationalForNo8 { get; set; }
        public string ProjectOwnerExpectation { get; set; }
        public string IntroducerExpectation { get; set; }
        public string VerificationOfInfo { get; set; }
        public Nullable<bool> IsMandateAvailable { get; set; }
        public string ProjectChampion { get; set; }
        public Nullable<int> PrincipalId1 { get; set; }
        public Nullable<int> PrincipalId2 { get; set; }
        public Nullable<int> PrincipalId3 { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        [Required(ErrorMessage = "Please enter name.")]
        public string Name { get; set; }
        public Nullable<int> MethodOfIntroductionId { get; set; }
        public Nullable<int> PriorityFrameworkId { get; set; }
        public Nullable<bool> IsRationalForNo8 { get; set; }
        public Nullable<int> KeyContactPersonId1 { get; set; }
        public Nullable<int> KeyContactPersonId2 { get; set; }
        public Nullable<int> KeyContactPersonId3 { get; set; }
        public Nullable<bool> IsPresentedByOwner { get; set; }
        public Nullable<bool> IsProposedMethodOfEngagement { get; set; }
        public Nullable<int> TenderTypeId { get; set; }
        public string TenderRegistrationPreQualification { get; set; }
        public string TenderOthers { get; set; }
        public Nullable<int> TypeOfSaleId { get; set; }
        public string Requirement { get; set; }
        public Nullable<int> RDDocsAvailableId { get; set; }
        public string TaskId { get; set; }
        public string List25 { get; set; }
        public Nullable<int> ApprovedToGoAheadId { get; set; }
        public Nullable<int> ContinentId { get; set; }
        public string ProjectLocation { get; set; }
        public Nullable<int> PriorityId { get; set; }
        public Nullable<int> FileStatusId { get; set; }
        public Nullable<System.DateTime> FileCreatedDate { get; set; }
        public Nullable<int> SLICCopyId { get; set; }
        public string OldFileId { get; set; }
        public Nullable<int> AssigneeId { get; set; }
        public Nullable<int> AvailabilityOfNDAId { get; set; }
        public Nullable<int> AvailabilityOfMandateId { get; set; }
        public string EvaluationScore { get; set; }
        public string Rating { get; set; }
        public string PriorityAccordingToRating { get; set; }
        public string URL { get; set; }
        public string Comment { get; set; }
        public Nullable<int> SourceId { get; set; }
        public Nullable<System.DateTime> DatePublished { get; set; }
        public Nullable<int> ProjectDivisionalStatusId { get; set; }
        public Nullable<int> HotPickId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
        public virtual WFM_Contact WFM_Contact { get; set; }
        public virtual WFM_Contact WFM_Contact1 { get; set; }
        public virtual WFM_Contact WFM_Contact2 { get; set; }
        public virtual WFM_Contact WFM_Contact3 { get; set; }
        public virtual WFM_Division WFM_Division { get; set; }
        public virtual WFM_MethodOfIntroduction WFM_MethodOfIntroduction { get; set; }
        public virtual WFM_Organization WFM_Organization { get; set; }
        public virtual WFM_PriorityFramework WFM_PriorityFramework { get; set; }
        public virtual WFM_ProjectSector WFM_ProjectSector { get; set; }
        public virtual WFM_ProjectStatus WFM_ProjectStatus { get; set; }
        public virtual WFM_ProjectSector WFM_ProjectSector1 { get; set; }
        
        public string ProjectTypeName { get; set; }
        public string OrganizationName { get; set; }
        public string SectorName { get; set; }
        public string SubSectorName { get; set; }
        public string StatusName { get; set; }
        public int DaysDue { get; set; }
        public int DaysInCurrentTab { get; set; }
        public string CurrentDocumentTabName { get; set; }
        public int Principal1Id { get; set; }
        public int Principal2Id { get; set; }
        public int Principal3Id { get; set; }
        public int Principal1Name { get; set; }
        public int Principal2Name { get; set; }
        public int Principal3Name { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_ProjectDocument> WFM_ProjectDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_ProjectDocumentFieldValue> WFM_ProjectDocumentFieldValue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_ProjectDocumentHistory> WFM_ProjectDocumentHistory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_ProjectDocumentTab> WFM_ProjectDocumentTab { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_Marketing> WFM_Marketing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_MarketingSourcingPartner> WFM_MarketingSourcingPartner { get; set; }

        public int SEStatus { get; set; }
        public int RDStatus { get; set; }
        public int MktStatus { get; set; }
        public int LegalStatus { get; set; }
        public int FinalStatus { get; set; }

    }
}
