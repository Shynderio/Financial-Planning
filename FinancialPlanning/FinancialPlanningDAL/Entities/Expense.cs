namespace FinancialPlanningDAL.Entities;

public class Expense
{
    public int No { get; set; }
    public DateTime Date { get; set; }
    public string Term { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string ExpenseName { get; set; } = string.Empty;
    public string CostType { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public double ExchangeRate { get; set; }
    public decimal TotalAmount { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public string PIC { get; set; } = string.Empty;
    public string? Note { get; set; } = string.Empty;
    public Status Status { get; set; }
}