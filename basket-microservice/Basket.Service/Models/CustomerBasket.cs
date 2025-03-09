namespace Basket.Service.Models;

internal class CustomerBasket
{
    private HashSet<BasketProduct> Items { get; } = new();
    public required string customerId { get; init; }
    public IEnumerable<BasketProduct> Products => Items;

    public decimal BasketTotal
    {
        get
        {
            return Items.Select(p => p.Quantity * p.ProductPrice).Sum();
        }
    }
    public void AddProduct(BasketProduct product)
    {
        if (product is null)
            throw new ArgumentNullException(nameof(product));

        var existsProduct = Items.FirstOrDefault(p => p.ProductId == product.ProductId);
        if (existsProduct is not null)
        {
            Items.Remove(existsProduct);
            Items.Add(new BasketProduct(existsProduct.ProductId, existsProduct.ProductName, product.Quantity));
        }
        else
            Items.Add(product);
    }

    public void RemoveProduct(string productId)
        => Items.RemoveWhere(p => p.ProductId == productId);


}
