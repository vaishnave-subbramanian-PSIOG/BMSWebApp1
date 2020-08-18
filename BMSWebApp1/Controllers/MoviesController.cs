using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BMSDbEntities;
using BMSWebApp1.Helper;
using BMSWebApp1.Models;

namespace BMSWebApp1.Controllers
{

    public class MoviesController : ApiController
    {
        [HttpGet]
        [Route("api/movies")]
        public IHttpActionResult GetMovies()
        {
            try
            {

                using (BMSApplicationEntities entities = new BMSApplicationEntities())
                {
                    List<MoviesViewModel> listMoviesViewModel = new List<MoviesViewModel>();

                    List<MOVIE> AllMovies = entities.MOVIEs.ToList();
                    List<DIRECTOR> AllDirectors = entities.DIRECTORs.ToList();
                    List<GENRE> AllGenres = entities.GENREs.ToList();

                    var moviesInfo = (from m in AllMovies
                                      join d in AllDirectors on m.DirectorID equals d.DirectorID
                                      join g in AllGenres on m.GenreID equals g.GenreID
                                      select new
                                      {
                                          MovieID = m.MovieID,
                                          MovieName = m.MovieName,
                                          Synopsis = m.Synopsis,
                                          TrailerURL = m.TrailerURL,
                                          PosterURL = m.PosterURL,
                                          DirectorName = d.DirectorName,
                                          GenreName = g.GenreName,
                                      });
                    foreach (var mi in moviesInfo)
                    {
                        MoviesViewModel moviesViewModel = new MoviesViewModel();
                        moviesViewModel.ID = mi.MovieID;
                        moviesViewModel.Name = mi.MovieName;
                        moviesViewModel.Synopsis = mi.Synopsis;
                        moviesViewModel.TrailerURL = mi.TrailerURL;
                        moviesViewModel.PosterURL = mi.PosterURL;
                        moviesViewModel.Director = mi.DirectorName;
                        moviesViewModel.Genre = mi.GenreName;
                        List<string> CastList = new List<string>();
                        var CastsInfo = entities.CASTINGs.Where(c => c.MOVIEs.Any(m => m.MovieID == mi.MovieID)).ToList();
                        foreach (var c in CastsInfo)
                        {
                            CastList.Add(c.ActorName);
                        }
                        moviesViewModel.Cast = CastList;
                        listMoviesViewModel.Add(moviesViewModel);
                    }
                    return Ok(listMoviesViewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message)); ;

            }
        }
        [HttpGet]
        [Route("api/movies/{id?}")]
        public IHttpActionResult GetMovies(int id)
        {

                    var movieVM = BMSDbMethods.MovieInformation(id);
                    if (movieVM==null)
                    {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server Error")); ;

                }
                else if(movieVM.Any())
                    {
                    return Ok(movieVM);

                    }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid ID")); 

            }
        }

        [HttpPost]
        [Route("api/movies")]
        public HttpResponseMessage PostMovies([FromBody] MOVIE movie)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                try
                {
                    entities.MOVIEs.Add(movie);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, movie);
                    message.Headers.Location = new Uri(Request.RequestUri + movie.MovieID.ToString());
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
        [Route("api/movies/{id?}")]
        public void DeleteMovies(int id)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                entities.MOVIEs.Remove(entities.MOVIEs.FirstOrDefault(c => c.MovieID == id));
                entities.SaveChanges();
            }
        }

        [HttpPut]
        [Route("api/movies/{id?}")]
        public void PutMovies(int id, [FromBody] MOVIE movie)
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                var entity = entities.MOVIEs.FirstOrDefault(c => c.MovieID == id);
                entity.MovieName = movie.MovieName;
                entity.Synopsis = movie.Synopsis;
                entity.GenreID = movie.GenreID;
                entity.DirectorID = movie.DirectorID;
                entities.SaveChanges();
            }
        }
    }
}
