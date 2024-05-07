using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;
using ThreeFriends.Models;

namespace ThreeFriends.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Appdbcontxt _context;

        public CategoryController()
        {
            _context = new Appdbcontxt(); // Assuming ApplicationDbContext is your EF context
        }

        // GET: Category
        public ActionResult Index()
        {
            var cat = _context.Categories.Where(c => c.UserId == SharedValues.CurUser.Id).ToList();
            ViewBag.IconList = GetIconList();
            ViewBag.CurUserCategories = cat;
            return View();
        }

        List<SelectListItem> GetIconList()
        {
            // Define the icons folder path
            string iconsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "icons");

            // Get all files in the icons folder
            string[] iconFiles = Directory.GetFiles(iconsFolderPath);

            // Extract file names
            List<SelectListItem> iconList = new List<SelectListItem>();
            foreach (var iconFile in iconFiles)
            {
                string iconName = Path.GetFileName(iconFile);
                string newName = Path.GetFileNameWithoutExtension(string.Join(" ", iconName.Split('-').Select(s => s[0].ToString().ToUpper() + s.Substring(1))));
                iconList.Add(new SelectListItem { Text = newName, Value = iconName });
            }

            return iconList;


        }

        public ActionResult GetIcons()
        {
            string iconsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "icons");
            string[] iconFiles = Directory.GetFiles(iconsFolderPath);

            List<SelectListItem> iconList = new List<SelectListItem>();
            foreach (var iconFile in iconFiles)
            {
                string iconName = Path.GetFileName(iconFile);
                string newName = Path.GetFileNameWithoutExtension(string.Join(" ", iconName.Split('-').Select(s => s[0].ToString().ToUpper() + s.Substring(1))));
                iconList.Add(new SelectListItem { Text = newName, Value = iconName });
            }

            return Json(iconList);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            ViewBag.IconList = GetIconList();
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                LogToHistory("Category Addition", $"Category '{category.Name}' added.");
                category.UserId = SharedValues.CurUser.Id;

                _context.Categories.Add(category);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.IconList = GetIconList();
            var cat = _context.Categories.Where(c => c.UserId == SharedValues.CurUser.Id).ToList();
            ViewBag.CurUserCategories = cat;
            return View("Index",category);
        }



        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var category = _context.Categories.Include(c => c.Transactions).SingleOrDefault(c => c.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var existingCategory = _context.Categories.Find(category.Id);
                if (existingCategory == null)
                {
                    return HttpNotFound();
                }

                // Update properties of the existing category with values from the edited category
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.Icon = category.Icon;

                // Save the changes to the database
                _context.SaveChanges();

                LogToHistory("Category Editing", $"Category '{category.Name}' edited.");
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }


            LogToHistory("Category Deletion", $"Category '{category.Name}' deleted.");
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }


            LogToHistory("Category Deletion", $"Category '{category.Name}' deleted.");
            _context.Categories.Remove(category);
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


        public IActionResult Generator(int id)
        {
            var namesList = new List<string> { "Food", "Transport", "Entertainment", "Health", "Education", "Clothing", "Gifts", "Online Games", "Rent", "Utilities", "Insurance", "Phone", "Internet", "Cable", "Gym", "Subscriptions", "Books", "Movies", "Music", "Software", "Hardware", "Medicine", "Doctor", "Dentist", "Optician", "Therapist", "Tuition", "Books", "Stationery", "Uniform", "Shoes", "Toys", "Decorations", "Cards", "Flowers", "Games", "Skins", "DLC", "Lootboxes" };
            var descriptionList = new List<string> { "Food Category", "Transport Category", "Entertainment Category", "Health Category", "Education Category", "Clothing Category", "Gifts Category", "Online Games Category", "Rent Category", "Utilities Category", "Insurance Category", "Phone Category", "Internet Category", "Cable Category", "Gym Category", "Subscriptions Category", "Books Category", "Movies Category", "Music Category", "Software Category", "Hardware Category", "Medicine Category", "Doctor Category", "Dentist Category", "Optician Category", "Therapist Category", "Tuition Category", "Books Category", "Stationery Category", "Uniform Category", "Shoes Category", "Toys Category", "Decorations Category", "Cards Category", "Flowers Category", "Games Category", "Skins Category", "DLC Category", "Lootboxes Category" };
            var iconList = GetIconList(); // Assuming this method is correctly implemented and returns a list of icons

            int step = new Random().Next(1, 10); // Instantiate Random object

            for (int i = 0; i < id; i += step)
            {
                int index = new Random().Next(0, namesList.Count);
                while(_context.Categories.Any(c => c.Name == namesList[index]))
                    index = new Random().Next(0, namesList.Count);
                var category = new Category
                {
                    Name = namesList[index],
                    Description = descriptionList[index],
                    Icon = iconList[new Random().Next(0, iconList.Count)].Value,
                    UserId = SharedValues.CurUser.Id
                };

                _context.Categories.Add(category);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Transaction");
        }


    }
}
