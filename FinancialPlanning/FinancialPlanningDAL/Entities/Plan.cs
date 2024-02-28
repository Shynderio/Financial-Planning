using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Entities
{
    [Table("Plan")]
    internal class Plan
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string PlanName { get; set; } = string.Empty;

        public int Status { get; set; }

        [ForeignKey("Term")]
        public Guid TermId { get; set; }
        public Term? Term { get; set; }

        [ForeignKey("Department")]
        public Guid DepartmentId { get; set; }
        public Department? Department { get; set; }
        public virtual ICollection<PlanVersion>? PlanVersions { get; set; }


    }
}
