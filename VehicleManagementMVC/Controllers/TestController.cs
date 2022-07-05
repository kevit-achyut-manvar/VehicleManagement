using Microsoft.AspNetCore.Mvc;

namespace VehicleManagementMVC.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
