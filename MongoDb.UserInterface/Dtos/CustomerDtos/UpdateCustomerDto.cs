﻿namespace MongoDb.UserInterface.Dtos.CustomerDtos
{
    public record UpdateCustomerDto(string Id, string FirstName, string LastName, string Email, string Address);
}
