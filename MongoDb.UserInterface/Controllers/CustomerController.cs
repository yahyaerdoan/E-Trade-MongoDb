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

        public async Task<IActionResult> DeleteCustomer(string id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(string id)
        {
            var values = await _customerService.GetByIdCustomerAsync(id);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            await _customerService.UpdateCustomerAsync(updateCustomerDto);
            return RedirectToAction("Index");
        }
    }
}
