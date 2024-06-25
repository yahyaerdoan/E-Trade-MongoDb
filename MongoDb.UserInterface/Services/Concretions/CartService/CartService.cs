﻿using AutoMapper;
using Microsoft.CodeAnalysis;
using MongoDb.UserInterface.Dtos.CartItemDto;
using MongoDb.UserInterface.Entities;
using MongoDb.UserInterface.Services.Abstractions.CartService;
using MongoDb.UserInterface.Settings.MongoDb.Context;
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
            var update = Builders<Cart>.Update.Push(x => x.CartItems, new CartItem
            {
                ProductId = productId,
                Quantity = quantity
            });

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
    }
}
