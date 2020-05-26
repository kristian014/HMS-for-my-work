using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HMS.Models
{
    public class Accomodation
    {
        [Key]
        public int ID { get; set; }

        public int AccomodationPackageID { get; set; }

        public AccomodationPackage AccomodationPackage { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}