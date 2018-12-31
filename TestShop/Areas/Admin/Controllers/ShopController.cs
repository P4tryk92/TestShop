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
        public ActionResult Categories()
        {
            // deklaracja listy kategorii do wyświetlenia
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }

            return View(categoryVMList);
        }
    }
}