using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HMS.Models
{
    public class Booking
    {
        [Key]
        public int ID { get; set; }

        // public int AccomodationID { get; set; }

        public int AccomodationID { get; set; }
        public Accomodation Accomodation { get; set; }

        public DateTime FromDate { get; set; }

        /// <summary>
        /// No Of Stay Nights
        /// </summary>
        public int Duration { get; set; }
    }
}