using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using FinancialPlanning.Service.Token;
using FinancialPlanning.WebAPI.Models.Report;
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
                var departments = _reportService.GetAllDepartment();    //Get all department
                var terms = _termService.GetAllTerms();   //Get all term

                //Mapper List report and reportViewModel
                var reportViewModels = _mapper.Map<List<ReportViewModel>>(reports);

                var result = new
                { 
                   
                    Terms = terms
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
