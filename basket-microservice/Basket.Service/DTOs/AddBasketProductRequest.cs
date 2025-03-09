namespace Basket.Service.DTOs;

public record AddBasketProductRequest(string ProductId, string ProductName, int Quantity = 1);

