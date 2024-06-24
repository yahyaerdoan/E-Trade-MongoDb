using MongoDb.UserInterface.Dtos.CartItemDto;
using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.Services.Abstractions.CartService
{
    public interface ICartService
    {
        Task AddCartItem(CreateCartItemDto createCartItemDto);
    }
}
