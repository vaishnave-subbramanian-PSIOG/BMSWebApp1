using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BMSDbEntities;

namespace BMSWebApp1.Models
{
    public class BookingWithMovieViewModel
    {
        public BookingViewModel BookingVM;
        public MoviesViewModel MovieVM;
        public System.TimeSpan ShowTime;
        public string TheatreName;
        public string TheatreLocation;
        public string TheatreAddress;
        public string TheatreContact;

    }
}