﻿using FinancialPlanning.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialPlanning.Data.Entities
{
    [Table("plan")]
    public class Plan
    {
        [Key] public Guid Id { get; set; }
        [Required] public string PlanName { get; set; } = string.Empty;
        [Required] public PlanStatus Status { get; set; }
        [ForeignKey("Term")] public Guid TermId { get; set; }
        public virtual Term Term { get; set; } = null!;
        [ForeignKey("Department")] public Guid DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<PlanVersion> PlanVersions { get; set; } = null!;
        public int? GetMaxVersion()
        {
            if (PlanVersions == null) return 1;
            return PlanVersions.MaxBy(rv => rv.Version)?.Version;

        }
    }
}