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
        public IHttpActionResult GetShowsOnDateForMovie ([FromUri] int MovieID, System.DateTime RequestedDate)
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
                                             s.ReservedSeatsCount,
                                             s.UnreservedSeatsCount,
                                             h.MovieHallType,
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
                            showViewModel.ShowID = si.ShowID;
                            showViewModel.ShowTime = si.ShowTime;
                            showViewModel.ReservedSeatsCount = (int)si.ReservedSeatsCount;
                            showViewModel.UnreservedSeatsCount = (int)si.UnreservedSeatsCount;
                            showViewModel.MovieHallType = si.MovieHallType;
                            showViewModel.Price = si.Price;
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

    }
}
