using AutoMapper;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Service.Services;
using FinancialPlanning.Service.Token;
using FinancialPlanning.WebAPI.Models;
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

        public ReportController(AuthService authService, IMapper mapper, ReportService reportService,TokenService tokenService)
        {
            _authService = authService;
            _mapper = mapper;
            _reportService = reportService;
            _tokenService = tokenService;
        }

        // Phương thức để lấy danh sách báo cáo của user
        [HttpGet("reports")]
        [Authorize]
        public async Task<IActionResult> GetListReport()
        {
            try
            {
                // Lấy token từ authorization header
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Lấy user email từ JWT token
                var useremail = _tokenService.GetEmailFromToken(token);

                // Lấy danh sách báo cáo của user từ cơ sở dữ liệu
                var reports = await _reportService.GetReportsByEmail(useremail);

                //Mapper List report and reportViewModel
                var result = _mapper.Map<List<ReportViewModel>>(reports);

                return Ok(result);
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

          
        }

    }
}
