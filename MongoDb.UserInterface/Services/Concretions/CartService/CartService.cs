using AutoMapper;
using Microsoft.CodeAnalysis;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Settings.MongoDb.NewContext;
using MongoDB.Bson;
using MongoDB.Driver;

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

        public async Task AddToCartAsync(string cartId, string productId, int quantity)
        {
            var filter = Builders<Cart>.Filter.Eq(x => x.Id, cartId);
            var cart = await _mongoDbContext.Carts.Find(filter).FirstOrDefaultAsync();

            var existingCartItemIndex = cart.CartItems.FindIndex(item => item.ProductId == productId);

            if (existingCartItemIndex < 0)
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
            else
            {
                cart.CartItems[existingCartItemIndex].Quantity += quantity;
            }

            await _mongoDbContext.Carts.ReplaceOneAsync(filter, cart);

            #region just add to cart no control
            //var filter = Builders<Cart>.Filter.Eq(x => x.Id, cartId);
            //var update = Builders<Cart>.Update.Push(x => x.CartItems, new CartItem
            //{
            //    ProductId = productId,
            //    Quantity = quantity
            //});

            //await _mongoDbContext.Carts.UpdateOneAsync(filter, update);
            #endregion

            #region  add to cart with product control increase quantity
            //// Define the filters
            //var cartFilter = Builders<Cart>.Filter.Eq(x => x.Id, cartId);
            //var productFilter = Builders<Cart>.Filter.ElemMatch(x => x.CartItems, item => item.ProductId == productId);

            //// Combine filters
            //var combinedFilter = Builders<Cart>.Filter.And(cartFilter, productFilter);

            //// Define the update for incrementing the quantity
            //var updateExistingProduct = Builders<Cart>.Update.Inc("CartItems.$.Quantity", quantity);

            //// Try to update the existing product quantity
            //var result = await _mongoDbContext.Carts.UpdateOneAsync(combinedFilter, updateExistingProduct);

            //// If the product does not exist, add it to the cart
            //if (result.ModifiedCount == 0)
            //{
            //    var newItem = new CartItem { ProductId = productId, Quantity = quantity };
            //    var updateAddNewProduct = Builders<Cart>.Update.Push(x => x.CartItems, newItem);
            //    await _mongoDbContext.Carts.UpdateOneAsync(cartFilter, updateAddNewProduct);
            //}
            #endregion
        }

        public async Task ClearCartByCustomerIdAsync(string customerId)
        {
            var filter = Builders<Cart>.Filter.Eq(cart => cart.CustomerId, customerId);
            var update = Builders<Cart>.Update.Set(cart => cart.CartItems, new List<CartItem>());
            await _mongoDbContext.Carts.UpdateOneAsync(filter, update);
        }

        public async Task DeleteCartItemAsync(string customerId,string productId)
        {
            var filter = Builders<Cart>.Filter.Eq(x => x.CustomerId, customerId);
            var update = Builders<Cart>.Update.PullFilter(x => x.CartItems, item => item.ProductId == productId);
            await _mongoDbContext.Carts.UpdateOneAsync(filter, update);
        }

        public async Task<Cart> GetCartByCustomerIdAsync(string id)
        {            
            return await _mongoDbContext.Carts.Find(x => x.CustomerId == id).FirstOrDefaultAsync();
        }

        public async Task InitializeCart(string id)
        {
            var existingCart = await _mongoDbContext.Carts.Find(x => x.CustomerId == id).FirstOrDefaultAsync();
            if (existingCart == null)
            {
                var cartId = ObjectId.GenerateNewId().ToString();
                var newCart = new Cart { Id = cartId, CustomerId = id, CartItems = new List<CartItem>() };
                await _mongoDbContext.Carts.InsertOneAsync(newCart);
            }
        }

        public async Task UpdateQuantityAsync(string cartId, string productId, int change)
        {
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(x => x.Id, cartId),
                Builders<Cart>.Filter.ElemMatch(c => c.CartItems, ci => ci.ProductId == productId)
            );

            var update = Builders<Cart>.Update.Inc("CartItems.$.Quantity", change);
            await _mongoDbContext.Carts.UpdateOneAsync(filter, update);
        }
    }
}
