using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BMSWebApp1.Models
{
    public class ShowViewModel
    {
        public int ShowID;
        public System.TimeSpan ShowTime;
        public int ReservedSeatsCount;
        public int UnreservedSeatsCount;
        public string MovieHallType;
        public decimal Price;
        public string TheatreName;
        public string TheatreLocation;
        public string TheatreAddress;
        public string TheatreContact;
    }
}