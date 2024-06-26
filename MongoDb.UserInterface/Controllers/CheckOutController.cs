using Microsoft.AspNetCore.Mvc;

namespace MongoDb.UserInterface.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
