using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HMS.Areas.Dashboard.ViewModels;
using HMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace HMS.Areas.Dashboard.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
      
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public RolesController()
        {
        }

        public RolesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

       



        // GET: Dashboard/Users
        public ActionResult Index(string searchTerm, string roleID, int? page)
        {


            int recordSize = 1;
            page = page ?? 1;

      
           
           UsersViewModel model = new UsersViewModel();

            model.SearchTerm = searchTerm;
            model.RoleID = roleID;
           // model.Roles = model2.GetAllAccomodationPackages();
         //  model.Users = UserManager.Users;

           model.Users = SearchUsers(searchTerm, roleID, recordSize, page.Value);

           var totalRecords = SearchusersCount(searchTerm, roleID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);


        }


        public IEnumerable<ApplicationUser> SearchUsers(string searchTerm, string roleID, int page, int recordSize)
        {


            var users = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }


            if (!string.IsNullOrEmpty(roleID))
            {
              //  users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }
           
            var skip = (page - 1) * recordSize;

            return users.OrderBy(x => x.Email).Skip(skip).Take(recordSize).ToList();
        }

        public int SearchusersCount(string searchTerm, string roleID)
        {


            var users = UserManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(a => a.Email.ToLower().Contains(searchTerm.ToLower()));
            }

            if (!string.IsNullOrEmpty(roleID))
            {
                //!string.IsNullOrEmpty(searchTerm) = accomodations.Where(a => a.AccomodationPackageID == accomodationPackageID.Value);
            }

            return users.Count();
        }


        [HttpGet]
        public async Task<ActionResult> Action(string ID)
        {
            UsersActionModel model = new UsersActionModel();
           // AccomodationPackageActionModel model2 = new AccomodationPackageActionModel();

            if (!string.IsNullOrEmpty(ID)) // we are trying to edit
            {
                var user = await UserManager.FindByIdAsync(ID);
                model.ID = user.Id;
                model.FullName = user.FullName;
                model.Email = user.Email;
                model.Username = user.UserName;
                model.Country = user.Country;
                model.City = user.City;
                model.Address = user.Address;
            }

           // model. = model2.GetAllAccomodationPackages();
            return PartialView("_Action", model);
        }

        [HttpPost]
        public async Task<JsonResult> Action(UsersActionModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null; 
            if (!string.IsNullOrEmpty(model.ID)) // we are trying to edit
            {
                var user = await UserManager.FindByIdAsync(model.ID);
               
                user.FullName = model.FullName;
                user.City = model.City;
                user.Address = model.Address;
                user.UserName = model.Username;
                user.Country = model.Country;
                user.Email = model.Email;

                 result = await UserManager.UpdateAsync(user);
            //    json.Data = new { Success = result.Succeeded, Message = string.Join(" , " , result.Errors) };
            }
            else // we are creating
            {
                var user = new ApplicationUser();
                user.FullName = model.FullName;
                user.City = model.City;
                user.Address = model.Address;
                user.UserName = model.Username;
                user.Country = model.Country;
                user.Email = model.Email;
                result = await UserManager.CreateAsync(user);
            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(" , ", result.Errors) };

            return json;
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string ID)
        {
            UsersActionModel model = new UsersActionModel();
            var user = await UserManager.FindByIdAsync(ID);


            model.ID = user.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(UsersActionModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID)) // we are trying to delete
            {
                var user = await UserManager.FindByIdAsync(model.ID);

                result = await UserManager.DeleteAsync(user);
                json.Data = new { Success = result.Succeeded, Message = string.Join(" , " , result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid User" };
            }
           
            return json;
        }
    }
}