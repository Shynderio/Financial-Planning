using AutoMapper;
using FinancialPlanningAPI.ViewModels;
using FinancialPlanningBAL.Services;
using FinancialPlanningDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermController : ControllerBase
    {
        private readonly IMapper _mappingProfile;
        private readonly TermService _termService;
        public TermController(IMapper mappingProfile, TermService termService)
        {
            _mappingProfile = mappingProfile;
            _termService = termService;
        }
        // private readonly FinancialPlanningBAL.Services.TermService _termService;
        // POST api/term
        [HttpPost]
        public async Task<IActionResult> CreateTerm(CreateTermModel termModel)
        {
            if (ModelState.IsValid)
            {
                var term = _mappingProfile.Map<Term>(termModel);
                await _termService.CreateTerm(term);
                return Ok();
            }

            return BadRequest();
        }

        // Other actions (GET, PUT, DELETE) can be added here
    }

}