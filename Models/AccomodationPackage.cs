using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HMS.Models
{
    public class AccomodationPackage
    {

        [Key]
        public int ID { get; set; }

        public int AccomodationTypeID { get; set; }

        public AccomodationType AccomodationType { get; set; }


        public string Name { get; set; }

        public int NoOfRoom { get; set; }
        public decimal FeePerNight { get; set; }
    }
}