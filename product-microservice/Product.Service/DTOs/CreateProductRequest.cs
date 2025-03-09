namespace Product.Service.DTOs;

public record CreateProductRequest(string Name, decimal Price, int ProductTypeId, string? Description = null);
