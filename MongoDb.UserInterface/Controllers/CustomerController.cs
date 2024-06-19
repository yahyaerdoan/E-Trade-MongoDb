using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.CustomerDtos;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;

namespace MongoDb.UserInterface.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _customerService.GetAllAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
           await _customerService.CreateCustomerAsync(createCustomerDto);
            return RedirectToAction("Index");
        }
    }
}
