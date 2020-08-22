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
                    var customer = entities.CUSTOMERs.FirstOrDefault(c => c.CustomerID == bookingInfo.CustomerID);
                    var movieVM = BMSDbMethods.MovieInformation(entities.MOVIEs.FirstOrDefault(m => m.SHOWINFOes.Any(s => s.ShowID == bookingInfo.ShowID)).MovieID);
                    if (movieVM == null)
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error")); ;

                    }
                    else if (movieVM.Any())
                    {
                        var showInfo = entities.SHOWINFOes.FirstOrDefault(c => c.ShowID == bookingInfo.ShowID);
                        var movieHallInfo = entities.MOVIEHALLs.FirstOrDefault(h=>h.MovieHallID==showInfo.MovieHallID);
                        var theatreInfo = entities.THEATRECOMPLEXes.FirstOrDefault(t=>t.TheatreID== movieHallInfo.TheatreID);
                        EmailVerificationLink.EmailLinkGenerator(customer.CustomerEmail,
    @"<div style='  display: block;
  margin-left: auto;
  margin-right: auto;
  width: 50%;'>
<div style='margin-bottom:20px'>
<h1>BOOKING CONFIRMATION</h1>
</div>
<div  style='margin-bottom:20px'><label>Booking date: </label>" + bookingInfo.BookingDate.ToString() + @"</div>
<div  style='margin-bottom:20px'><label>Movie:</label>" + movieVM.First().Name + @"</div>
<div  style='margin-bottom:20px'><img  src='"+ movieVM.First().TrailerURL+ @"' alt='' /></div>
<div  style='margin-bottom:20px'><label>Show date: </label>"+bookingInfo.ShowDate.ToString().Substring(0, 10) + @"</div>
<div  style='margin-bottom:20px'><label>Show time: </label>"+ showInfo.ShowTime.ToString().Substring(0, 5) + @"</div>
<div  style='margin-bottom:20px'><label>Theatre: </label>"+ theatreInfo.TheatreName+@", "+ theatreInfo.TheatreLocation+ @"</div>
<div  style='margin-bottom:20px'><label>Theatre address: </label>"+ theatreInfo.TheatreAddress+ @"</div>
<div  style='margin-bottom:20px'><label>Theatre contact number: </label>"+ theatreInfo.TheatreContact + @"</div>
<div  style='margin-bottom:20px'><label>Tickets chosen: </label>"+ string.Join(", ", bookingViewModel.ConfirmedSeats) +@"</div>
<div  style='margin-bottom:20px'><label><strong>Total price: </strong></label>Rs "+paymentInfo.PaymentAmount+ @"</div>
<div  style='margin-bottom:20px'><label>Transaction mode: </label>" + paymentInfo.TransactionMode+@"</div>
  </div>", "BookingConfirmation");
                    }
                    else
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid ID"));

                    }

                    return Ok(bookingInfo.BookingID);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Write(ex);
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message)); ;



                }
            }
        }

        [HttpGet]
        [Route("api/bookings/customer/{CustomerID}")]
        public IHttpActionResult GetAllBookingsForCustomer(int CustomerID)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                try
                {
                    var AllBookings = entities.BOOKINGs.Where(b=>b.CustomerID==CustomerID).ToList();
                    var AllPayments = entities.PAYMENTDETAILS.ToList();
                    var AllShows = entities.SHOWINFOes.ToList();
                    var AllMovieHalls = entities.MOVIEHALLs.ToList();
                    var AllTheatres = entities.THEATRECOMPLEXes.ToList();

                    var bookingsInfo = (from b in AllBookings
                                        join p in AllPayments on b.BookingID equals p.BookingID
                                        join s in AllShows on b.ShowID equals s.ShowID
                                        join h in AllMovieHalls on s.MovieHallID equals h.MovieHallID
                                        join t in AllTheatres on h.TheatreID equals t.TheatreID
                                        select new
                                        {
                                            b.BookingID,
                                            b.BookingDate,
                                            b.ShowDate,
                                            b.ShowID,
                                            s.ShowTime,
                                            p.PaymentDate,
                                            p.PaymentAmount,
                                            p.TransactionMode,
                                            t.TheatreName,
                                            t.TheatreLocation,
                                            t.TheatreAddress,
                                            t.TheatreContact
                                        });
                    List<BookingWithMovieViewModel> bookingWithMovieViewModelList = new List<BookingWithMovieViewModel>();
                    foreach (var bi in bookingsInfo)
                    {
                        BookingViewModel bookingViewModel = new BookingViewModel();
                        bookingViewModel.BookingDate = bi.BookingDate;
                        bookingViewModel.ShowID = bi.ShowID;
                        bookingViewModel.ShowDate = bi.ShowDate;
                        bookingViewModel.PaymentDate = bi.PaymentDate;
                        bookingViewModel.PaymentAmount = bi.PaymentAmount;
                        bookingViewModel.TransactionMode = bi.TransactionMode;
                        bookingViewModel.CustomerID = CustomerID;
                        var ReservedSeatsInfoForEachBooking = entities.MOVIEHALLSEATs.Where(mhs => mhs.BOOKINGs.Any(b => b.BookingID == bi.BookingID)).ToList();
                        List<string> ReservedSeatsForEachBooking = new List<string>();
                        foreach (var s in ReservedSeatsInfoForEachBooking)
                        {
                            ReservedSeatsForEachBooking.Add(s.SeatNumber);
                        }
                        bookingViewModel.ConfirmedSeats = ReservedSeatsForEachBooking;


                        var movieInfo = entities.MOVIEs.FirstOrDefault(mhs => mhs.SHOWINFOes.Any(b => b.ShowID == bi.ShowID));

                        var movieVM = BMSDbMethods.MovieInformation(movieInfo.MovieID);
                        if (movieVM == null)
                        {
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error")); ;

                        }
                        else if (movieVM.Any())
                        {
                           bookingWithMovieViewModelList.Add(new BookingWithMovieViewModel { 
                               BookingVM = bookingViewModel, 
                               MovieVM = movieVM.First(),
                               ShowTime = bi.ShowTime,
                               TheatreName=bi.TheatreName,
                               TheatreAddress = bi.TheatreAddress,
                               TheatreContact=bi.TheatreContact,
                               TheatreLocation = bi.TheatreLocation
                           });

                        }
                        else
                        {
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid ID"));

                        }

                    }
                    return Ok(bookingWithMovieViewModelList);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message)); ;



                }
            }
        }


        [HttpGet]
        [Route("api/booking/{BookingID}")]
        public IHttpActionResult GetBookingByID(int BookingID)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                try
                {
                    var bookingEntity = entities.BOOKINGs.FirstOrDefault(b => b.BookingID == BookingID);
                    var paymentEntity = entities.PAYMENTDETAILS.FirstOrDefault(p =>p.BookingID == BookingID);
                    var showInfo = entities.SHOWINFOes.FirstOrDefault(c => c.ShowID == bookingEntity.ShowID);
                    var movieHallInfo = entities.MOVIEHALLs.FirstOrDefault(h => h.MovieHallID == showInfo.MovieHallID);
                    var theatreInfo = entities.THEATRECOMPLEXes.FirstOrDefault(t => t.TheatreID == movieHallInfo.TheatreID);
                    var ReservedSeatsInfoForEachBooking = entities.MOVIEHALLSEATs.Where(mhs => mhs.BOOKINGs.Any(b => b.BookingID == BookingID)).ToList();
                    List<string> ReservedSeatsForEachBooking = new List<string>();
                    foreach (var s in ReservedSeatsInfoForEachBooking)
                    {
                        ReservedSeatsForEachBooking.Add(s.SeatNumber);
                    }
                    BookingViewModel bookingViewModel = new BookingViewModel();
                    bookingViewModel.BookingDate = bookingEntity.BookingDate;
                    bookingViewModel.ShowDate = bookingEntity.ShowDate;
                    bookingViewModel.ShowID = bookingEntity.ShowID;
                    bookingViewModel.CustomerID = bookingEntity.CustomerID;
                    bookingViewModel.PaymentDate = paymentEntity.PaymentDate;
                    bookingViewModel.PaymentAmount = paymentEntity.PaymentAmount;
                    bookingViewModel.TransactionMode = paymentEntity.TransactionMode;
                    bookingViewModel.ConfirmedSeats = ReservedSeatsForEachBooking;
                    var movieInfo = entities.MOVIEs.FirstOrDefault(mhs => mhs.SHOWINFOes.Any(b => b.ShowID == bookingEntity.BookingID));

                    var movieVM = BMSDbMethods.MovieInformation(movieInfo.MovieID);
                    if (movieVM == null)
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error")); ;

                    }
                    else if (movieVM.Any())
                    {
                        return Ok(new BookingWithMovieViewModel { 
                            BookingVM = bookingViewModel, 
                            MovieVM = movieVM.First(),
                            ShowTime = showInfo.ShowTime,
                            TheatreName = theatreInfo.TheatreName,
                            TheatreAddress = theatreInfo.TheatreAddress,
                            TheatreContact = theatreInfo.TheatreContact,
                            TheatreLocation = theatreInfo.TheatreLocation
                        });

                    }
                    else
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid ID"));

                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);

                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message)); ;



                }
            }
        }
    }
}
