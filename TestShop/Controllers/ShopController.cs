using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestShop.Models.Data;
using TestShop.Models.ViewModels.Shop;

namespace TestShop.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            // deklarujemy CategoryVM list
            List<CategoryVM> categoryVMList;

            // inicjalizacja listy
            using (Db db = new Db())
            {
                categoryVMList = db.Categories
                                    .ToArray()
                                    .OrderBy(x => x.Sorting)
                                    .Select(x => new CategoryVM(x))
                                    .ToList();
            }

            // zwracamy partial z listą
            return PartialView(categoryVMList);
        }

        // GET: /shop/category/name
        public ActionResult Category(string name)
        {
            // deklaracja productVMList
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                // pobieramy id kategorii
                CategoryDTO categoryDTO = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;

                // inicjalizacja listy produktów
                productVMList = db.Products
                                    .ToArray()
                                    .Where(x => x.CategoryId == catId)
                                    .Select(x => new ProductVM(x))
                                    .ToList();
                // pobieramy nazwę kategorii
                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                ViewBag.CategoryName = productCat.CategoryName;
            }

            // zwracamy widok z listą produktów z danej kategorii
            return View(productVMList);
        }

        // GET: /shop/products-details/name
        [ActionName("products-details")]
        public ActionResult ProductDetails(string name)
        {
            // deklaracja productVM i productDTO
            ProductVM model;
            ProductDTO dto;

            // inicjalizacja product id
            int id = 0;

            using (Db db = new Db())
            {
                // sprawdzamy czy produkt istnieje
                if (!db.Products.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                // inicjalizacja productDTO
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();

                // pobieramy id
                id = dto.Id;

                //inicjalizacja modelu
                model = new ProductVM(dto);
            }

            // pobieramy galerię zdjęć dla wybranego produktu
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                            .Select(fn => Path.GetFileName(fn));

            // zwracamy widok z modelem
            return View("ProductDetails", model);
        }
    }
}