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

        List<Category> GetUserCategories()
        {
            List<Category> categories = _context.Categories.Where(c => c.UserId == SharedValues.CurUser.Id).ToList();
            return categories;


        }

        public void CreatePieChartIncomeExpense()
        {
            double expensesTotal = 0;
            foreach(var transaction in _context.Transactions.Where(t => t.TransactionType == "Expense").ToList())
                expensesTotal += (double)transaction.Amount;
            double incomeTotal = 0;
            foreach (var transaction in _context.Transactions.Where(t => t.TransactionType == "Income").ToList())
                incomeTotal += (double)transaction.Amount;

            // Prepare data for chart
            var dataPoints = new[] {
            new { y = expensesTotal, indexLabel = "Expenses" },
            new { y = incomeTotal, indexLabel = "Income" }
        };

            var chartConfig = new
            {
                title = new { text = "Transaction Summary" },
                legend = new { maxWidth = 350, itemWidth = 120 },
                data = new[] {
                new {
                    type = "doughnut",
                    showInLegend = true,
                    legendText = "{indexLabel}",
                    dataPoints = dataPoints
                }
            }
            };

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

            var chartConfig = new
            {
                theme = "light2",
                animationEnabled = true,
                title = new { text = "Transaction Summary" },
                axisX = new
                {
                    interval = 1,
                    intervalType = "month",
                    valueFormatString = "MMM"
                },
                axisY = new
                {
                    title = "Amount (in USD)",
                    includeZero = true,
                    valueFormatString = "$#0"
                },
                data = new[] {
                new {
                    type = "line",
                    name = "Income",
                    markerSize = 12,
                    xValueFormatString = "MMM, YYYY",
                    yValueFormatString = "$###.#",
                    dataPoints = incomeDataPoints
                },
                new {
                    type = "line",
                    name = "Expense",
                    markerSize = 12,
                    xValueFormatString = "MMM, YYYY",
                    yValueFormatString = "$###.#",
                    dataPoints = expenseDataPoints
                }
            }
            };

            ViewBag.ChartConfig2 = JsonConvert.SerializeObject(chartConfig);
        }
        

        // GET: Transaction
        public IActionResult Index()
        {
            var transactions = _context.Transactions.Where(t => t.UserId == SharedValues.CurUser.Id).ToList();
            foreach (var transaction in transactions)
                _context.Entry(transaction).Reference(t => t.Category).Load();
            //PrepareChartData();
            CreatePieChartIncomeExpense();
            CreateLineChartIncomeExpense();
            return View(transactions);
        }
        // GET: Transaction/Income
        [HttpGet]
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
                return RedirectToAction("Index", "Transaction");
            }

            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
            ViewBag.Transactions = GetUserTransactions("Expense");
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
        public IActionResult Edit(int id)
        {

            var transaction = _context.Transactions.Find(id);
            if (transaction == null) return NotFound();
            ViewBag.Categories = GetUserCategories();
            return View(transaction);
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
            return View(transaction);
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
            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var transaction = _context.Transactions.Find(id);
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Generator(int id)
        {
            var categories = _context.Categories.ToList();
            var random = new Random();
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
                    Timestamp = new DateTime(2020, 1, 1).AddDays(random.Next(0, 365))
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
