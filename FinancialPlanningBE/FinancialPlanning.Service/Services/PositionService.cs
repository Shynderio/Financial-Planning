using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Service.Services
{
    public class PositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }
        //Get all positions
        public async Task<List<Position>> GetAllPositions()
        {
            var result = await _positionRepository.GetAllPositons();
            return result;
        }
    }
}
