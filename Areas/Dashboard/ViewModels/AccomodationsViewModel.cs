using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HMS.Models;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccomodationsViewModel
    {
        public IEnumerable<Accomodation> Accomodations { get; set; }

        public int? AccomodationPackageID { get; set; }
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
        public string SearchTerm { get; set; }

        public Pager Pager { get; set; }
    }

    public class AccomodationActionModel
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public int ID { get; set; }

        public int AccomodationPackageID { get; set; }
        public AccomodationPackage AccomodationPackage { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }

        public IEnumerable<Accomodation> GetAllAccomodationTypes()
        {
            return context.Accomodations.ToList();
        }

        public IEnumerable<Accomodation> GetAllAccomodationsByAccomodationPackage(int accomodationPackageID)
        {
            return context.Accomodations.Where(x => x.AccomodationPackageID == accomodationPackageID).ToList();
        }

        public IEnumerable<Accomodation> SearchAccomodations(string searchTerm, int? accomodationPackageID, int page, int recordSize)
        {
           

            var accomodations = context.Accomodations.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationPackageID.HasValue && accomodationPackageID.Value > 0)
            {
                accomodations = accomodations.Where(a => a.AccomodationPackageID == accomodationPackageID.Value);
            }

            var skip = (page - 1) * recordSize;

            return accomodations.OrderBy(x => x.AccomodationPackageID).Skip(skip).Take(recordSize).ToList();
        }

        public int SearchAccomodationsCount(string searchTerm, int? accomodationPackageID)
        {
          

            var accomodations = context.Accomodations.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodations = accomodations.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationPackageID.HasValue && accomodationPackageID.Value > 0)
            {
                accomodations = accomodations.Where(a => a.AccomodationPackageID == accomodationPackageID.Value);
            }

            return accomodations.Count();
        }

        public Accomodation GetAccomodationByID(int ID)
        {
          
            {
                return context.Accomodations.Find(ID);
            }
        }

        public bool SaveAccomodation(Accomodation accomodation)
        {
           

            context.Accomodations.Add(accomodation);

            return context.SaveChanges() > 0;
        }

        public bool UpdateAccomodation(Accomodation accomodation)
        {
          

            context.Entry(accomodation).State = System.Data.Entity.EntityState.Modified;

            return context.SaveChanges() > 0;
        }

        public bool DeleteAccomodation(Accomodation accomodation)
        {
          

            context.Entry(accomodation).State = System.Data.Entity.EntityState.Deleted;

            return context.SaveChanges() > 0;
        }

    }

}



