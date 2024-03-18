using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Service.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        //Get all roles
        public async Task<List<Role>> GetAllRoles()
        {
            var result = await _roleRepository.GetAllRoles();
            return result;
        }
    }
}
