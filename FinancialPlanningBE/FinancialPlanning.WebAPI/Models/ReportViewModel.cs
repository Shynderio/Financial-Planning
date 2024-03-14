﻿using FinancialPlanning.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinancialPlanning.WebAPI.Models
{
    public class ReportViewModel
    {
        public Guid Id { get; set; }
        [Required] public string ReportName { get; set; } = string.Empty;
        [Required] public int Month { get; set; }
        [Required] public int Status { get; set; }
        [Required] public string TermName { get; set; } = null!;
        [Required] public string DepartmentName { get; set; } = null!;
        [Required] public string Version { get; set; } = null!;
        
    }
}