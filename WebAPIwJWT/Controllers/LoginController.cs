namespace WebAPIwJWT.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Web.Http;
    using WebAPIwJWT.Models;
    using WebAPIwJWT.Utilities.JWT;

    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoPing")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echoUser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(User user)
        {
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                using (TestJWTModel db = new TestJWTModel())
                {
                    User userExist = db.User.FirstOrDefault(x => x.UserName.ToLower().Trim().Equals(user.UserName.ToLower().Trim()) &&
                                                                 x.Password.ToLower().Trim().Equals(user.Password.ToLower().Trim()));

                    if (userExist != null)
                    {
                        string token = TokenGenerator.GenerateTokenJwt(user.UserName.ToLower().Trim());
                        return Ok(token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}