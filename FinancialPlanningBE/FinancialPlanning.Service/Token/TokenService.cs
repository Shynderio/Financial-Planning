using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Service.Token
{
    public class TokenService
    {
        //Get email form token
        public string GetEmailFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken != null)
            {
                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "email");
                if (emailClaim != null)
                {
                    return emailClaim.Value;
                }
            }

            return null;
        }
       
    }
}
