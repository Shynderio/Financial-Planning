using System;
using System.ComponentModel.DataAnnotations;

namespace FinancialPlanningAPI.Models
{
    public class TermViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Term name is required")]
        public string TermName { get; set; }

        [Required(ErrorMessage = "Creator ID is required")]
        public Guid CreatorId { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [RegularExpression(@"^[1,3,6]$", ErrorMessage = "Duration must be set to 1, 3, or 6")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Plan due date is required")]
        public DateTime PlanDueDate { get; set; }

        [Required(ErrorMessage = "Report due date is required")]
        public DateTime ReportDueDate { get; set; }

        // [Required(ErrorMessage = "Status is required")]
        // [Range(1, 3, ErrorMessage = "Status must be set to 1, 2, or 3")]
        public int Status { get; }
        public TermViewModel(Guid id, string termName, Guid creatorId, int duration, DateTime startDate, DateTime planDueDate, DateTime reportDueDate, int status)
        {
            Id = id;
            TermName = termName;
            CreatorId = creatorId;
            Duration = duration;
            StartDate = startDate;
            PlanDueDate = planDueDate;
            ReportDueDate = reportDueDate;
            Status = status;
        }
    }
}
