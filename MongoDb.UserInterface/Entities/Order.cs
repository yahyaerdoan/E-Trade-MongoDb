using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDb.UserInterface.Entities
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string CartId { get; set; }
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

        //public string PaymentId { get; set; }
        //public string PaymentToken { get; set; }
        //public string ConversationId { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public EnumOrderState OrderState { get; set; }
        public EnumPaymentType PaymentType { get; set; }
    }

    public enum EnumOrderState
    {
        Waiting = 1,
        UnPaid = 2,
        Completed = 3,
    }

    public enum EnumPaymentType
    {
        CreditCard = 1,
        DebitCard = 2,
        PayPal = 3,
        Zell = 4
    }
}
