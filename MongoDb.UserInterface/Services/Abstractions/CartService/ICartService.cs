using MongoDb.UserInterface.Dtos.CartItemDto;
using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.Services.Abstractions.CartService
{
    public interface ICartService
    {
        Task InitializeCart(string customerId);
        Task<Cart> GetCartByCustomerIdAsync(string id);
        Task AddToCartAsync(string customerId, string productId, int quantity);
    }
}
