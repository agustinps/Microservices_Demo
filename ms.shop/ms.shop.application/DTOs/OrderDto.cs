namespace ms.shop.application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string Email { get; set; }
    }
}
