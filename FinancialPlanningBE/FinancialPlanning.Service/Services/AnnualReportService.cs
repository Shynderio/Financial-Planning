﻿using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
            _reportRepository = reportRepository;
        }

        public async Task<string> GenerateAnnualReport()
        {
            try
            {
                DateTime timenow = DateTime.Now;
                DateTime create_at = new DateTime(timenow.Year, 12, 20);

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
                    byte[] file = await _fileService.GetFileAsync("CorrectPlan.xlsx");

                    //Change 1 
                    //Get expense of report
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
                var annualFile = await _fileService.ConvertAnnualReportToExcel(expenseAnnualReports, annualreport);
                //Import file to cloud
                string filePath = Path.Combine("AnnualExpenseReport", "AnnualReport_" + year + ".xlsx");

                await _fileService.UploadFileAsync(filePath.Replace('\\', '/'), new MemoryStream(annualFile));

                return "Import successfully!";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<IEnumerable<AnnualReport>> GetAllAnnualReportsAsync()
        {
            DateTime dateTime = DateTime.Now;
            DateTime timeGenAnnualReport = new DateTime(dateTime.Year, 12, 20);
            int currentYear = dateTime.Year;
            if (dateTime <= timeGenAnnualReport)
            {
                currentYear--;
            }
            List<AnnualReport> annualReports = new List<AnnualReport>();
            while (currentYear > 0)
            {
                try
                {
                    var file = await _fileService.GetFileAsync("AnnualExpenseReport/AnnualReport_" + currentYear + ".xlsx");
                    List<ExpenseAnnualReport> expenses;
                    List<AnnualReport> annual;
                    //Convert
                    (expenses, annual) = _fileService.ConvertExelAnnualReport(new ExcelPackage(new MemoryStream(file)));
                    annualReports.AddRange(annual);
                }
                catch
                {
                    break;
                }
                currentYear--;
            }

            return annualReports;

        }

    }
}
