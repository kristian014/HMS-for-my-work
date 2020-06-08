using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HMS.Models;

namespace HMS.Areas.Dashboard.ViewModels
{
    public class AccomodationPackagesViewModels
    {
        public IEnumerable<AccomodationPackage> AccomodationPackages { get; set; }
        public int? AccomodationTypeID { get; set; }
        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }
        public string SearchTerm { get; set; }
        public Pager Pager { get; set; }

    }


    public class AccomodationPackageActionModel
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public int ID { get; set; }

        public int AccomodationTypeID { get; set; }
        public AccomodationType AccomodationType { get; set; }

        public string Name { get; set; }
        public int NoOfRoom { get; set; }
        public decimal FeePerNight { get; set; }

        public string PictureIDs { get; set; }

        public IEnumerable<AccomodationType> AccomodationTypes { get; set; }
        // public List<AccomodationPackagePicture> AccomodationPackagePictures { get; set; }


        public bool SaveAccomodationPackage(AccomodationPackage accomodationPackage)
        {

            context.AccomodationPackages.Add(accomodationPackage);
            return context.SaveChanges() > 0;
        }

        public AccomodationPackage GetAccomodationPackageByID(int ID)
        {
            return context.AccomodationPackages.Find(ID);
        }

        public bool UpdateAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            var exitingAccomodationPackage = context.AccomodationPackages.Find(accomodationPackage.ID);
            //context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Modified;
            context.Entry(exitingAccomodationPackage).CurrentValues.SetValues(accomodationPackage);
            return context.SaveChanges() > 0;
        }
        public bool DeleteAccomodationPackage(AccomodationPackage accomodationPackage)
        {
            context.Entry(accomodationPackage).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }

        public IEnumerable<AccomodationPackage> SearchAccomodationPackages(string searchTerm, int? accomodationTypeID, int page, int recordSize)
        {


            var accomodationPackages = context.AccomodationPackages.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackages = accomodationPackages.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            if (accomodationTypeID.HasValue && accomodationTypeID.Value > 0)
            {
                accomodationPackages = accomodationPackages.Where(a => a.AccomodationTypeID == accomodationTypeID.Value);
            }

            var skip = (page - 1) * recordSize;
            //  skip = (1    -  1) = 0 * 3 = 0
            //  skip = (2    -  1) = 1 * 3 = 3
            //  skip = (3    -  1) = 2 * 3 = 6

            return accomodationPackages.OrderBy(x => x.AccomodationTypeID).Skip(skip).Take(recordSize).ToList();

          
        }

        public int SearchAccomodationPackagesCount(string searchTerm, int? accomodationTypeID)
        {
            

            var accomodationPackages = context.AccomodationPackages.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationPackages = accomodationPackages.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (accomodationTypeID.HasValue && accomodationTypeID.Value > 0)
            {
                accomodationPackages = accomodationPackages.Where(a => a.AccomodationTypeID == accomodationTypeID.Value);
            }

            return accomodationPackages.Count();

        }

        public IEnumerable<AccomodationPackage> GetAllAccomodationPackages()
        {

            return context.AccomodationPackages.ToList();
        }


    }
}