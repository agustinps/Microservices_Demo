using Basket.Service.DTOs;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Service.EndPoints;

public static class BasketApiEndPoints
{
    public static void RegisterEndPoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/basket/{customerId}", GetBasket);

        routeBuilder.MapPost("/basket/{customerId}", CreateBasket).RequireAuthorization();

        routeBuilder.MapPut("/basket/{customerId}", AddBasketProduct).RequireAuthorization();

        routeBuilder.MapDelete("/basket/{customerId}/{productId}", DeleteBasketProduct).RequireAuthorization();

        routeBuilder.MapDelete("/basket/{customerId}", DeleteBasket).RequireAuthorization();
    }


    #region GET
    internal static async Task<CustomerBasket> GetBasket(IBasketStore basketStore, string customerId)
        => await basketStore.GetBasketByCustomerId(customerId);
    #endregion 

    #region POST
    internal static async Task<IResult> CreateBasket(
        IBasketStore basketStore,
        IDistributedCache cache,
        [FromServices] ILogger<Program> logger,
        string customerId,
        CreateBasketRequest createBasketRequest)
    {
        var customerBasket = new CustomerBasket { customerId = customerId };

        decimal.TryParse(await cache.GetStringAsync(createBasketRequest.ProductId), out decimal cachedProductPrice);

        customerBasket.AddProduct(new BasketProduct(createBasketRequest.ProductId,
            createBasketRequest.ProductName, cachedProductPrice));

        await basketStore.CreateCustomerBasket(customerBasket);

        var message = $"Product {createBasketRequest.ProductName} added to basket with customer {customerId}";
        logger.LogInformation(message);

        return TypedResults.Created();
    }
    #endregion

    # region PUT
    internal static async Task<IResult> AddBasketProduct(
        IBasketStore basketStore,
        IDistributedCache cache,
        [FromServices] ILogger<Program> logger,
        string customerId,
        AddBasketProductRequest addProductRequest)
    {
        var customerBasket = await basketStore.GetBasketByCustomerId(customerId);

        decimal.TryParse(await cache.GetStringAsync(addProductRequest.ProductId), out decimal cachedProductPrice);

        customerBasket.AddProduct(new BasketProduct(addProductRequest.ProductId,
            addProductRequest.ProductName, cachedProductPrice, addProductRequest.Quantity));

        await basketStore.UpdateCustomerBasket(customerBasket);
        var message = $"Product {addProductRequest.ProductName} updated in basket with customer {customerId}";
        logger.LogInformation(message);
        return TypedResults.NoContent();
    }
    #endregion

    #region DELETE
    internal static async Task<IResult> DeleteBasketProduct(
        IBasketStore basketStore,
        [FromServices] ILogger<Program> logger,
        string customerId,
        string productId)
    {
        var customerBasket = await basketStore.GetBasketByCustomerId(customerId);
        customerBasket.RemoveProduct(productId);
        var message = $"Product with id {productId} removed from basket";
        logger.LogInformation(message);
        await basketStore.UpdateCustomerBasket(customerBasket);
        return TypedResults.NoContent();
    }



    internal static async Task<IResult> DeleteBasket(
        IBasketStore basketStore,
        [FromServices] ILogger<Program> logger,
        string customerId)
    {
        await basketStore.DeleteCustomerBasket(customerId);
        var message = $"Basket deleted";
        logger.LogInformation(message);
        return TypedResults.NoContent();
    }
    #endregion

}
