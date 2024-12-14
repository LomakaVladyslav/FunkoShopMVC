using Microsoft.AspNetCore.Mvc;

namespace FunkoShopInfrastructure.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
