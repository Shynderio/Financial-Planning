using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Service.Services
{
    public class DepartmentService
    {
        private readonly IDepartmentRepository _departmentrepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentrepository = departmentRepository;
        }
        //Get all Departments
        public async Task<List<Department>> GetAllDepartments()
        {
            var result = await _departmentrepository.GetAllDepartments();
            return result;
        }
    }
}
