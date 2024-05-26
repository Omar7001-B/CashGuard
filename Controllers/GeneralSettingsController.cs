using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Timers;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class GeneralSettingsController : Controller
    {

        public IActionResult Index()
        {
            SharedValues.setHover("Settings");
            return View();
        }
        public IActionResult SetDateRange(DateTime DataRangeFrom, DateTime DateRangeTo)
        {
            if (DataRangeFrom != default(DateTime))
            {
                GeneralSettings.DataRangeFrom = DataRangeFrom;
            }
            if (DateRangeTo != default(DateTime))
            {
                GeneralSettings.DataRangeTo = DateRangeTo;
            }
            if (DataRangeFrom > DateRangeTo)
            {
                ModelState.AddModelError("DataRangeFrom", "Date From must be less than Date To");
                return View();
            }

            if (ModelState["DataRangeFrom"] != null)
                ModelState.Remove("DataRangeFrom");

            return View("index", "GeneralSettings");
        }

        public IActionResult SetCurrency(string Currency)
        {
            if (Currency != null)
            {
                GeneralSettings.Currency = Currency;
            }
            return View("index" , "GeneralSettings");
        }

        public IActionResult SetDeleteOldTo(DateTime DeleteOldTo)
        {
            if (DeleteOldTo != default(DateTime)) // 00/00/0001 00:00:00
            {
                GeneralSettings.DeleteOldTo = DeleteOldTo;
                GeneralSettings.SetTransactions();
                GeneralSettings.SetHistoryItems();
                GeneralSettings.UpdateTransactions();
                GeneralSettings.UpdateHistory();
            }
            return View("index", "GeneralSettings");
        }

        // Function to delete old data
        // get user all gategories
        // get all transactions
        // delte all categories and transactions from the user regeiste date till the date he choose
        // update the model lists of the all categories and transactions
        // update the database from the model lists



    }
}
