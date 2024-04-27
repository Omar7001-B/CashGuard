using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // GET: Category/Create
        public ActionResult Create()
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
                iconList.Add(new SelectListItem { Text = iconName, Value = iconName });
            }

            // Set ViewBag.IconList
            ViewBag.IconList = iconList;

            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
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
                //_context.Entry(category).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
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

            return View(category);
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

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
