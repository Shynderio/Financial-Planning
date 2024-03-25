using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Service.Services
{
    public class AnnualReportService
    {
        private readonly ITermRepository _termRepository;
        private readonly IReportRepository _reportRepository;
        private readonly FileService _fileService;

        public AnnualReportService(IReportRepository reportRepository, ITermRepository termRepository, FileService fileService)
        {
            _fileService = fileService;
            _termRepository = termRepository;
            _termRepository = termRepository;
        }

        public async Task ImportAnnualReport()
        {
            try
            {
                DateTime timenow = DateTime.Now;
                DateTime create_at = new DateTime(timenow.Year, 12, 20).Date;

                int year = timenow.Year;
                //Total term 
                int totalTerm = await _termRepository.GetTotalTermByYear(year);
                //Total department
                int totalDepartment = await _reportRepository.GetTotalDepartByYear(year);

                var annualreport = new AnnualReport
                {
                    Year = year,
                    CreateDate = create_at,
                    TotalTerm = totalTerm,
                    TotalDepartment = totalDepartment,
                };
                List<ExpenseAnnualReport> expenseAnnualReports = new List<ExpenseAnnualReport>();
                List<Report> reports = await _reportRepository.GetAllReportsByYear(year);
                foreach (Report report in reports)
                {
                    string fileName = report.ReportName;
                    byte[] file = await _fileService.GetFileAsync(fileName);

                    //Change 1 
                    List<Expense> expenses = _fileService.ConvertExcelToList(file, 0);
                    decimal totalExpense = expenses.Sum(e => e.TotalAmount);
                    decimal biggestExpense = expenses.Max(e => e.TotalAmount);
                    ExpenseAnnualReport expenseAnnualReport = new ExpenseAnnualReport
                    {
                        Department = report.Department.DepartmentName,
                        TotalExpense = (int)totalExpense,
                        BiggestExpenditure = (int)biggestExpense,
                        CostType = expenses[0].CostType,
                    };
                    expenseAnnualReports.Add(expenseAnnualReport);
                }

                //Convert list to exel
                byte[] annualFile = await _fileService.ConvertAnnualReportToExcel(expenseAnnualReports, annualreport);
                //Import file to cloud

            }
            catch (Exception ex)
            {
               
            }

        }

    }
}
