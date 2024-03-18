using FinancialPlanning.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController(PositionService positionService) : ControllerBase
    {
        private readonly PositionService _positionService = positionService ?? throw new ArgumentNullException(nameof(positionService));
        //Get all positions
        [HttpGet]
        public async Task<IActionResult> GetAllPositions()
        {
            var roles = await _positionService.GetAllPositions();
            return Ok(roles);
        }
    }
}
