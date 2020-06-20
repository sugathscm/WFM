using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFM.BAL.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Code { get; set; }
        public int SectorId { get; set; }
        public string SectorName { get; set; }
        public int SubSectorId { get; set; }
        public string SubSectorName { get; set; }
        public DateTime ExpiaryDate { get; set; }
        public DateTime StartDate { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public int DaysDue { get; set; }
        public int DaysInCurrentTab { get; set; }
        public int CurrentDocumentTabId { get; set; }
        public DateTime CurrentDocumentTabDate { get; set; }
        public string CurrentDocumentTabName { get; set; }
        public int Principal1Id { get; set; }
        public int Principal2Id { get; set; }
        public int Principal3Id { get; set; }
        public int Principal1Name { get; set; }
        public int Principal2Name { get; set; }
        public int Principal3Name { get; set; }
    }
}
