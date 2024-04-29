using Microsoft.AspNetCore.Mvc;
using System.Timers;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class GeneralSettings : Controller
    {
        // i can send class as binding butt the class is static ):
        public IActionResult Index(DateTime DataRangeFrom, DateTime DateRangeTo, string Currency, DateTime DeleteOldTo)
        {
            if (DataRangeFrom != default(DateTime))
            {
                SharedValues.DataRangeFrom = DataRangeFrom;
            }
            if ( DateRangeTo != default(DateTime))
            {
                SharedValues.DataRangeTo = DateRangeTo;
            }
            if (SharedValues.Currency == null)
            {
                SharedValues.Currency = Currency;
            }
            if (DeleteOldTo != default(DateTime))
            {
                SharedValues.DeleteOldTo = DeleteOldTo;
            }
            return View();
        }
    }
}
