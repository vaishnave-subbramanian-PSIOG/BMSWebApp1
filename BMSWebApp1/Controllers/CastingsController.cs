using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BMSDbEntities;

namespace BMSWebApp1.Controllers
{

    public class CastingsController : ApiController
    {
        [HttpGet]
        [Route("api/casting")]
        public IHttpActionResult GetCastings()
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                return Ok(entities.CASTINGs.ToList());
            }
        }
        [HttpGet]
        [Route("api/casting/{id?}")]
        public IHttpActionResult GetCastings(int id)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                return Ok(entities.CASTINGs.FirstOrDefault(c => c.CastingID == id));
            }
        }
        [HttpPost]
        [Route("api/casting")]
        public HttpResponseMessage PostCasting([FromBody] CASTING casting)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                try
                {
                    entities.CASTINGs.Add(casting);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, casting);
                    message.Headers.Location = new Uri(Request.RequestUri + casting.CastingID.ToString());
                    return message;
                }
                catch (Exception ex)
                {
                    return
                       Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);


                }
            }
        }

        [HttpDelete]
        [Route("api/casting/{id?}")]
        public void DeleteCasting(int id)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                entities.CASTINGs.Remove(entities.CASTINGs.FirstOrDefault(c => c.CastingID == id));
                entities.SaveChanges();
            }
        }

        [HttpPut]
        [Route("api/casting/{id?}")]
        public void PutCasting(int id, [FromBody] CASTING casting)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                var entity = entities.CASTINGs.FirstOrDefault(c => c.CastingID == id);
                entity.ActorName = casting.ActorName;
                entities.SaveChanges();
            }
        }
    }
}
