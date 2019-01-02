using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestShop.Models.Data;
using TestShop.Models.ViewModels.Shop;

namespace TestShop.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Categories
        [HttpGet]
        public ActionResult Categories()
        {
            // Deklaracja listy kategorii do wyświetlenia
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }

            return View(categoryVMList);
        }

        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            // Deklaracja id
            string id;

            using (Db db = new Db())
            {
                // Sprawdzanie czy nazwa kategorii jest unikalna
                if (db.Categories.Any(x => x.Name == catName))
                {
                    return "tytulzajety";
                }

                // Inicjalizacja DTO
                CategoryDTO dto = new CategoryDTO();
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 1000;

                // Zapis do bazy
                db.Categories.Add(dto);
                db.SaveChanges();

                // Pobieramy id
                id = dto.Id.ToString();
            }
            return id;
        }

        // POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public ActionResult ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;

                // Deklaracja DTO 
                CategoryDTO dto;

                // Sortowanie kategorii
                foreach (var catId in id)
                {
                    dto = db.Categories.Find(catId);
                    dto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }

            return View();
        }

        // GET: Admin/Shop/DeleteCategory
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                // pobieramy kategorię o podanym id
                CategoryDTO dto = db.Categories.Find(id);

                // usuwanie kategorii
                db.Categories.Remove(dto);

                // zapis
                db.SaveChanges();
            }

            return RedirectToAction("Categories");
        }

        // POST: Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            using (Db db = new Db())
            {
                // sprawdzenie czy kategoria jest unikalna
                if (db.Categories.Any(x => x.Name == newCatName))
                    return "tytulzajety";

                // edycja kategorii
                CategoryDTO dto = db.Categories.Find(id);
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();

                db.SaveChanges();
            }

            return "Ok";
        }
    }
}