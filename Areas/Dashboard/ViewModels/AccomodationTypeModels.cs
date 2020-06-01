using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HMS.Models;

namespace HMS.Areas.Dashboard.ViewModels
{
   
    public class AccomodationTypeModels
    {
        [Key]
        public int ID { get; set; }
        public IEnumerable<AccomodationType> accomodationTypes { get; set; }
        public string SearchTerm { get; set; }

       
    }

    public class AccomodationTypeActionModel
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// This method saves information into the database
        /// </summary>
        /// <param name="accomodationType"></param>
        /// <returns></returns>
        public bool SaveAccomodationType(AccomodationType accomodationType)
        {

            context.AccomodationTypes.Add(accomodationType);
            return context.SaveChanges() > 0;
        }

        public AccomodationType GetAccomodationTypeByID(int ID)
        {
            return context.AccomodationTypes.Find(ID);
        }

        public bool UpdateAccomodationType(AccomodationType accomodationType)
        {
            context.Entry(accomodationType).State = System.Data.Entity.EntityState.Modified;
            return context.SaveChanges() > 0;
        }
        public bool DeleteAccomodationType(AccomodationType accomodationType)
        {
            context.Entry(accomodationType).State = System.Data.Entity.EntityState.Deleted;
            return context.SaveChanges() > 0;
        }

        public IEnumerable<AccomodationType> SearchAccomodationTypes(string searchTerm)
        {
          

            var accomodationTypes = context.AccomodationTypes.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                accomodationTypes = accomodationTypes.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return accomodationTypes.ToList();
        }

        public IEnumerable<AccomodationType> GetAllAccomodationTypes()
        {
            return context.AccomodationTypes.ToList();
        }


    }


}