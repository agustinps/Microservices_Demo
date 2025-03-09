namespace Product.Service.DTOs;

public record UpdateProductRequest(string Name, decimal Price, int ProductTypeId, string? Description = null);
