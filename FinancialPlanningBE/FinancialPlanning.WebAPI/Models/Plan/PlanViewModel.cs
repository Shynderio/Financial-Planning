namespace FinancialPlanning.WebAPI.Models.Plan
{
    public class PlanViewModel
    {
        public int No { get; set; }
        public string Plan { get; set; } = String.Empty; 
        public string Term { get; set; } = String.Empty;
        public string Department { get; set; } = String.Empty;
        public string Status { get; set; } = String.Empty;
        public int Version { get; set; }
    }
}
