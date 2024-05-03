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
            return _context.Categories.Where(c => c.UserId == SharedValues.CurUser.Id).ToList();
        }

        // GET: Transaction
        public IActionResult Index()
        {
            var transactions = _context.Transactions.Where(t => t.UserId == SharedValues.CurUser.Id).ToList();
            foreach (var transaction in transactions)
                _context.Entry(transaction).Reference(t => t.Category).Load();
            return View(transactions);
        }
        // GET: Transaction/Income
        [HttpGet]
        public IActionResult Income()
        {
            ViewBag.Transactions = GetUserTransactions("Income");
            ViewBag.Categories = new SelectList(GetUserCategories(), "Id", "Name");
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
                return RedirectToAction("Index", "Transaction");
            }

            ViewBag.Categoriesa = GetUserCategories();
            ViewBag.Transactions = GetUserTransactions("Expense");
            return View();
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
                return RedirectToAction("Index", "Transaction");
            }

            ViewBag.Categoriesa = GetUserCategories();
            ViewBag.Transactions = GetUserTransactions("Expense");
            return View();
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
