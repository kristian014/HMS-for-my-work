using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HMS.Areas.Dashboard.ViewModels;
using HMS.Models;

namespace HMS.Areas.Dashboard.Controllers
{
    public class AccomodationsController : Controller
    {
        // GET: Dashboard/Accomodations
        public ActionResult Index(string searchTerm, int? accomodationPackageID, int? page)
        {
           

            int recordSize = 3;
            page = page ?? 1;
            AccomodationActionModel model3 = new AccomodationActionModel();
            AccomodationPackageActionModel model2 = new AccomodationPackageActionModel();
            AccomodationsViewModel model = new AccomodationsViewModel();

            model.SearchTerm = searchTerm;
            model.AccomodationPackageID = accomodationPackageID;
            model.AccomodationPackages = model2.GetAllAccomodationPackages();
            model.Accomodations = model3.SearchAccomodations(searchTerm, accomodationPackageID, page.Value, recordSize);
            var totalRecords = model3.SearchAccomodationsCount(searchTerm, accomodationPackageID);
            model.Pager = new Pager(totalRecords, page, recordSize);

            return View(model);

           
        }

        [HttpGet]
        public ActionResult Action(int? ID)
        {
            AccomodationActionModel model = new AccomodationActionModel();
            AccomodationPackageActionModel model2 = new AccomodationPackageActionModel();

            if (ID.HasValue)
            {
                var accomodation = model.GetAccomodationByID(ID.Value);
                model.ID = accomodation.ID;
                model.AccomodationPackageID = accomodation.AccomodationPackageID;
                model.Name = accomodation.Name;
                model.Description = accomodation.Description;
            }

            model.AccomodationPackages = model2.GetAllAccomodationPackages();
            return PartialView("_Action" , model);
        }

        [HttpPost]
        public JsonResult Action(AccomodationActionModel model)
        {
            JsonResult json = new JsonResult();
            var result = false;
            if (model.ID > 0) // we are trying to edit
            {
                var accomodation = model.GetAccomodationByID(model.ID);
                accomodation.AccomodationPackageID = model.AccomodationPackageID;
                accomodation.Name = model.Name;
                accomodation.Description = model.Description;
                result = model.UpdateAccomodation(accomodation);
            }
            else
            {
                Accomodation accomodation = new Accomodation();
                accomodation.AccomodationPackageID = model.AccomodationPackageID;
                accomodation.Name = model.Name;
                accomodation.Description = model.Description;
                result = model.SaveAccomodation(accomodation);
            }

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Accomodation." };
            }

            return json;
        }

        [HttpGet]
        public ActionResult Delete(int ID)
        {
            AccomodationActionModel model = new AccomodationActionModel();

            var accomodation = model.GetAccomodationByID(ID);

            model.ID = accomodation.ID;

            return PartialView("_Delete", model);
        }

        [HttpPost]
        public JsonResult Delete(AccomodationActionModel model)
        {
            JsonResult json = new JsonResult();

            var result = false;

            var accomodation = model.GetAccomodationByID(model.ID);

            result = model.DeleteAccomodation(accomodation);

            if (result)
            {
                json.Data = new { Success = true };
            }
            else
            {
                json.Data = new { Success = false, Message = "Unable to perform action on Accomodation." };
            }

            return json;
        }

    }
}