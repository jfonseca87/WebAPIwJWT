namespace WebAPIwJWT.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using WebAPIwJWT.Models;

    [Authorize]
    [RoutePrefix("api/Partners")]
    public class SocioController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetPartners()
        {
            try
            {
                using (TestJWTModel db = new TestJWTModel())
                {
                    return Ok(db.Socio.ToList());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{idPartner}")]
        public IHttpActionResult GetPartnerById(int idPartner)
        {
            try
            {
                using (TestJWTModel db = new TestJWTModel())
                {
                    return Ok(db.Socio.FirstOrDefault(x => x.IdSocio == idPartner));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreatePartner(Socio socio)
        {
            if (socio == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                using (TestJWTModel db = new TestJWTModel())
                {
                    db.Socio.Add(socio);
                    db.SaveChanges();
                    return Created("", socio);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdatePartner(Socio socio)
        {
            if (socio == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                using (TestJWTModel db = new TestJWTModel())
                {
                    Socio partnerToUpdate = db.Socio.FirstOrDefault(x => x.IdSocio == socio.IdSocio);

                    if (partnerToUpdate == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

                    partnerToUpdate.Nombre = socio.Nombre;

                    db.Entry(partnerToUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return Ok(partnerToUpdate);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{idPartner}")]
        public IHttpActionResult DeletePartner(int idPartner)
        {
            try
            {
                using (TestJWTModel db = new TestJWTModel())
                {
                    Socio partnerToDelete = db.Socio.FirstOrDefault(x => x.IdSocio == idPartner);

                    if (partnerToDelete == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }

                    db.Socio.Remove(partnerToDelete);
                    db.SaveChanges();

                    return Ok(partnerToDelete);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}