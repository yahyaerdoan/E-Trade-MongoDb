namespace MongoDb.UserInterface.Dtos.CustomerDtos
{
    public record GetByIdCustomerDto(string Id, string FirstName, string LastName, string Email, string Address);
}
