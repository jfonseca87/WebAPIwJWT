namespace WebAPIwJWT.Utilities.JWT
{
    using System;
    using System.Configuration;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;

    public static class TokenGenerator
    {
        public static string GenerateTokenJwt(string userName)
        {
            string secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            string audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            string issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            string expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) });

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            IdentityModelEventSource.ShowPII = true;
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                    subject: claimsIdentity,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                    signingCredentials: signingCredentials
                );
            string jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);

            return jwtTokenString;
        }
    }
}