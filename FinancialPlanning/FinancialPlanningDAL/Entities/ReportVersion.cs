using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Entities
{
    [Table("ReportVersion")]
    internal class ReportVersion
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public int Version {  get; set; }   
        public DateTime ImportDate { get; set; }   
        public Guid CreatorId { get; set; }

        [ForeignKey("ReportId")]     
        public virtual Report? Report { get; set; }
        [ForeignKey("CreatorId")]
        public virtual User? User { get; set; }
    }
}
