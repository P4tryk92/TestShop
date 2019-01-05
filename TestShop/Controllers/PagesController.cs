using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestShop.Models.Data;
using TestShop.Models.ViewModels.Pages;

namespace TestShop.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/{pages}
        public ActionResult Index(string page = "")
        {
            // ustawiamy adres naszej strony
            if (page == "")
                page = "home";

            // deklarujemy pageVM
            PageVM model;
            PageDTO dto;

            // sprawdzamy czy strona istnieje
            using (Db db = new Db())
            {
                if (!db.Pages.Any(x => x.Slug.Equals(page)))
                    return RedirectToAction("Index", new { page = "" });
            }

            // pobieramy pageDTO
            using (Db db = new Db())
            {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }

            // ustawiamy tytuł naszej strony
            ViewBag.PageTitle = dto.Title;

            // sprawdzamy czy strona ma pasek boczny
            if (dto.HasSidebar == true)
                ViewBag.Sidebar = "Tak";
            else
                ViewBag.Sidebar = "Nie";

            // inicjalizujemy pageVM
            model = new PageVM(dto);

            // zwracamy widok z pageVM
            return View(model);
        }

        public ActionResult PagesMenuPartial()
        {
            // deklaracja PageVM
            List<PageVM> pageVMList;

            // pobieranie stron
            using (Db db = new Db())
            {
                pageVMList = db.Pages.ToArray()
                                .OrderBy(x => x.Sorting)
                                .Where(x => x.Slug != "home")
                                .Select(x => new PageVM(x))
                                .ToList();
            }

            // zwracamy pageVMList
            return PartialView(pageVMList);
        }

        public ActionResult SidebarPartial()
        {
            // deklarujemy model
            SidebarVM model;

            // inicjalizujemy model
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                model = new SidebarVM(dto);
            }

            // zwracamy partial z modelem
            return PartialView(model);
        }
    }
}