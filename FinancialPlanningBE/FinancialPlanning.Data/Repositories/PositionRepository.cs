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
    public class PositionRepository : IPositionRepository
    {
        private readonly DataContext context;

        public PositionRepository(DataContext context)
        {
            this.context = context;

        }
        public async Task<List<Position>> GetAllPositons()
        {
            return await context.Positions!.ToListAsync();
        }
    }
}
