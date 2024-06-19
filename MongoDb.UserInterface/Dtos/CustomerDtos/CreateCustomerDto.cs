namespace MongoDb.UserInterface.Dtos.CustomerDtos
{
    public record CreateCustomerDto(string FirstName, string LastName, string Email, string Address);
}
