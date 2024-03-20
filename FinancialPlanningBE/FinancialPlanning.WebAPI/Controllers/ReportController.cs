using Amazon.Runtime.Documents;
using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using FinancialPlanning.Service.Token;
using FinancialPlanning.WebAPI.Models.Department;
using FinancialPlanning.WebAPI.Models.Report;
using FinancialPlanning.WebAPI.Models.Term;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IMapper _mapper;
        private readonly ReportService _reportService;
        private readonly TokenService _tokenService;
        private readonly TermService _termService;
        private readonly FileService _fileService;

        public ReportController(AuthService authService, IMapper mapper,
            ReportService reportService, TokenService tokenService, TermService termService,
            FileService fileService
            )
        {

            _authService = authService;
            _mapper = mapper;
            _reportService = reportService;
            _tokenService = tokenService;
            _termService = termService;
            _fileService = fileService;


        }

        // Phương thức để lấy danh sách báo cáo của user
        [HttpGet("reports")]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> GetListReport()
        {

            try
            {
                // get token ffrom authorization header
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var useremail = _tokenService.GetEmailFromToken(token);   // Get user email from JWT token

                var reports = await _reportService.GetReportsByEmail(useremail); // Get list reports          
                var departments = await _reportService.GetAllDepartment();    //Get all department
                var terms = await _termService.GetAllTerms();   //Get all term

                //Mapper model
                var reportViewModels = _mapper.Map<List<ReportViewModel>>(reports);
                var termListModels = terms.Select(t => _mapper.Map<TermListModel>(t)).ToList();
                var departmentViewModel = _mapper.Map<List<DepartmentViewModel>>(departments);

                var result = new
                {
                    Reports = reportViewModels,
                    Terms = termListModels,
                    Departments = departmentViewModel
                };

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> DeleteReport(Guid id)
        {
            await _reportService.DeleteReport(id);
            return Ok(new { message = $"Report with id {id} deleted successfully!" });
        }

        

        [HttpGet("{id:guid}")]
        //[Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> DownloadFileFromUrlAsync(Guid id)
        {
            try
            {
              var report = await _reportService.GetReportById(id);
              var reportVersions = await _reportService.GetReportVersionsAsync(id);
                //string reportName = report.ReportName;
                string reportName = "CorrectPlan";
                string url = await _reportService.GetFileByName(reportName+ ".xlsx");
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", reportName+ ".xlsx");

                bool isDownLoad = await _fileService.DownloadFile(url, savePath);
                //download file sucessfull 
                if (isDownLoad)
                {
                    // conver file to list expense
                    using (var fileStream = new FileStream(savePath, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            List<Expense> expenses = _fileService.ConvertExcelToList(fileStream, 0);
                            //mapper
                            var reportViewModel = _mapper.Map<ReportViewModel>(report);
                            var reportVersionModel = _mapper.Map<IEnumerable<ReportVersionModel>>(reportVersions);
                            // Get the name of the user who uploaded the file
                            var firstReportVersion = reportVersionModel.FirstOrDefault();
                            var uploadedBy = firstReportVersion != null ? firstReportVersion.UploadedBy : null;
                            
                            var result = new
                            {
                                Report = reportViewModel,
                                Expenses = expenses,
                                ReportVersion = reportVersionModel,
                                UploadedBy = uploadedBy
                            };

                            return Ok(result);
                        }
                        catch
                        {
                            return BadRequest("Failed to convert");
                        }
                    }
                }
                else
                {
                    return BadRequest("Failed to download file");
                }
            }
            //error when download
            catch (Exception ex)
            {
                return StatusCode(500, $"Error : {ex.Message}");
            }
        }

    }
}
