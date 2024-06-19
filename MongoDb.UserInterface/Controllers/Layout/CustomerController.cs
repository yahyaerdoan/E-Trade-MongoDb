using Microsoft.AspNetCore.Mvc;

namespace MongoDb.UserInterface.Controllers.Layout
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
