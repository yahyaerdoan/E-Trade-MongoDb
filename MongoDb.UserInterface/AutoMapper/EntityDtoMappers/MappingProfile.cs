using AutoMapper;
using MongoDb.UserInterface.Dtos.CategoryDto;
using MongoDb.UserInterface.Dtos.CustomerDtos;
using MongoDb.UserInterface.Dtos.ProductDto;
using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.AutoMapper.EntityDtoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, GetByIdCategoryDto>().ReverseMap();


            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, ResultProductDto>().ReverseMap();
            CreateMap<Product, GetByIdProductDto>().ReverseMap();

            CreateMap<(Product product, string categoryName, string descriptipon), ResultProductWithCategoryDto>()
            .ConstructUsing(src => new ResultProductWithCategoryDto(
                src.product.Id,
                src.product.Name,
                src.product.Description,
                src.product.Price,
                src.product.StockQuantity,
                src.product.ImageIds,
                src.categoryName,
                src.descriptipon)).ReverseMap();

            CreateMap<Customer, CreateCustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();
            CreateMap<Customer, ResultCustomerDto>().ReverseMap();
            CreateMap<Customer, GetByIdCustomerDto>().ReverseMap();
        }
    }
}
