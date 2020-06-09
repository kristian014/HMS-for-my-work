using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HMS.Areas.Dashboard.ViewModels;
using HMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace HMS.Areas.Dashboard.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
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

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        public RoleController()
        {
        }

        public RoleController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }





        // GET: Dashboard/Users
        public ActionResult Index(string searchTerm, int? page)
        {


            int recordSize = 6;
            page = page ?? 1;



            RolesListingModel model = new RolesListingModel();

            model.SearchTerm = searchTerm;

            model.Roles = SearchRoles(searchTerm,  page.Value, recordSize);

            var totalRecords = SearchRolesCount(searchTerm);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);


        }


        public IEnumerable<IdentityRole> SearchRoles(string searchTerm, int page, int recordSize)
        {


            var roles = RoleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }


            var skip = (page - 1) * recordSize;

            return roles.OrderBy(x => x.Name).Skip(skip).Take(recordSize).ToList();
           
        }

        public int SearchRolesCount(string searchTerm)
        {

            var roles = RoleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                roles = roles.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            return roles.Count();
        }


        [HttpGet]
        public async Task<ActionResult> Action(string ID)
        {
            RoleActionModel model = new RoleActionModel();
            // AccomodationPackageActionModel model2 = new AccomodationPackageActionModel();

            if (!string.IsNullOrEmpty(ID)) // we are trying to edit
            {
                var roles = await RoleManager.FindByIdAsync(ID);
                model.ID = roles.Id;
                model.Name = roles.Name;

            }

            // model. = model2.GetAllAccomodationPackages();
            return PartialView("_Action", model);
        }

        [HttpPost]
        public async Task<JsonResult> Action(RoleActionModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;
            if (!string.IsNullOrEmpty(model.ID)) // we are trying to edit
            {
                var roles = await RoleManager.FindByIdAsync(model.ID);
                roles.Name = model.Name;
                result = await RoleManager.UpdateAsync(roles);
                //    json.Data = new { Success = result.Succeeded, Message = string.Join(" , " , result.Errors) };
            }
            else // we are creating
            {
                var role = new IdentityRole();
                role.Name = model.Name;
                result = await RoleManager.CreateAsync(role);
            }

            json.Data = new { Success = result.Succeeded, Message = string.Join(" , ", result.Errors) };

            return json;
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string ID)
        {
            RoleActionModel model = new RoleActionModel();
            var role = await RoleManager.FindByIdAsync(ID);


            model.ID = role.Id;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(RoleActionModel model)
        {
            JsonResult json = new JsonResult();
            IdentityResult result = null;

            if (!string.IsNullOrEmpty(model.ID)) // we are trying to delete
            {
                var role = await RoleManager.FindByIdAsync(model.ID);

                result = await RoleManager.DeleteAsync(role);
                json.Data = new { Success = result.Succeeded, Message = string.Join(" , ", result.Errors) };
            }
            else
            {
                json.Data = new { Success = false, Message = "Invalid Role" };
            }

            return json;
        }

    }
}