
using FinancialPlanningBAL.DTOs;
using FinancialPlanningBAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanningAPI.Controllers
{
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;
        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("send")]
        public IActionResult SendEmail(EmailDTO request)
        {
            _emailService.SendEmail(request);
            return Ok("Email sent successfully");
        }
    }
}