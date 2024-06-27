using Microsoft.AspNetCore.Mvc;
using MongoDb.UserInterface.Dtos.CartItemDto;
using MongoDb.UserInterface.Dtos.OrderDtos;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Services.Abstractions.CustomerServices;
using MongoDb.UserInterface.Services.Abstractions.OrderService;
using MongoDb.UserInterface.Services.Abstractions.ProductServices;

namespace MongoDb.UserInterface.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public CheckOutController(IProductService productService, ICustomerService customerService, ICartService cartService, IOrderService orderService)
        {
            _productService = productService;
            _customerService = customerService;
            _cartService = cartService;
            _orderService = orderService;
        }

        private string GetStaticCustomerId()
        {
            return _customerService.GetCustomerByStaticId();
        }
        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartByCustomerIdAsync(GetStaticCustomerId());

            var resultOrderDtos = new ResultOrderDto()
            { 
                ResultCartDto = new ResultCartDto()
                {
                    Id = cart.Id,
                    ResultCartItemDtos = new List<ResultCartItemDto>()
                }
            };

            var cartQuantities = new Dictionary<string, int>();

            foreach (var cartItem in cart.CartItems)
            {
                var product = await _productService.GetByIdProductAsync(cartItem.ProductId);
                 
                var resultCartItemDto = new ResultCartItemDto
                {
                    ProductId = cartItem.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = cartItem.Quantity,
                    Id = cart.Id
                };

                resultOrderDtos.ResultCartDto.ResultCartItemDtos.Add(resultCartItemDto);

                cartQuantities[cartItem.ProductId.ToString()] = cartItem.Quantity;
            }

            ViewBag.CartQuantities = cartQuantities;

            return View(resultOrderDtos);
        }

        [HttpGet]
        public IActionResult Checkout() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Checkout(ResultOrderDto model)
        {
            return View(model);
        }     
      
    }
}
