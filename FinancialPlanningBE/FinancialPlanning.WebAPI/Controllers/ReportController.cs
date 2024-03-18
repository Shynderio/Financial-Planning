using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using FinancialPlanning.Service.Token;
using FinancialPlanning.WebAPI.Models.Department;
using FinancialPlanning.WebAPI.Models.Report;
using FinancialPlanning.WebAPI.Models.Term;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ReportController(AuthService authService, IMapper mapper,
            ReportService reportService,TokenService tokenService,TermService termService)
        {
            _authService = authService;
            _mapper = mapper;
            _reportService = reportService;
            _tokenService = tokenService;
            this._termService = termService;
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

    }
}
