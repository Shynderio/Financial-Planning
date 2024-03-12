using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Service.Services
{
    public class ReportService
    {
        private readonly IReportRepository _repository;
        private readonly IConfiguration configuration;
       public ReportService(IReportRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            this.configuration = configuration;
        }

        public async Task<string> GetReportsByEmail(string email)
        {



            return "";
        }
    }
}
