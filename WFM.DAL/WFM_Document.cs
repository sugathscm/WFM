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
    
    public partial class WFM_Document
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WFM_Document()
        {
            this.WFM_DocumentField = new HashSet<WFM_DocumentField>();
            this.WFM_ProjectDocument = new HashSet<WFM_ProjectDocument>();
            this.WFM_ProjectDocumentHistory = new HashSet<WFM_ProjectDocumentHistory>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> HasFields { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> ProjectTypeId { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<int> DocumentTabId { get; set; }
    
        public virtual WFM_DocumentTab WFM_DocumentTab { get; set; }
        public virtual WFM_ProjectType WFM_ProjectType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_DocumentField> WFM_DocumentField { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_ProjectDocument> WFM_ProjectDocument { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WFM_ProjectDocumentHistory> WFM_ProjectDocumentHistory { get; set; }
    }
}
