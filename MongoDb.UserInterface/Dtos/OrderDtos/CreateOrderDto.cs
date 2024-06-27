using MongoDb.UserInterface.Entities;

namespace MongoDb.UserInterface.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string OrderNote { get; set; }
    }
}
