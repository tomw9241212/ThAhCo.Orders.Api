namespace ThAhCo.Orders.Api.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime RequestedDate { get; set; }

    }
}
