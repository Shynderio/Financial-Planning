using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext context;

        public DepartmentRepository(DataContext context)
        {
            this.context = context;

        }
        //Get DepartmentId by email
        public async Task<string> GetDepartmentIdByEmail(string email)
        {
            var user = context.Users!.FirstOrDefault(x => x.Email == email); 
            return user.DepartmentId.ToString();
        }
        

        //Get name of department
        public async Task<string> GetDepartmentNameByUser(User user)
        {

            var department = await context.Departments!.SingleOrDefaultAsync(d => d.Id == user.DepartmentId);
            if (department != null)
            {
                return department.DepartmentName;
            }

            return "";
        }

     
    }
}
