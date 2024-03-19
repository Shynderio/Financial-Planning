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
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext context;

        public RoleRepository(DataContext context)
        {
            this.context = context;

        }
        public async Task<List<Role>> GetAllRoles()
        {
            return await context.Roles!.ToListAsync();
        }
    }
}
