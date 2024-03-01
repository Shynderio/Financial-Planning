namespace FinancialPlanningBAL.DTOs
{
    public class TermDTO
    {
        public Guid Id { get; set; }
        public string TermName { get; set; } = string.Empty;
        public Guid CreatorId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlanDueDate { get; set; }
        public DateTime ReportDueDate { get; set; }
        public int Status { get; set; }
        // Additional properties if needed
    }
}
