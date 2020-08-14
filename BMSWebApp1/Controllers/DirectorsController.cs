using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BMSDbEntities;

namespace BMSWebApp1.Controllers
{

    public class DirectorsController : ApiController
    {
        [HttpGet]
        [Route("api/director")]
        public IHttpActionResult GetDirectors()
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                return Ok(entities.DIRECTORs.ToList());
            }
        }
        [HttpGet]
        [Route("api/director/{id?}")]
        public IHttpActionResult GetDirectors(int id)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                return Ok(entities.DIRECTORs.FirstOrDefault(c => c.DirectorID == id));
            }
        }
        [HttpPost]
        [Route("api/director")]
        public HttpResponseMessage PostDirector([FromBody] DIRECTOR director)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                try
                {
                    entities.DIRECTORs.Add(director);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, director);
                    message.Headers.Location = new Uri(Request.RequestUri + director.DirectorID.ToString());
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
        [Route("api/director/{id?}")]
        public void DeleteDirector(int id)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                entities.DIRECTORs.Remove(entities.DIRECTORs.FirstOrDefault(c => c.DirectorID == id));
                entities.SaveChanges();
            }
        }

        [HttpPut]
        [Route("api/director/{id?}")]
        public void PutDirector(int id, [FromBody] DIRECTOR director)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                var entity = entities.DIRECTORs.FirstOrDefault(c => c.DirectorID == id);
                entity.DirectorName = director.DirectorName;
                entities.SaveChanges();
            }
        }
    }
}
