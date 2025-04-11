using Microsoft.AspNetCore.Mvc;

namespace GbgMerch.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}