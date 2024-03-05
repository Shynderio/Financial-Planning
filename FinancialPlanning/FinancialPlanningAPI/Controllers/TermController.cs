using AutoMapper;
using FinancialPlanningAPI.Models;
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

        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartTerm(Guid id)
        {
            await _termService.StartTerm(id);
            return Ok();
        }

        [HttpGet("allTerms")]
        public async Task<IActionResult> getAllTerms()
        {
            var terms = await _termService.GetAllTerms();
            return Ok(terms);
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateTerm(Guid id, CreateTermModel termModel)
        {
            if (ModelState.IsValid)
            {
                var term = _mappingProfile.Map<Term>(termModel);
                term.Id = id;
                await _termService.UpdateTerm(term);
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTerm(Guid id)
        {
            await _termService.DeleteTerm(id);
            return Ok();
        }
        // Other actions (GET, PUT, DELETE) can be added here
    }

}