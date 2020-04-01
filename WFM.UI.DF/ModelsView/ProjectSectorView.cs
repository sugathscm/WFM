using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WFM.UI.ModelsView
{
    public class ProjectSectorView
    {
        public int Id { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}