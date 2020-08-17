using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BMSDbEntities;

namespace BMSWebApp1.Controllers
{
    public class BookingController : ApiController
    {
        //[HttpPost]
        //[Route("api/booking")]
        //public HttpResponseMessage PostBooking([FromBody] BOOKING booking)
        //{
        //    using (BMSApplicationEntities entities = new BMSApplicationEntities())
        //    {
        //        try
        //        {
        //            booking.BookingDate = DateTime.Now();
        //            entities.BOOKINGs.Add(booking);
        //            entities.SaveChanges();
        //            var message = Request.CreateResponse(HttpStatusCode.Created, genre);
        //            message.Headers.Location = new Uri(Request.RequestUri + genre.GenreID.ToString());
        //            return message;
        //        }
        //        catch (Exception ex)
        //        {
        //            return
        //               Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);


        //        }
        //    }
        //}
    }
}
