using Microsoft.AspNetCore.Mvc;

namespace MongoDb.UserInterface.Controllers.Layout
{
    public class LayoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
