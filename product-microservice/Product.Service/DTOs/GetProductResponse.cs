namespace Product.Service.DTOs;

public record GetProductResponse(int Id, string Name, decimal Price, string ProductType, string? Description = null);
