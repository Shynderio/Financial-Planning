using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Entities
{
    public class ExpenseAnnualReport
    {
        public string Department { get; set; } = string.Empty;
        public long TotalExpense { get; set; }
        public long BiggestExpenditure { get; set; }
        public string CostType { get; set; } = string.Empty;
      
    }
}
