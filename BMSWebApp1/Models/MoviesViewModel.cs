using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BMSDbEntities;

namespace BMSWebApp1.Models
{
    public class MoviesViewModel
    { public int ID;
      public string Name;
      public string Synopsis;
      public string TrailerURL;
      public string PosterURL;
      public string Director;
      public string Genre;
      public List<CASTING> Cast;
    }
}