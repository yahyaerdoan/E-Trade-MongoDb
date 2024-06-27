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
                    Id = cart.Id,
                    ProductId = cartItem.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = cartItem.Quantity,

                };

                resultOrderDtos.ResultCartDto.ResultCartItemDtos.Add(resultCartItemDto);

                cartQuantities[cartItem.ProductId.ToString()] = cartItem.Quantity;
            }

            ViewBag.CartQuantities = cartQuantities;

            return View(resultOrderDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(ResultOrderDto model)
        {
            var userId = GetStaticCustomerId();
            var cart = await _cartService.GetCartByCustomerIdAsync(userId);

            // Initialize CreateOrderDto if it's null
            if (model.CreateOrderDto == null)
            {
                model.CreateOrderDto = new CreateOrderDto
                {
                    //OrderDate = DateTime.Now,
                    CustomerId = userId,
                    // Set other properties from the form data or user info
                };
            }

            // Initialize ResultCartDto if it's null
            if (model.ResultCartDto == null)
            {
                model.ResultCartDto = new ResultCartDto
                {
                    Id = cart.Id,
                    //ResultCartItemDtos = new List<ResultCartItemDto>()
                };
            }

            // Ensure ResultCartItemDtos is initialized
            if (model.ResultCartDto.ResultCartItemDtos == null)
            {
                model.ResultCartDto.ResultCartItemDtos = new List<ResultCartItemDto>();
            }
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

                model.ResultCartDto.ResultCartItemDtos.Add(resultCartItemDto);
            }

            SaveOrder(model, userId);
            await ClearCart(userId);

            return RedirectToAction("Index", "Home");
        }
        private void SaveOrder(ResultOrderDto model, string userId)
        {
            var order = new Order()
            {
                CartId = model.ResultCartDto.Id,
                OrderNumber = new Random().Next(111111, 999999).ToString(),
                OrderState = EnumOrderState.Completed,
                PaymentType = EnumPaymentType.CreditCard,
                OrderDate = DateTime.Now,
                FirstName = model.CreateOrderDto.FirstName,
                LastName = model.CreateOrderDto.LastName,
                Email = model.CreateOrderDto.Email,
                Phone = model.CreateOrderDto.Phone,
                Address = model.CreateOrderDto.Address,
                City = model.CreateOrderDto.City,
                State = model.CreateOrderDto.State,
                Country = model.CreateOrderDto.Country,
                OrderNote = model.CreateOrderDto.OrderNote,
                CustomerId = userId,               
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in model.ResultCartDto.ResultCartItemDtos)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Price = item.Price.ToString(),
                    Quantity = item.Quantity                   
                };

                order.OrderItems.Add(orderItem);
            }

            _orderService.Create(order);

        }
        private async Task ClearCart(string userId)
        {
            await _cartService.ClearCartByCustomerIdAsync(userId);
        }
    }
}
