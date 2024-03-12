using AutoMapper;
using FinancialPlanning.Service.Services;
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
        public ReportController(AuthService authService, IMapper mapper, ReportService reportService)
        {
            _authService = authService;
            _mapper = mapper;
            _reportService = reportService;
        }

        // Phương thức để lấy danh sách báo cáo của user
        [HttpGet]
        public async Task<IActionResult> GetListReport()
        {
            try
            {
                // Lấy token từ authorization header
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Lấy user email từ JWT token
                var useremail = _authService.GetUserFromToken(token);

                // Lấy danh sách báo cáo của user từ cơ sở dữ liệu
                var reports = await _reportService.GetReportsByEmail(useremail);

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            // Trả về một giá trị mặc định nếu không có lỗi xảy ra
            return BadRequest();
        }

    }
}
