using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class TransactionController : Controller
    {
        private readonly Appdbcontxt _context;

        public TransactionController(Appdbcontxt context) { _context = context; }


        // get transactions based on type of transaction
        List<Transaction> GetUserTransactions(string type)
        {
            List<Transaction> transactions;
            if (type == "Income")
                transactions = _context.Transactions .Where(t => t.UserId == SharedValues.CurUser.Id && t.TransactionType == "Income") .ToList(); 
            else if(type == "Expense")
                transactions = _context.Transactions .Where(t => t.UserId == SharedValues.CurUser.Id && t.TransactionType == "Expense") .ToList();
            else
                transactions = _context.Transactions.Where(t => t.UserId == SharedValues.CurUser.Id).ToList();

            foreach (var transaction in transactions) _context.Entry(transaction).Reference(t => t.Category).Load();
            return transactions;
        }


        // get all user transactions
        List<Transaction> GetUserTransactions()
        {
            List<Transaction> transactions;
            transactions = _context.Transactions
                                 .Where(t => t.UserId == SharedValues.CurUser.Id)
                                 .OrderByDescending(t => t.Timestamp)
                                 .ToList();

            foreach (var transaction in transactions) _context.Entry(transaction).Reference(t => t.Category).Load();
            return transactions;
        }

        List<Category> GetUserCategories()
        {
            List<Category> categories = _context.Categories.Where(c => c.UserId == SharedValues.CurUser.Id).ToList();
            return categories;



        }

        public static object CreateTransactionPierChart(List<Transaction> transactions, string type)
        {
            var DataPoints = transactions.Where(t => t.TransactionType == type).GroupBy(t => t.CategoryId).Select(group => new
            {
                y = group.Sum(t => t.Amount),
                label = group.First().Category?.Name
            }).ToList();

            return new
            {
                animationEnabled = true,
                title = new
                {
                    text = type + " Summary"
                },
                data = new[]
                {
                    new
                    {
                        type = "doughnut",
                        dataPoints = DataPoints
                    }
                }

            };
        }


        public void ExpensesByCategory()
        {
            var chartConfig = CreateTransactionPierChart(GetUserTransactions(), "Expense");
            ViewBag.ChartConfig = JsonConvert.SerializeObject(chartConfig);
        }

        public void CreateLineChartIncomeExpense()
        {
            // Retrieve list of transactions from the database
            var transactions = _context.Transactions.ToList();

            // Separate transactions into income and expense
            var incomeTransactions = transactions.Where(t => t.UserId == SharedValues.CurUser.Id && t.TransactionType == "Income").ToList();
            var expenseTransactions = transactions.Where(t => t.UserId == SharedValues.CurUser.Id && t.TransactionType == "Expense").ToList();

            // Prepare data points for income line
            var incomeDataPoints = incomeTransactions.Select(transaction => new
            {
                x = transaction.Timestamp,
                y = transaction.Amount,
                indexLabel = _context.Categories.Find(transaction.CategoryId)?.Name
            }).ToList();

            // Prepare data points for expense line
            var expenseDataPoints = expenseTransactions.Select(transaction => new
            {
                x = transaction.Timestamp,
                y = transaction.Amount,
                indexLabel = _context.Categories.Find(transaction.CategoryId)?.Name
            }).ToList();

            // Sort Them Based on Date
            incomeDataPoints = incomeDataPoints.OrderBy(d => d.x).ToList();
            expenseDataPoints = expenseDataPoints.OrderBy(d => d.x).ToList();

            incomeDataPoints = incomeDataPoints.Skip(Math.Max(0, incomeDataPoints.Count() - 10)).ToList();
            expenseDataPoints = expenseDataPoints.Skip(Math.Max(0, expenseDataPoints.Count() - 10)).ToList();

            string intervalType = "year";

            if(incomeDataPoints.Count != 0 && expenseDataPoints.Count != 0)
            {
                var maxDate = incomeDataPoints.Max(d => d.x);
                maxDate = maxDate > expenseDataPoints.Max(d => d.x) ? maxDate : expenseDataPoints.Max(d => d.x);

                var minDate = incomeDataPoints.Min(d => d.x);
                minDate = minDate < expenseDataPoints.Min(d => d.x) ? minDate : expenseDataPoints.Min(d => d.x);

                if ((maxDate - minDate).TotalDays < 30)
                    intervalType = "day";
                else if ((maxDate - minDate).TotalDays < 365)
                    intervalType = "month";
                else
                    intervalType = "year";
            }


            var chartConfig = new
            {
                theme = "light2",
                animationEnabled = true,
                title = new { text = "Recent Transactions" },
                axisX = new
                {
                    //title = "Date",
                    interval = 1,
                    intervalType = intervalType,
                    //valueFormatString = "MMM"
                },
                axisY = new
                {
                    //title = "Amount",
                    //includeZero = true,
                    //valueFormatString = "$#0"
                },
                data = new[] {
                new {
                    type = "line",
                    name = "Income",
                    markerSize = 12,
                    //xValueFormatString = "MMM, YYYY",
                    //yValueFormatString = "$###.#",
                    dataPoints = incomeDataPoints
                },
                new {
                    type = "line",
                    name = "Expense",
                    markerSize = 12,
                    //xValueFormatString = "MMM, YYYY",
                    //yValueFormatString = "$###.#",
                    dataPoints = expenseDataPoints
                }
            }
            };

            ViewBag.ChartConfig2 = JsonConvert.SerializeObject(chartConfig);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///Elaraby COdes OF MErge VIews

        public IActionResult TransactionAddition()
        {
            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
            ViewBag.Transactions = GetUserTransactions();
            return View("TransactionAddition" , new Transaction());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TransactionAddition(Transaction transactionToCreate)
        {
            transactionToCreate.UserId = SharedValues.CurUser.Id;

            if (transactionToCreate.TransactionType == "Income")
            {
                if (ModelState.IsValid)
                {
                    // check if exist update 
                    if (transactionToCreate.Id != 0)
                    {
                        _context.Transactions.Update(transactionToCreate);
                        _context.SaveChanges();
                        LogToHistory("Income Update", $"Income '{transactionToCreate.Title}' updated.");
                        return RedirectToAction("TransactionAddition", "Transaction");
                    }
                    _context.Transactions.Add(transactionToCreate);
                    _context.SaveChanges();
                    LogToHistory("Income Addition", $"Income '{transactionToCreate.Title}' added.");
                    return RedirectToAction("TransactionAddition", "Transaction");
                }
            }
            else if (transactionToCreate.TransactionType == "Expense")
            {
                if (ModelState.IsValid)
                {
                    if (transactionToCreate.Id != 0)
                    {
                        _context.Transactions.Update(transactionToCreate);
                        _context.SaveChanges();
                        LogToHistory("Expense Update", $"Expense '{transactionToCreate.Title}' updated.");
                        return RedirectToAction("TransactionAddition", "Transaction");
                    }
                    _context.Transactions.Add(transactionToCreate);
                    _context.SaveChanges();
                    LogToHistory("Expense Addition", $"Expense '{transactionToCreate.Title}' added.");
                    return RedirectToAction("TransactionAddition", "Transaction");
                }
            }

            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
            ViewBag.Transactions = GetUserTransactions();
            return View(transactionToCreate);

        }

        // end of ELAraby Codes
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        // GET: Transaction
        public IActionResult Index()
        {
            var transactions = _context.Transactions.Where(t => t.UserId == SharedValues.CurUser.Id).ToList();
            foreach (var transaction in transactions)
                _context.Entry(transaction).Reference(t => t.Category).Load();
            //PrepareChartData();
            ExpensesByCategory();
            CreateLineChartIncomeExpense();
            return View(transactions);
        }
        // GET: Transaction/Income
        [HttpGet]

        // GET: Transaction/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                return RedirectToAction("Index", "Transaction");
            }
            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
            return View(transaction);
        }


        // GET: Transaction/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null) return NotFound();
            var forbag = GetUserCategories();
            ViewBag.Categories = forbag.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.Transactions = GetUserTransactions();
            return View("TransactionAddition",transaction);
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Update(transaction);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = GetUserCategories();
            ViewBag.Transactions = GetUserTransactions();
            return View("TransactionAddition" ,transaction);
        }

        // GET: Transaction/Details/5
        public IActionResult Details(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // GET: Transaction/Delete/5
        public IActionResult Delete(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return RedirectToAction("TransactionAddition", "Transaction");
        }

        public IActionResult Generator(int id)
        {
            var categories = _context.Categories.ToList();
            var random = new Random();

            // Define start and end date for the range
            var startDate = DateTime.Now.AddMonths(-1); // One month before the current date
            var endDate = DateTime.Now.AddMonths(1); // One month after the current date
            var range = (endDate - startDate).Days; // Get the range in days

            for (int i = 0; i < id; i++)
            {
                var transaction = new Transaction
                {
                    Title = "Transaction " + i,
                    Amount = random.Next(1, 1000),
                    Info = "Info " + i,
                    TransactionType = random.Next(0, 2) == 0 ? "Income" : "Expense",
                    CategoryId = categories[random.Next(0, categories.Count)].Id,
                    UserId = SharedValues.CurUser.Id,
                    Timestamp = startDate.AddDays(random.Next(range)) // Add random days within the range
                };
                _context.Transactions.Add(transaction);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        private void LogToHistory(string operationType, string details)
        {
            var historyItem = new HistoryItem
            {
                OperationType = operationType,
                Details = details,
                Timestamp = DateTime.Now,
                UserId = SharedValues.CurUser.Id,
            };

            _context.History.Add(historyItem);
            _context.SaveChanges();
        }
        public IActionResult Income()
        {
            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
            ViewBag.Transactions = GetUserTransactions("Income");
            return View();
        }

        // POST: Transaction/Income
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Income(Transaction transactionToCreate)
        {
            transactionToCreate.UserId = SharedValues.CurUser.Id;
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transactionToCreate);
                _context.SaveChanges();
                LogToHistory("Income Addition", $"Income '{transactionToCreate.Title}' added.");
                return RedirectToAction("income", "Transaction");
            }

            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
            ViewBag.Transactions = GetUserTransactions("Income");
            return View(transactionToCreate);
        }

        // GET: Transaction/Expense
        [HttpGet]
        public IActionResult Expense()
        {
            ViewBag.Transactions = GetUserTransactions("Expense");
            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
            return View();
        }

        // POST: Transaction/Expense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Expense(Transaction transactionToCreate)
        {
            transactionToCreate.UserId = SharedValues.CurUser.Id;
            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transactionToCreate);
                _context.SaveChanges();
                LogToHistory("Expense Addition", $"Expense '{transactionToCreate.Title}' added.");
                return RedirectToAction("Index", "Transaction");
            }

            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
            ViewBag.Transactions = GetUserTransactions("Expense");
            return View(transactionToCreate);
        }

    }
}




/*
public IActionResult Income()
{
    ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
     var transactions = _context.Transactions
                        .Where(t => t.TransactionType == "Income")
                        .Include(t => t.Category) // Eager loading
                        .ToList();

    foreach (var transaction in transactions)
    {
        _context.Entry(transaction).Reference(t => t.Category).Load();
    }
    return View(transactions);
}
*/
