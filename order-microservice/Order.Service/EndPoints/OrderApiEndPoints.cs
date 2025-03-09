using ECommerce.shared.Infrastructure.EventBus.Abstraction;
using ECommerce.Shared.Observability;
using Microsoft.AspNetCore.Mvc;
using Order.Service.DTOs;
using Order.Service.Infrastructure.Data;
using Order.Service.IntegrationEvents.Events;

namespace Order.Service.EndPoints;

public static class OrderApiEndPoints
{
    public static void RegisterEndPoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/order/{customerId}", CreateOrder).RequireAuthorization();

        routeBuilder.MapGet("/order/{customerId}/{orderId}", GetOrder).RequireAuthorization();

    }


    #region POST
    internal static async Task CreateOrder([FromServices] IEventBus eventBus,
                [FromServices] IOrderStore orderStore,
                [FromServices] MetricFactory metricFactory,
                [FromServices] ILogger<Program> logger,
                string customerId,
                CreateOrderRequest orderRequest)
    {
        var order = new Models.Order { CustomerId = customerId };
        foreach (var product in orderRequest.orders)
            order.AddOrderProduct(product.ProductId, product.Quantity);

        await orderStore.CreateOrderAsync(order);

        var message = $"Order with id {order.OrderId} added for customer {order.CustomerId}";
        logger.LogInformation(message);

        var orderCounter = metricFactory.Counter("total-orders", "Orders");
        orderCounter.Add(1);

        var productsPerOrderHistogram = metricFactory.Histogram("products-per-order", "Products");
        productsPerOrderHistogram.Record(order.OrderProducts.DistinctBy(p => p.ProductId).Count());

        await eventBus.PublishAsync(new OrderCreatedEvent(customerId));
        TypedResults.Created($"{order.CustomerId}/{order.OrderId}");
    }

    #endregion

    #region GET
    internal static async Task<IResult> GetOrder([FromServices] IOrderStore orderStore,
             string customerId,
             string orderId)
    {
        var order = await orderStore.GetCustomerOrderByIdAsync(customerId, orderId);
        return order is null
          ? TypedResults.NotFound("Order not found for customer")
          : TypedResults.Ok(order);
    }

    #endregion

}
