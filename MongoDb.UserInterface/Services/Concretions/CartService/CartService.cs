using AutoMapper;
using MongoDb.UserInterface.Dtos.CartItemDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Settings.MongoDb.Context;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;

namespace MongoDb.UserInterface.Services.Concretions.CartService
{
    public class CartService : ICartService
    {
        private readonly IMongoDbContext _mongoDbContext;
        private readonly IMapper _mapper;

        public CartService(IMongoDbContext mongoDbContext, IMapper mapper)
        {
            _mongoDbContext = mongoDbContext;
            _mapper = mapper;
        }

        public async Task AddCartItem(CreateCartItemDto createCartItemDto)
        {
            var values = _mapper.Map<CartItem>(createCartItemDto);
            await _mongoDbContext.CartItems.InsertOneAsync(values);
        }
    }
}
