namespace ms.storage.application.DTOs
{
    public class ProductCreateDto
    {        
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Supplier { get; set; }
    }
}
