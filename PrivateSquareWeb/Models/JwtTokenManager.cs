using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace PrivateSquareWeb.Models
{
    public class JwtTokenManager
    {
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
        public string GenerateToken(string data, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, data) }),
                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);
            return token;
        }

        public string DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {

                return JsonConvert.SerializeObject(tokenHandler.ReadJwtToken(token).Payload);
            }
            catch (Exception ex)
            {
                string Exception = ex.ToString();
                var decoded = token.Split('.').Take(2).Select(x => Encoding.UTF8.GetString(Convert.FromBase64String(x.PadRight(x.Length + (x.Length % 4), '='))));
                //Encoding.UTF8.GetString(Convert.FromBase64String(token.PadRight(token.Length + (token.Length - token.Length % 4) % 4, '=')));
                return decoded.ToArray()[1];// token.Split('.').Take(2).Select(x => Encoding.UTF8.GetString(Convert.FromBase64String(x.PadRight(x.Length + (x.Length % 4), '='))));
            }

        }
    }
}