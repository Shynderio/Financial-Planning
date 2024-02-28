using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Entities
{
    [Table("PlanVersion")]
    internal class PlanVersion
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PlanId { get; set; }
        [Required]
        public int Version { get; set; }
        [Required]
        public Guid CreatorId { get; set; }

        public DateTime ImportDate { get; set; }
        [ForeignKey("PlanId")]
        public virtual Plan? Plan { get; set; }
        [ForeignKey("CreatorId")]
        public virtual User? User { get; set; }

    }
}
