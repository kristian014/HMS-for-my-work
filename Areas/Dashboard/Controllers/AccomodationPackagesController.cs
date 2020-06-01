using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HMS.Areas.Dashboard.ViewModels;
using HMS.Models;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationPackagesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Dashboard/AccomodationPackages
        public ActionResult Index(string searchTerm, int? accomodationTypeID, int? page)
        {
            int recordSize = 3;
            page = page ?? 1;
            AccomodationPackageActionModel model3 = new AccomodationPackageActionModel();
            AccomodationPackagesViewModels model2 = new AccomodationPackagesViewModels();
            AccomodationTypeActionModel model = new AccomodationTypeActionModel();
            model2.SearchTerm = searchTerm;
            model2.AccomodationTypes = model.GetAllAccomodationTypes();
            model2.AccomodationPackages = model3.SearchAccomodationPackages(searchTerm, accomodationTypeID, page.Value, recordSize);
           var totalRecords = model3.SearchAccomodationPackagesCount(searchTerm, accomodationTypeID);
           model2.Pager = new Pager(9, page, recordSize);
            return View(model2);
         
        }


        [HttpGet]
        public ActionResult Action(int? ID)
        {
            AccomodationPackageActionModel model = new AccomodationPackageActionModel();
            AccomodationTypeActionModel model2 = new AccomodationTypeActionModel();
            if (ID.HasValue) // WE ARE editing
            {
                var accomodationPackage = model.GetAccomodationPackageByID(ID.Value);
                model.ID = accomodationPackage.ID;
                model.AccomodationTypeID = accomodationPackage.AccomodationTypeID;
                model.Name = accomodationPackage.Name;
                model.NoOfRoom = accomodationPackage.NoOfRoom;
                model.FeePerNight = accomodationPackage.FeePerNight;

            }
            model.AccomodationTypes = model2.GetAllAccomodationTypes();

            return PartialView("_Action", model);
        }

        [HttpPost]

        public JsonResult Action( AccomodationPackageActionModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;
            if (model.ID > 0) // edit record
            {
                var accomodationPackage = model.GetAccomodationPackageByID(model.ID);
                accomodationPackage.AccomodationTypeID = model.AccomodationTypeID;
                accomodationPackage.Name = model.Name;
                accomodationPackage.AccomodationTypeID = model.AccomodationTypeID;
                accomodationPackage.FeePerNight = model.FeePerNight;
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                result = model.UpdateAccomodationPackage(accomodationPackage);

            }
            else // create record
            {
                AccomodationPackage accomodationPackage = new AccomodationPackage();
                accomodationPackage.AccomodationTypeID = model.AccomodationTypeID;
                accomodationPackage.Name = model.Name;
             
                accomodationPackage.NoOfRoom = model.NoOfRoom;
                accomodationPackage.FeePerNight = model.FeePerNight;
                result = model.SaveAccomodationPackage(accomodationPackage);
                //accomodationType.Description = model.Description;
                //result = model.SaveAccomodationType(accomodationType);
            }
            // create an object of accomodation type

            // return PartialView("_Action", model);
            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on AccomodationType" };
            }
            return json;
        }

        // my method to get all the list of accomodation from the database
        //public IEnumerable<AccomodationType> GetAccomodationTypes()
        //{
        //    //var context = new ApplicationDbContext();
        //    return context.AccomodationTypes.ToList();
        //}





        [HttpGet]
        public ActionResult Delete(int? ID)
        {
            AccomodationPackageActionModel model = new AccomodationPackageActionModel();
            AccomodationTypeActionModel model2 = new AccomodationTypeActionModel();
            var accomodationPackage = model.GetAccomodationPackageByID(ID.Value);
              model.ID = accomodationPackage.ID;

              return PartialView("_Delete", model);
        }

        [HttpPost]

        public JsonResult Delete(AccomodationPackageActionModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;
          
                var accomodationPackage = model.GetAccomodationPackageByID(model.ID);
               
                result = model.DeleteAccomodationPackage(accomodationPackage);


            // return PartialView("_Action", model);
            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on AccomodationType" };
            }
            return json;
        }

    }
}
