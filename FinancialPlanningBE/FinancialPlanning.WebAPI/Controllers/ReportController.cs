using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using FinancialPlanning.Service.Token;
using FinancialPlanning.WebAPI.Models.Department;
using FinancialPlanning.WebAPI.Models.Report;
using FinancialPlanning.WebAPI.Models.Term;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
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
                // get token from authorization header
                var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
                var useremail = _tokenService.GetEmailFromToken(token); // Get user email from JWT token

                var reports = await _reportService.GetReportsByEmail(useremail); // Get list reports          
                var departments = await _reportService.GetAllDepartment(); //Get all department
                var terms = await _termService.GetAllTerms(); //Get all term

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


        [HttpGet("details/{id:guid}")]
        [Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> GetReportDetails(Guid id)
        {
            try
            {
                //Get report
                var report = await _reportService.GetReportById(id);
                //Get reportVersions
                var reportVersions = await _reportService.GetReportVersionsAsync(id);

                var expenses = _fileService.ConvertExcelToList(await _fileService.GetFileAsync("CorrectPlan.xlsx"), 0);

                //mapper
                var reportViewModel = _mapper.Map<ReportViewModel>(report);
                var reportVersionModel = _mapper.Map<IEnumerable<ReportVersionModel>>(reportVersions).ToList();
                // Get the name of the user who uploaded the file
                var firstReportVersion = reportVersionModel.FirstOrDefault();
                var uploadedBy = firstReportVersion?.UploadedBy;

                var result = new
                {
                    Report = reportViewModel,
                    Expenses = expenses,
                    ReportVersions = reportVersionModel,
                    UploadedBy = uploadedBy
                };

                return Ok(result);
            }
            //error when download
            catch (Exception ex)
            {
                return StatusCode(500, $"Error : {ex.Message}");
            }
        }

        //export report 
        [HttpGet("export/{id:guid}/{version:int}")]
        //[Authorize(Roles = "Accountant, FinancialStaff")]
        public async Task<IActionResult> ExportSingleReport(Guid id, int version)
        {
            try
            {
                //from reportVersion Id -> get name report + version
                await _reportService.GetReportById(id);
                //get url from name file
                var url = await _reportService.GetFileByName("CorrectPlan.xlsx");

                // return URL
                return Ok(new { downloadUrl = url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex });
            }
        }

        [HttpGet("export")]
        public Task<IActionResult> ExportMultipleReport(List<Guid> reportIds)
        {
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}