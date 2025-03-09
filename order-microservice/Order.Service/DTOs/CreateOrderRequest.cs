namespace Order.Service.DTOs;

public record CreateOrderRequest(List<OrderProductDto> orders);
