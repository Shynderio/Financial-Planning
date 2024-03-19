using FinancialPlanning.Service.Services;
using FinancialPlanning.WebAPI.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanning.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(DepartmentService departmentService) : ControllerBase
    {
        private readonly DepartmentService _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        //Get all departments
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartments();
            return Ok(departments);
        }
    }
  
}
