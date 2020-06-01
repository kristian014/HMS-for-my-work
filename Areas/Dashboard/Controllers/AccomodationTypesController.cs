using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HMS.Areas.Dashboard.ViewModels;
using HMS.Models;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationTypesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: Dashboard/AccomodationType
        public ActionResult Index(string searchTerm)
        {
            AccomodationTypeActionModel model2 = new AccomodationTypeActionModel();
            AccomodationTypeModels model = new AccomodationTypeModels();
            model.SearchTerm = searchTerm;
            model.accomodationTypes = model2.SearchAccomodationTypes(searchTerm);
            
            return View(model);
        }

        /// <summary>
        /// This method is return the list of all information in the database
        ///  to acheive this I created a method to get all infomation using Iemermarable list. 
        /// </summary>
        /// <returns></returns>
        //public ActionResult Listing()
        //{
        //    // I created an object model that we can use to access the accomodationtypemodel class 
        //    AccomodationTypeModels  model = new AccomodationTypeModels();
        //    model.accomodationTypes = GetAccomodationTypes();
        //    return PartialView("_Listing", model);
        //}

    /// <summary>
    /// This Action method handles the Create and edit using HTTPGet to get information and uses Httppost to post information and saves it into the database
    /// </summary>
    /// <returns></returns>
        [HttpGet]
        public ActionResult Action(int? ID)
        {
            AccomodationTypeActionModel model = new AccomodationTypeActionModel();
            if (ID.HasValue) // WE ARE editing
            {
                var accomodationtype = model.GetAccomodationTypeByID(ID.Value);
                model.ID = accomodationtype.ID;
                model.Name = accomodationtype.Name;
                model.Description = accomodationtype.Description;
            }
          
            return PartialView("_Action" , model);
        }

        [HttpPost]

        public JsonResult Action(AccomodationTypeActionModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;
            if (model.ID > 0) // edit record
            {
                var accomodationtype = model.GetAccomodationTypeByID(model.ID);
                accomodationtype.Name = model.Name;
                accomodationtype.Description = model.Description;
                result = model.UpdateAccomodationType(accomodationtype);

            }
            else // create record
            {
                AccomodationType accomodationType = new AccomodationType();
                accomodationType.Name = model.Name;
                accomodationType.Description = model.Description;
                result = model.SaveAccomodationType(accomodationType);
            }
           // create an object of accomodation type
         
           // return PartialView("_Action", model);
           if (result)
           {
               json.Data = new {Success = true};
           }
           else
           {
               json.Data = new {Success = false, Message = "Unable to perform action on AccomodationType"};
           }
           return json;
        }

        // my method to get all the list of accomodation from the database
        public IEnumerable<AccomodationType> GetAccomodationTypes()
        {
            //var context = new ApplicationDbContext();
            return context.AccomodationTypes.ToList();
        }


        /// <summary>
        /// This Action method handles the delete using HTTPGet to get information and uses Httppost to post information and remove it into the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(int ID)
        {
            AccomodationTypeActionModel model = new AccomodationTypeActionModel();

            var accomodationtype = model.GetAccomodationTypeByID(ID);
            model.ID = accomodationtype.ID;

            return PartialView("_Delete", model);
        }

        [HttpPost]

        public JsonResult Delete(AccomodationTypeActionModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;
           
                var accomodationtype = model.GetAccomodationTypeByID(model.ID);
                result = model.DeleteAccomodationType(accomodationtype);

           

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