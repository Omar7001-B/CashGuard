using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Timers;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class GeneralSettingsController : Controller
    {
        // i can send class as binding butt the class is static ):
        public IActionResult Index(DateTime DataRangeFrom, DateTime DateRangeTo, string Currency, DateTime DeleteOldTo)
        {
            if (DataRangeFrom != default(DateTime))
            {
                GeneralSettings.DataRangeFrom = DataRangeFrom;
            }
            if (DateRangeTo != default(DateTime))
            {
                GeneralSettings.DataRangeTo = DateRangeTo;
            }
            if (GeneralSettings.Currency == null)
            {
                GeneralSettings.Currency = Currency;
            }
            if (DeleteOldTo != default(DateTime))
            {
                GeneralSettings.DeleteOldTo = DeleteOldTo;
            }


            if (DataRangeFrom > DateRangeTo)
            {

                ModelState.AddModelError("DataRangeFrom", "Date From must be less than Date To");
                return View();
            }
            ModelState.Remove("DataRangeFrom");
            return View();
        }

        // Function to delete old data
        // get user all gategories
        // get all transactions
        // delte all categories and transactions from the user regeiste date till the date he choose
        // update the model lists of the all categories and transactions
        // update the database from the model lists



    }
}
