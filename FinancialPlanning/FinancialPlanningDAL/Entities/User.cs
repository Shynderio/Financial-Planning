using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanningDAL.Entities
{
    [Table("User")]
    internal class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string DOB {get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        public Guid PositionId { get; set; }    
        public Guid RoleId { get; set; }    

        public int Status { get; set; }
        public string Notes { get; set; }=string.Empty;
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position? Position { get; set; }
        public virtual ICollection<Term>? Terms { get; set; }
        public virtual ICollection<PlanVersion>? PlanVersions { get; set; }
        public virtual ICollection<ReportVersion>? ReportVersions { get; set; }
    }
}
