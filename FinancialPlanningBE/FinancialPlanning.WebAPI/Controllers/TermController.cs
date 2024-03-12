using AutoMapper;
using FinancialPlanning.Service.Services;
using FinancialPlanning.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using FinancialPlanning.WebAPI.Models.Term;

namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TermController(IMapper mapper, TermService termService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly TermService _termService = termService ?? throw new ArgumentNullException(nameof(termService));

        [HttpPost]
        public async Task<IActionResult> CreateTerm(CreateTermModel termModel)
        {
            if (ModelState.IsValid)
            {
                var term = _mapper.Map<Term>(termModel);
                await _termService.CreateTerm(term);
                return Ok(new { message = "Term created successfully!" });
            }

            return BadRequest(new { error = "Invalid model state!" });
        }

        [HttpPut("start/{id:guid}")]
        public async Task<IActionResult> StartTerm(Guid id)
        {
            await _termService.StartTerm(id);
            return Ok(new { message = "Term started successfully!" });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTermById(Guid id)
        {
            var term = await _termService.GetTermById(id);
            return Ok(term);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTerms()
        {
            var terms = await _termService.GetAllTerms();
            var termListModels = terms.Select(t => _mapper.Map<TermListModel>(t)).ToList();
            return Ok(termListModels);
        }

        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateTerm(Guid id, CreateTermModel termModel)
        {
            if (ModelState.IsValid)
            {
                var term = _mapper.Map<Term>(termModel);
                term.Id = id;
                await _termService.UpdateTerm(term);
                return Ok(new { message = $"Term with id {id} updated successfully!" });
            }

            return BadRequest(new { error = "Invalid model state!" });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTerm(Guid id)
        {
            await _termService.DeleteTerm(id);
            return Ok(new { message = $"Term with id {id} deleted successfully!" });
        }
    }
}
