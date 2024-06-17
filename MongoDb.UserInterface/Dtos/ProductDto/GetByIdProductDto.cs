﻿namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record GetByIdProductDto(string Id, string Name, string Description, decimal Price, int StockQuantity, string CategoryId, string ImageUrl);
}
