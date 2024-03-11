namespace FinancialPlanning.Data.Entities;

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
    public string? Currency { get; set; }
    public double? ExchangeRate { get; set; }
    public decimal TotalAmount { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public string PIC { get; set; } = string.Empty;
    public string? Note { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Expense)
        {
            return false;
        }

        Expense other = (Expense)obj;
        return No == other.No &&
               Date == other.Date &&
               Term == other.Term &&
               Department == other.Department &&
               ExpenseName == other.ExpenseName &&
               CostType == other.CostType &&
               UnitPrice == other.UnitPrice &&
               Amount == other.Amount &&
               Currency == other.Currency &&
               Nullable.Equals(ExchangeRate, other.ExchangeRate) &&
               TotalAmount == other.TotalAmount &&
               ProjectName == other.ProjectName &&
               SupplierName == other.SupplierName &&
               PIC == other.PIC &&
               Note == other.Note;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(No);
        hashCode.Add(Date);
        hashCode.Add(Term);
        hashCode.Add(Department);
        hashCode.Add(ExpenseName);
        hashCode.Add(CostType);
        hashCode.Add(UnitPrice);
        hashCode.Add(Amount);
        hashCode.Add(Currency);
        hashCode.Add(ExchangeRate);
        hashCode.Add(TotalAmount);
        hashCode.Add(ProjectName);
        hashCode.Add(SupplierName);
        hashCode.Add(PIC);
        hashCode.Add(Note);

        return hashCode.GetHashCode();
    }

}