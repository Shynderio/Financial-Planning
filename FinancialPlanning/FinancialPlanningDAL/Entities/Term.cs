using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Entities
{
    [Table("Term")]
    internal class Term
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string TermName { get; set; }=string.Empty;
        [Required]
        public Guid CreatorId { get; set; }
      
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlanDueDate { get; set; }
        public DateTime ReportDueDate { get; set; }
        public int Status { get; set; }
        [ForeignKey("CreatorId")]
      
        public virtual User? User { get; set; }  
        public ICollection<Plan>? Plans { get; set;}
        public ICollection<Report>? Reports { get; set;}


    }
}
