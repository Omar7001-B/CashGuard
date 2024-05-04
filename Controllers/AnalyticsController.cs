using Microsoft.AspNetCore.Mvc;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly Appdbcontxt _context;

        public AnalyticsController(Appdbcontxt context)
        {
            _context = context;
        }

        List<Transaction> GetUserTransactions(string type)
        {
            List<Transaction> transactions;
            if (type == "Income")
                transactions = _context.Transactions.Where(t => t.UserId == SharedValues.CurUser.Id && t.TransactionType == "Income").ToList();
            else if (type == "Expense")
                transactions = _context.Transactions.Where(t => t.UserId == SharedValues.CurUser.Id && t.TransactionType == "Expense").ToList();
            else
                transactions = _context.Transactions.Where(t => t.UserId == SharedValues.CurUser.Id).ToList();

            foreach (var transaction in transactions) _context.Entry(transaction).Reference(t => t.Category).Load();
            return transactions;
        }


        public IActionResult Index()
        {
            // var transactions = _context.Transactions.ToList();
            var transactions = GetUserTransactions("All");
            var expensesTotal = GetUserTransactions("Expense").Sum(t => t.Amount);
            var incomeTotal = GetUserTransactions("Income").Sum(t => t.Amount);
            var categories = _context.Categories.Where(c => c.UserId == SharedValues.CurUser.Id).ToList();

            ViewBag.ExpensesTotal = expensesTotal;
            ViewBag.IncomeTotal = incomeTotal;

            return View(transactions);
        }
    }
}
