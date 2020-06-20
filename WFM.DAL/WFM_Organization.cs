//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class WFM_Organization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WFM_Organization()
        {
            this.WFM_Project = new HashSet<WFM_Project>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public string Email { get; set; }
        public string LandLine { get; set; }
        public string FixedLine { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_Project> WFM_Project { get; set; }
    }
}
