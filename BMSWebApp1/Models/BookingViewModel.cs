using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSWebApp1.Models
{
    public class BookingViewModel
    {
        public System.DateTime BookingDate { get; set; }
        public int CustomerID { get; set; }
        public int ShowID { get; set; }
        public System.DateTime ShowDate { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string TransactionMode { get; set; }

        public List<string> ConfirmedSeats;

    }
}