using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Entities
{
    [Table("department")]
    public class Department
    {
        [Key] public Guid Id { get; set; }
        [Required] public string DepartmentName { get; set; } = string.Empty;
        public virtual ICollection<User>? Users { get; set; }
        public virtual ICollection<Plan>? Plans { get; set; }
        public virtual ICollection<Report>? Reports { get; set; }
    }
}