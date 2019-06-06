namespace WebAPIwJWT.Utilities.JWT
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.IdentityModel.Tokens;

    internal class TokenValidationHandler : DelegatingHandler
    {
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authHeaders;

            if (!request.Headers.TryGetValues("Authorization", out authHeaders) || authHeaders.Count() > 1)
            {
                return false;
            }

            string bearerToken = authHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                string secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
                string audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                string issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
                string expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));

                SecurityToken securityToken;
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            bool isValid = expires != null && (DateTime.UtcNow < expires) ? true : false;
            return isValid;
        }
    }
}