using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public interface IEmailRepository
    {
        void SendWelcomeEmail(string Username, string Password, string emailAddress, string createdUser);
    }
}
