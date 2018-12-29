using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestShop.Models.Data;
using TestShop.Models.ViewModels.Pages;

namespace TestShop.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Deklaracja listy PageVM
            List<PageVM> pagesList;

            using (Db db = new Db())
            {
                // Inicjalizacja listy
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            return View(pagesList);
        }
        [HttpGet]
        // GET: Admin/Pages/AddPages
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPages
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Sprawdzanie model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                string slug;

                //Inicjalizacja PageDTO
                PageDTO dto = new PageDTO();

                dto.Title = model.Title;
                
                // Gdy nie mamy adresu strony to bierzemy tytuł
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                // Zapobieganie dodaniu tej samej nazwy strony
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Ten tytuł lub adres strony już istnieje.");
                    return View(model);
                }

                dto.Title = model.Title;
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 1000;

                // zapis DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            TempData["SM"] = "Dodałeś nową stronę";

            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageVM model;

            using (Db db = new Db())
            {
                // pobieramy stronę o danym id
                PageDTO dto = db.Pages.Find(id);

                if (dto == null)
                {
                    return Content("Strona nie istnieje");
                }

                model = new PageVM(dto);
            }

            return View(model);
        }

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // pobranie Id strony
                int id = model.Id;

                string slug = "home";

                // pobranie strony do edycji
                PageDTO dto = db.Pages.Find(id);
                
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Strona lub tytuł już istnieje.");
                }

                // modyfikacja DTO
                dto.Title = model.Title;
                dto.Slug = slug;
                dto.HasSidebar = model.HasSidebar;
                dto.Body = model.Body;

                // zapis strony do bazy
                db.SaveChanges();
            }

            TempData["SM"] = "Wyedytowałeś stronę";

            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/Details/id
        [HttpGet]
        public ActionResult Details(int id)
        {
            // deklaracja PageVM
            PageVM model;

            using (Db db = new Db())
            {
                // Pobranie strony o id
                PageDTO dto = db.Pages.Find(id);
                
                // sprawdzenie czy strona o takim id istnieje
                if (dto == null)
                {
                    return Content("Strona o podanym id nie istnieje.");
                }

                // inicjalizacja PageVM
                model = new PageVM(dto);
            }

            return View(model);
        }

        // GET: Admin/Pages/Delete/id
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (Db db = new Db())
            {
                // Pobranie strony do usunięcia
                PageDTO dto = db.Pages.Find(id);

                // Usuwanie wybranej strony z bazy
                db.Pages.Remove(dto);

                // Zapis
                db.SaveChanges();
            }

            // Redirect
            return RedirectToAction("Index");
        }

        // Post: Admin/Pages/ReorderPages
        [HttpGet]
        public ActionResult ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                PageDTO dto;

                // sortowanie stron, zapis na bazie
                foreach (var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }
            return View();
        }

        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarVM model;

            using (Db db = new Db())
            {
                // Pobieramy SidebarDTO
                SidebarDTO dto = db.Sidebar.Find(1);

                // Inicjalizacja modelu
                model = new SidebarVM(dto);
            }

            return View(model);
        }

        // POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                // Pobieramy SidebarDTO
                SidebarDTO dto = db.Sidebar.Find(1);

                // Modyfikacja Sidebar'a
                dto.Body = model.Body;

                // Zapis na bazie;
                db.SaveChanges();
            }

            // Ustawiamy komunikat o modyfikacji Sidebar'a
            TempData["SM"] = "Zmodyfikowałeś pasek boczny.";

            // Redirect
            return RedirectToAction("EditSidebar");
        }
    }
}