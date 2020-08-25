using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BMSDbEntities;
using BMSWebApp1.Helper;
using BMSWebApp1.Models;

namespace BMSWebApp1.Controllers
{
    public class ShowController : ApiController
    {
        [HttpGet]
        [Route("api/show")]
        public IHttpActionResult GetShowsOnDateForMovie ([FromUri] int MovieID, DateTime RequestedDate)
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())
                {

                    var AllShowsForMovie = entities.SHOWINFOes.Where(c => c.MOVIEs.Any(m => m.MovieID == MovieID)).ToList(); ;
                    var AllShowsOnDateForMovie = AllShowsForMovie.FindAll(c => c.FromDate <= RequestedDate && c.ToDate >= RequestedDate);
                    if (AllShowsOnDateForMovie.Any())
                    {
                        List<MOVIEHALL> AllMovieHalls = entities.MOVIEHALLs.ToList();
                        List<THEATRECOMPLEX> AllTheatres = entities.THEATRECOMPLEXes.ToList();

                        var showsInfo = (from s in AllShowsOnDateForMovie
                                         join h in AllMovieHalls on s.MovieHallID equals h.MovieHallID
                                         join th in AllTheatres on h.TheatreID equals th.TheatreID
                                         select new
                                         {
                                             s.ShowID,
                                             s.ShowTime,
                                             h.MovieHallType,
                                             h.Capacity,
                                             h.Price,
                                             th.TheatreName,
                                             th.TheatreLocation,
                                             th.TheatreAddress,
                                             th.TheatreContact,

                                         });
                        List<ShowViewModel> listShowViewModel = new List<ShowViewModel>();

                        foreach (var si in showsInfo)
                        {
                            ShowViewModel showViewModel = new ShowViewModel();

                            //Reserved Seats
                            var AllBookingsInfoForShow = entities.BOOKINGs.Where(b => b.ShowID == si.ShowID && b.ShowDate == RequestedDate).ToList();

                            showViewModel.ReservedSeatsCount =0;
                            foreach (var bookingInfo in AllBookingsInfoForShow)
                            {
                                var reservedSeatsForEachDateCount = entities.MOVIEHALLSEATs.Where(c => c.BOOKINGs.Any(m => m.BookingID == bookingInfo.BookingID)).ToList().Count;
                                showViewModel.ReservedSeatsCount += reservedSeatsForEachDateCount;

                            }

                            showViewModel.ShowID = si.ShowID;
                            showViewModel.ShowTime = si.ShowTime;
                            showViewModel.MovieHallType = si.MovieHallType;
                            showViewModel.Price = si.Price;
                            showViewModel.UnreservedSeatsCount = si.Capacity- showViewModel.ReservedSeatsCount;
                            showViewModel.TheatreName = si.TheatreName;
                            showViewModel.TheatreLocation = si.TheatreLocation;
                            showViewModel.TheatreAddress = si.TheatreAddress;
                            showViewModel.TheatreContact = si.TheatreContact;
                            listShowViewModel.Add(showViewModel);

                        }
                        return Ok(listShowViewModel);


                    }
                    else
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not Found")); ;

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message)); ;

            }
        }

        [HttpGet]
        [Route("api/seats")]
        public IHttpActionResult GetSeatsInfoForShow([FromUri] int ShowID, DateTime RequestedDate)
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())
                {
                    var showEntity = entities.SHOWINFOes.FirstOrDefault(c => c.ShowID == ShowID);
                    if (showEntity != null) {
                        var AllSeatsInfoForShow = entities.MOVIEHALLSEATs.Where(c => c.MovieHallID == showEntity.MovieHallID).ToList();
                        List<string> AllSeatsForShow = new List<string>();
                        foreach (var seatInfo in AllSeatsInfoForShow)
                        {
                            AllSeatsForShow.Add(seatInfo.SeatNumber);
                        }

                        //Reserved Seats
                        var AllBookingsInfoForShow = entities.BOOKINGs.Where(b => b.ShowID == ShowID && b.ShowDate== RequestedDate).ToList();
                        List<string> AllReservedSeatsForShow = new List<string>();
                        foreach (var bookingInfo in AllBookingsInfoForShow)
                        {
                            var reservedSeatsForEachDate = entities.MOVIEHALLSEATs.Where(c => c.BOOKINGs.Any(m => m.BookingID == bookingInfo.BookingID)).ToList();
                            foreach (var seatInfo in reservedSeatsForEachDate)
                            {
                                AllReservedSeatsForShow.Add(seatInfo.SeatNumber);
                            }
                        }
                        SeatsViewModel seatsViewModel = new SeatsViewModel();
                        seatsViewModel.Seats = AllSeatsForShow;
                        seatsViewModel.ReservedSeats = AllReservedSeatsForShow;
                        return Ok(seatsViewModel);
                    }
                    else {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Show Not Found")); ;
                    }
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
