using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BMSDbEntities;
using BMSWebApp1.Helper;
using BMSWebApp1.Models;

namespace BMSWebApp1.Controllers
{
    public class BookingController : ApiController
    {
        [HttpPost]
        [Route("api/booking")]
        public IHttpActionResult PostBooking([FromBody] BookingViewModel bookingViewModel)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                DbContextTransaction transaction = entities.Database.BeginTransaction();
                try
                {
                    bookingViewModel.BookingDate = BMSDbMethods.ConvertToIndianTime(DateTime.UtcNow);
                    bookingViewModel.PaymentDate = BMSDbMethods.ConvertToIndianTime(DateTime.UtcNow);

                    BOOKING bookingInfo = new BOOKING
                    {
                        BookingDate = bookingViewModel.BookingDate,
                        CustomerID = bookingViewModel.CustomerID,
                        ShowID = bookingViewModel.ShowID,
                        ShowDate = bookingViewModel.ShowDate
                    };
                    entities.BOOKINGs.Add(bookingInfo);
                    entities.SaveChanges();

                    PAYMENTDETAIL paymentInfo = new PAYMENTDETAIL
                    {
                        BookingID = bookingInfo.BookingID,
                        PaymentDate = bookingViewModel.PaymentDate,
                        PaymentAmount = bookingViewModel.PaymentAmount,
                        TransactionMode = bookingViewModel.TransactionMode
                    };
                    entities.PAYMENTDETAILS.Add(paymentInfo);
                    entities.SaveChanges();
                    List<MOVIEHALLSEAT> hallSeats = new List<MOVIEHALLSEAT>();

                    foreach (var cs in bookingViewModel.ConfirmedSeats)
                    {
                        var hallSeatEntity = entities.MOVIEHALLSEATs.FirstOrDefault(s => s.SeatNumber==cs);
                        bookingInfo.MOVIEHALLSEATs.Add(hallSeatEntity);

                    }
                    entities.SaveChanges();
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message)); ;



                }
            }
        }
    }
}
