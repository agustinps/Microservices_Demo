using ECommerce.shared.Infrastructure.EventBus.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Product.Service.DTOs;
using Product.Service.Infrastructure.Data;
using Product.Service.IntegrationEvents;

namespace Product.Service.EndPoints;

public static class ProductApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {

        routeBuilder.MapGet("/product/{productId}", GetByProductId);

        routeBuilder.MapPost("/product/", CreateProduct)
            .RequireAuthorization();

        routeBuilder.MapPut("/product/{productId}", UpdateProduct)
            .RequireAuthorization();
    }


    #region GET
    internal static async Task<IResult> GetByProductId(
            [FromServices] IProductStore productStore,
            [FromServices] ILogger<Program> logger,
            int productId)
    {
        var product = await productStore.GetById(productId);
        var message = $"Product with id {productId} found";
        logger.LogInformation(message);
        return product is null
            ? TypedResults.NotFound("Product not found")
            : TypedResults.Ok(new GetProductResponse(product.Id, product.Name,
                product.Price, product?.ProductType?.Type!, product?.Description));
    }

    #endregion

    #region POST
    internal static async Task CreateProduct(
            [FromServices] IProductStore productStore,
            [FromServices] ILogger<Program> logger,
            CreateProductRequest request)
    {
        var product = new Models.Product
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            ProductTypeId = request.ProductTypeId
        };
        await productStore.CreateProduct(product);
        var message = $"Product {product.Name} created";
        logger.LogInformation(message);
        TypedResults.Created(product.Id.ToString());
    }

    #endregion

    #region PUT
    internal static async Task<IResult> UpdateProduct(
                [FromServices] IProductStore productStore,
                [FromServices] IEventBus eventBus,
                [FromServices] ILogger<Program> logger,
                int productId,
                UpdateProductRequest request)
    {
        var product = await productStore.GetById(productId);
        if (product is null)
        {
            var notfoundMessage = $"Product with id {productId} not found";
            logger.LogInformation(notfoundMessage);
            return TypedResults.NotFound($"Product with id {productId} does not exist");
        }

        var existingPrice = product.Price;
        product.Name = request.Name;
        product.Price = request.Price;
        product.ProductTypeId = request.ProductTypeId;
        product.Description = request.Description;
        await productStore.UpdateProduct(product);
        var message = $"Product {product.Name} updated";
        logger.LogInformation(message);
        if (!decimal.Equals(existingPrice, request.Price))
            await eventBus.PublishAsync(new ProductPriceUpdatedEvent(productId, request.Price));

        return TypedResults.NoContent();
    }
    #endregion

}
