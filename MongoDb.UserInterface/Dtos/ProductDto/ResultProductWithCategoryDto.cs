namespace MongoDb.UserInterface.Dtos.ProductDto
{
    public record ResultProductWithCategoryDto( string Id, 
                                                string Name, 
                                                string Description, 
                                                decimal Price, 
                                                int StockQuantity, 
                                                //string CategoryId, 
                                                List<string> ImageIds, 
                                                string CategoryName
                                                //string CategoryDescription
                                              );
}
