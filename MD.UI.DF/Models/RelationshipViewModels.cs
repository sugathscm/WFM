using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WFM.DAL;

namespace WFM.UI.DF.Models
{

    public class RelationshipViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Person Name")]
        public int MemberId { get; set; }
        public string RelationshipText { get; set; }
        public string PersonName { get; set; }
        public string DateCreated { get; set; }
        public int RelatedMemberId { get; set; }
        public string JSONRelationshipList { get; set; }
        public List<GetPersonList_Result> PersonList { get; set; }
        public List<RelationshipDetailViewModel> RelationshipDetails { get; set; }
    }

    public class RelationshipDetailViewModel
    {
        public int Id { get; set; }
        public int PersonRelationshipId { get; set; }
        [Required]
        [Display(Name = "Relationship Type")]
        public int RelationshipTypeId { get; set; }
        [Required]
        [Display(Name = "Related Person Name")]
        public int RelatedPersonId { get; set; }
        public string PersonName { get; set; }
        public string RelationshipTypeName { get; set; }
        public List<GetPersonList_Result> RelatedPersonList { get; set; }
        public List<RelationshipType> RelationshipTypeList { get; set; }
    }

    public class JSONRelationshipDetailViewModel
    {
        public int RelPersonId { get; set; }
        public int RelTypeId { get; set; }
        public string RelPersonName { get; set; }
        public string RelTypeName { get; set; }
    }
}
