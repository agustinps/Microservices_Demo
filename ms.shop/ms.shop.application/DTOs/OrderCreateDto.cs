namespace ms.shop.application.DTOs
{
    public class OrderCreateDto
    {
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Email { get; set; }
    }
}
