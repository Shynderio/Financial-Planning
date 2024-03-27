﻿using FinancialPlanning.Common;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using OfficeOpenXml;

namespace FinancialPlanning.Service.Services
{
    public class ReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly FileService _fileService;

        public ReportService(IReportRepository reportRepository,
            IAuthRepository authRepository, IDepartmentRepository departmentRepository, FileService fileService)
        {
            _reportRepository = reportRepository;
            _fileService = fileService;
            _authRepository = authRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Report>> GetReportsByEmail(string email)
        {
            var role = await _authRepository.GetRoleUser(email);

            //If role is FinancialStaff 
            if (role.Equals("FinancialStaff"))
            {
                //Get departmentId
                var departIdRaw = await _departmentRepository.GetDepartmentIdByEmail(email);
                Console.WriteLine(departIdRaw);
                var src = departIdRaw;
                var departId = Guid.Parse(src);
                //Get report by DepartmentId
                var reports = await _reportRepository.GetReportsByDepartId(departId);
                return reports;
            }
            else
            {
                //If role is accountant - getAll
                var reports = await _reportRepository.GetAllReports();
                return reports;
            }
        }

        public async Task DeleteReport(Guid id)
        {
            var reportToDelete = await _reportRepository.GetReportById(id);
            if (reportToDelete != null)
            {
                await _reportRepository.DeleteReportVersions(reportToDelete.ReportVersions!);
                await _reportRepository.DeleteReport(reportToDelete);
            }
            else
            {
                throw new ArgumentException("Report not found with the specified ID");
            }
        }

        public async Task<IEnumerable<Department>> GetAllDepartment()
        {
            return await _departmentRepository.GetAllDepartment();
        }

        public async Task<string> GetFileByName(string key)
        {
            return await _fileService.GetFileUrlAsync(key);
        }

        public async Task<Report?> GetReportById(Guid id)
        {
            var report = await _reportRepository.GetReportById(id);
            return report;
        }

        public async Task<IEnumerable<ReportVersion>> GetReportVersionsAsync(Guid reportId)
        {
            var repostVersions = await _reportRepository.GetReportVersionsByReportID(reportId);
            return repostVersions;
        }

        public async Task<byte[]> MergeExcelFiles(List<Guid> reportIds)
        {
            var reports = new List<Report>();
            foreach (var reportId in reportIds)
            {
                var report = await _reportRepository.GetReportById(reportId);
                if (report == null)
                {
                    throw new ArgumentException($"Report with id {reportId} does not exist.");
                }

                reports.Add(report);
            }

            reports = reports.OrderBy(r => r.DepartmentId).ToList();

            var expenses = new List<Expense>();
            foreach (var report in reports)
            {
                expenses.AddRange(_fileService.ConvertExcelToList(
                    await _fileService.GetFileAsync(report.Department.DepartmentName + "/"
                    + report.Term.TermName + "/" + report.Month + "/Report/version_" + report.GetMaxVersion()+ ".xlsx"),
                    1));
            }
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package =
                new ExcelPackage(new FileInfo(Path.Combine(Directory.GetCurrentDirectory(),
                    Constants.TemplatePath[1])));
            var worksheet = package.Workbook.Worksheets[0];
            worksheet.InsertColumn(1, 1); // Add a column to the left of the worksheet
            worksheet.Cells[1, 1].Value = "No.";
            
            var currentDepartment = expenses[0].Department;
            var currentIndex = 1;

            foreach (var expense in expenses)
            {
                if (!expense.Department.Equals(currentDepartment))
                {
                    currentDepartment = expense.Department;
                    currentIndex = 1;
                }
                expense.No = currentIndex++;
            }

            return await _fileService.ConvertListToExcelAsync(expenses, 1);
        }

        public bool ValidateReportFile(byte[] file){
            try
            {
                // Assuming plan documents have document type 0
                return _fileService.ValidateFile(file, documentType: 1);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new InvalidOperationException("An error occurred while validating the plan file.", ex);
            }
        }

        public List<Expense> GetExpenses(byte[] file)
        {
            try
            {
                // Convert the file to a list of expenses using FileService
                var expenses = _fileService.ConvertExcelToList(file, documentType: 1);
                return expenses;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new InvalidOperationException("An error occurred while importing the report file.", ex);
            }
        }

        public async Task CreateReport(List<Expense> expenses, Report report, Guid userId)
        {
            var departId = _departmentRepository.GetDepartmentIdByUid(userId);
            report.DepartmentId = departId;
            var isReportExist = await _reportRepository.IsReportExist(report.TermId, report.DepartmentId, report.Month);
            if (isReportExist)
            {
                throw new ArgumentException("Report already exists with the specified term, department and month");
            } else {
                report.Status = (int)ReportStatus.New;
                report.UpdateDate = DateTime.Now;
                var result = await _reportRepository.CreateReport(report, userId);

                var filename = Path.Combine(result.Department.DepartmentName, result.Term.TermName, result.Month, "Report", "version_" + result.GetMaxVersion() + ".xlsx");
                // Convert list of expenses to Excel file                        
                var excelFileStream = await _fileService.ConvertListToExcelAsync(expenses, 1);
                // Upload the file to AWS S3
                await _fileService.UploadFileAsync(filename.Replace('\\', '/'), new MemoryStream(excelFileStream));
            }
        }

        public async Task ReupReport(List<Expense> expenses, Guid reportId, Guid userId)
        {
            var isReportExist = await _reportRepository.GetReportById(reportId);
            if (isReportExist == null)
            {
                throw new ArgumentException("Report not found with the specified ID");
            } else {
                await _reportRepository.ReupReport(reportId, userId);

                var report = await _reportRepository.GetReportById(reportId);
                var filename = Path.Combine(report!.Department.DepartmentName, report.Term.TermName, report.Month, "Report", "version_" + report.GetMaxVersion() + ".xlsx");
                // Convert list of expenses to Excel file
                var excelFileStream = await _fileService.ConvertListToExcelAsync(expenses, 1);
                // Upload the file to AWS S3
                await _fileService.UploadFileAsync(filename.Replace('\\', '/'), new MemoryStream(excelFileStream));
            }
        }

        public async Task CloseDueReports()
        {
            var reports = await _reportRepository.GetAllDueReports();
            await _reportRepository.CloseAllDueReports(reports);
        }

        internal async Task GenerateAnnualReport()
        {
            await Task.CompletedTask;
        }
    }
}
