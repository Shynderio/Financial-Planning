using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Entities
{
    [Table("report")]
    public class Report
    {
        [Key] public Guid Id { get; set; }
        [Required] public string ReportName { get; set; } = string.Empty;
        [Required] public string Month { get; set; } = string.Empty;
        [Required] public int Status { get; set; }
        [Required] public DateTime UpdateDate { get; set; }
        [ForeignKey("Term")] public Guid TermId { get; set; }
        public virtual Term Term { get; set; } = null!;
        [ForeignKey("Department")] public Guid DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<ReportVersion>? ReportVersions { get; set; }

        public int? GetMaxVersion()
        {
            if (this.ReportVersions == null) return 1;
            else return this.ReportVersions.OrderByDescending(rv => rv.Version).FirstOrDefault()?.Version;

        }
     
    }
}