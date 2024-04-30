using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class TransactionController : Controller
    {
        private readonly Appdbcontxt _context;

        public TransactionController(Appdbcontxt context)
        {
            _context = context;
        }

        // GET: Transaction
        public IActionResult Index()
        {
             var transactions = _context.Transactions
                                .Include(t => t.Category) // Eager loading
                                .ToList();
            foreach (var transaction in transactions)
            {
                _context.Entry(transaction).Reference(t => t.Category).Load();
            }
            return View(transactions);
        }
        // GET: Transaction/Income
        [HttpGet]
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
            ViewBag.Transactions = transactions;
            return View();
        }

        // POST: Transaction/Income
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Income(Transaction transactionToCreate)
        {
            // Check if the provided CategoryId is valid
            var category = _context.Categories.Find(transactionToCreate.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError(nameof(transactionToCreate.CategoryId), "Invalid category.");
            }
            else
            {
                transactionToCreate.Category = category;
                ModelState.Remove(nameof(transactionToCreate.Category));
            }

            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transactionToCreate);
                _context.SaveChanges();
                return RedirectToAction("Index", "Transaction");
            }

            // If ModelState is not valid, collect error messages
            var errorMessage = string.Join(", ", ModelState.Values
                                                    .SelectMany(v => v.Errors)
                                                    .Select(e => e.ErrorMessage));

            // Send error message back to the view
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", transactionToCreate.CategoryId);

            // Reload transactions to display in the view
            var transactions = _context.Transactions
                                .Where(t => t.TransactionType == "Income")
                                .Include(t => t.Category)
                                .ToList();

            foreach (var trans in transactions)
            {
                _context.Entry(trans).Reference(t => t.Category).Load();
            }

            ViewBag.Transactions = transactions;
            return View();
        }

        // GET: Transaction/Expense
        [HttpGet]
        public IActionResult Expense()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            var transactions = _context.Transactions
                                .Where(t => t.TransactionType == "Expense")
                                .Include(t => t.Category) // Eager loading
                                .ToList();

            foreach (var transaction in transactions)
            {
                _context.Entry(transaction).Reference(t => t.Category).Load();
            }
            ViewBag.Transactions = transactions;
            return View();
        }

        // POST: Transaction/Expense
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Expense(Transaction transactionToCreate)
        {
            // Check if the provided CategoryId is valid
            var category = _context.Categories.Find(transactionToCreate.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError(nameof(transactionToCreate.CategoryId), "Invalid category.");
            }
            else
            {
                transactionToCreate.Category = category;
                ModelState.Remove(nameof(transactionToCreate.Category));
            }

            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transactionToCreate);
                _context.SaveChanges();
                return RedirectToAction("Index", "Transaction");
            }

            // If ModelState is not valid, collect error messages
            var errorMessage = string.Join(", ", ModelState.Values
                                                    .SelectMany(v => v.Errors)
                                                    .Select(e => e.ErrorMessage));

            // Send error message back to the view
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", transactionToCreate.CategoryId);

            // Reload transactions to display in the view
            var transactions = _context.Transactions
                                .Where(t => t.TransactionType == "Expense")
                                .Include(t => t.Category)
                                .ToList();

            foreach (var trans in transactions)
            {
                _context.Entry(trans).Reference(t => t.Category).Load();
            }

            ViewBag.Transactions = transactions;
            return View();
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
            // Check if the provided CategoryId is valid
            var category = _context.Categories.Find(transaction.CategoryId);
            if (category == null)
            {
                ModelState.AddModelError(nameof(transaction.CategoryId), "Invalid category.");
            }
            else
            {
                transaction.Category = category;
                ModelState.Remove(nameof(transaction.Category));
            }

            if (ModelState.IsValid)
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                return RedirectToAction("Index", "Transaction");
            }

            // If ModelState is not valid, collect error messages and transaction data
            var errorMessage = string.Join(", ", ModelState.Values
                                                    .SelectMany(v => v.Errors)
                                                    .Select(e => e.ErrorMessage));
            var transactionData = Newtonsoft.Json.JsonConvert.SerializeObject(transaction);

            // Send error message and transaction data back to the view
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.TransactionData = transactionData;
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }


        // GET: Transaction/Edit/5
        public IActionResult Edit(int id)
        {

            var errors = ModelState .Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", transaction.CategoryId);
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
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", transaction.CategoryId);
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
    }
}

