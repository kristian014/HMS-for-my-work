using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using HMS.Areas.Dashboard.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public virtual IEnumerable<AccomodationTypeModels>  AccomodationTypeModelses { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<AccomodationType> AccomodationTypes { get; set; }
        public DbSet<AccomodationPackage> AccomodationPackages { get; set; }
     
        public DbSet<Accomodation> Accomodations { get; set; }
     
        public DbSet<Booking> Bookings { get; set; }

        public System.Data.Entity.DbSet<HMS.Areas.Dashboard.ViewModels.AccomodationTypeModels> AccomodationTypeModels { get; set; }
        // public DbSet<Picture> Pictures { get; set; }
        //  public DbSet<AccomodationPackagePicture> AccomodationPackagePictures { get; set; }
        // public DbSet<AccomodationPicture> AccomodationPictures { get; set; }
    }
}