﻿using FinancialPlanning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public interface IDepartmentRepository
    {
        public Task<string?> GetDepartmentIdByEmail(string email);
        public Task<string> GetDepartmentNameByUser(User user);
    }
}
