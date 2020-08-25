using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BMSDbEntities;

namespace BMSWebApp1.Controllers
{

    public class GenresController : ApiController
    {
        [HttpGet]
        [Route("api/genre")]
        public IHttpActionResult GetGenres()
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                return Ok(entities.GENREs.ToList());
            }
        }
        [HttpGet]
        [Route("api/genre/{id?}")]
        public IHttpActionResult GetGenres(int id)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                return Ok(entities.GENREs.FirstOrDefault(c => c.GenreID == id));
            }
        }
        [HttpPost]
        [Route("api/genre")]
        public HttpResponseMessage PostGenre([FromBody] GENRE genre)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                try
                {
                    entities.GENREs.Add(genre);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, genre);
                    message.Headers.Location = new Uri(Request.RequestUri + genre.GenreID.ToString());
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
        [Route("api/genre/{id?}")]
        public void DeleteGenre(int id)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                entities.GENREs.Remove(entities.GENREs.FirstOrDefault(c => c.GenreID == id));
                entities.SaveChanges();
            }
        }

        [HttpPut]
        [Route("api/genre/{id?}")]
        public void PutGenre(int id, [FromBody] GENRE genre)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                var entity = entities.GENREs.FirstOrDefault(c => c.GenreID == id);
                entity.GenreName = genre.GenreName;
                entities.SaveChanges();
            }
        }
    }
}
