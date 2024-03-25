using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Entities
{
    public class ExpenseAnnualReport
    {
        public string Department { get; set; }
        public int TotalExpense { get; set; }
        public int BiggestExpenditure { get; set; }
        public string CostType { get; set; }
    }
}
