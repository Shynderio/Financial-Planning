using FinancialPlanning.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinancialPlanning.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase    
    {
        // POST: api/plan
        // [HttpPost("import")]
        // public async ActionResult<string> Import(IFormFile file, String user)
        // {
        //     if (file == null)
        //     {
        //         return BadRequest("No file uploaded");
        //     }

        //     try {
        //         await _fileService.Import(file, user);
        //     }
        //     return Ok();
        // }


        [HttpGet("search")]
        public IActionResult SearchPlans(string keyword, Guid? departmentId, int? status)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { Message = "Please enter keyword", ErrorCode = "ME010" });
            }
            /*
            IQueryable<Plan> query = _context.Plans;

            if (departmentId.HasValue)
            {
                query = query.Where(p => p.DepartmentId == departmentId);
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status);
            }

            var searchResult = query.ToList();
            */
            return Ok();
        }
    }



}