using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
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
               public class TransactionFilterViewModel
        {
            [Display(Name = "Start Date")]
            public DateTime? StartDate { get; set; }

            [Display(Name = "End Date")]
            public DateTime? EndDate { get; set; }

            [Display(Name = "Transaction Type")]
            public string SelectedTransactionType { get; set; }

            [Display(Name = "Category")]
            public int? SelectedCategory { get; set; }

            [Display(Name = "Minimum Amount")]
            public decimal? MinAmount { get; set; }

            [Display(Name = "Maximum Amount")]
            public decimal? MaxAmount { get; set; }

            [Display(Name = "Keyword")]
            public string Keyword { get; set; }

            [Display(Name = "Sort By")]
            public string SortBy { get; set; }

            [Display(Name = "Sort Order")]
            public string SortOrder { get; set; }

            public List<Transaction> Transactions { get; set; }
            public List<Category> Categories { get; set; }
            public string[] TransactionTypes { get; set; }
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

        public void OldChart()
       {
             // var transactions = _context.Transactions.ToList();
            var transactions = GetUserTransactions("All");
            var expensesTotal = GetUserTransactions("Expense").Sum(t => t.Amount);
            var incomeTotal = GetUserTransactions("Income").Sum(t => t.Amount);
            var categories = _context.Categories.Where(c => c.UserId == SharedValues.CurUser.Id).ToList();

            ViewBag.ExpensesTotal = expensesTotal;
            ViewBag.IncomeTotal = incomeTotal;

            return;
        }

        public static object CreateTransactionBarChart(List<Transaction> transactions)
        {
            var IncomeDataPoints = transactions.Where(t => t.TransactionType == "Income").GroupBy(t => t.CategoryId).Select(group => new
            {
                y = group.Sum(t => t.Amount),
                label = group.First().Category?.Name
            }).ToList();

            var ExpenseDataPoints = transactions.Where(t => t.TransactionType == "Expense").GroupBy(t => t.CategoryId).Select(group => new
            {
                y = group.Sum(t => t.Amount),
                label = group.First().Category?.Name
            }).ToList();

            return new
            {
                title = new
                {
                    text = "Transaction Summary"
                },
                data = new[]
                {
                    new
                    {
                        type = "bar",
                        showInLegend = true,
                        legendText = "Income",
                        //color = "blue",
                        dataPoints = IncomeDataPoints
                    },
                    new
                    {
                        type = "bar",
                        showInLegend = true,
                        legendText = "Expense",
                        //color = "red",
                        dataPoints = ExpenseDataPoints
                    }
                }
            };
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
                title = new
                {
                    text = type + " Summary"
                },
                data = new[]
                {
                    new
                    {
                        type = "doughnut",
                        showInLegend = true,
                        legendText = "{label}",
                        indexLabel = "{label}: #percent%",
                        indexLabelPlacement = "inside",
                        dataPoints = DataPoints
                    }
                }

            };
        }

        public static object CreateTransactionLineChart(List<Transaction> transactions)
        {
            var incomeTransactions = transactions.Where(t => t.TransactionType == "Income").ToList();
            var expenseTransactions = transactions.Where(t => t.TransactionType == "Expense").ToList();

            var incomeDataPoints = incomeTransactions.Select(transaction => new
            {
                x = transaction.Timestamp,
                y = transaction.Amount,
                indexLabel = transaction.Title + " - " + transaction.Category.Name
            }).ToList();

            // Prepare data points for expense line
            var expenseDataPoints = expenseTransactions.Select(transaction => new
            {
                x = transaction.Timestamp,
                y = transaction.Amount,
                indexLabel = transaction.Title + " - " + transaction.Category.Name
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

            return chartConfig;
        }



        // GET: Transaction/Filtration
        public ActionResult Index()
        {
            var viewModel = new TransactionFilterViewModel
            {
                Categories = _context.Categories.ToList(),
                TransactionTypes = new[] { "Expense", "Income" }
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(TransactionFilterViewModel viewModel)
        {
                 var transactions = _context.Transactions.Where(t =>
                 (t.UserId == SharedValues.CurUser.Id) &&
                (viewModel.SelectedTransactionType == null || t.TransactionType == viewModel.SelectedTransactionType) &&
                (viewModel.SelectedCategory == null || t.CategoryId == viewModel.SelectedCategory) &&
                (!viewModel.StartDate.HasValue || t.Timestamp >= viewModel.StartDate) &&
                (!viewModel.EndDate.HasValue || t.Timestamp <= viewModel.EndDate) &&
                (!viewModel.MinAmount.HasValue || t.Amount >= viewModel.MinAmount) &&
                (!viewModel.MaxAmount.HasValue || t.Amount <= viewModel.MaxAmount) &&
                (string.IsNullOrEmpty(viewModel.Keyword) || t.Title.Contains(viewModel.Keyword) || t.Info.Contains(viewModel.Keyword))
            ).ToList();

            switch (viewModel.SortBy)
            {
                case "Date":
                    transactions = viewModel.SortOrder == "Asc" ? transactions.OrderBy(t => t.Timestamp).ToList() : transactions.OrderByDescending(t => t.Timestamp).ToList();
                    break;
                case "Amount":
                    transactions = viewModel.SortOrder == "Asc" ? transactions.OrderBy(t => t.Amount).ToList() : transactions.OrderByDescending(t => t.Amount).ToList();
                    break;
                case "Title":
                    transactions = viewModel.SortOrder == "Asc" ? transactions.OrderBy(t => t.Title).ToList() : transactions.OrderByDescending(t => t.Title).ToList();
                    break;
                case "Category":
                    transactions = viewModel.SortOrder == "Asc" ? transactions.OrderBy(t => t.Category.Name).ToList() : transactions.OrderByDescending(t => t.Category.Name).ToList();
                    break;
            }

            foreach (var transaction in transactions) _context.Entry(transaction).Reference(t => t.Category).Load();
            viewModel.Transactions = transactions;
            viewModel.TransactionTypes = new[] { "Expense", "Income" };
            viewModel.Categories = _context.Categories.ToList();

            // Charts Config
            ViewBag.BarChartConfig = JsonConvert.SerializeObject(CreateTransactionBarChart(transactions));
            ViewBag.IncomePieChartConfig = JsonConvert.SerializeObject(CreateTransactionPierChart(transactions, "Income"));
            ViewBag.ExpensePieChartConfig = JsonConvert.SerializeObject(CreateTransactionPierChart(transactions, "Expense"));
            ViewBag.LineChartConfig = JsonConvert.SerializeObject(CreateTransactionLineChart(transactions));

            return View(viewModel);
        }
    }
}
