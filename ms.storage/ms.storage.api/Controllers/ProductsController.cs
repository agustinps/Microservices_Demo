using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms.storage.application.Commands;
using ms.storage.application.DTOs;
using ms.storage.application.Queries;
using ms.storage.domain.Interfaces;

namespace ms.storage.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMediator mediator;

        public ProductsController(IProductRepository productRepository, IMediator mediator)
        {
            this.productRepository = productRepository;
            this.mediator = mediator;
        }

        [HttpGet("[action]/{productName}")]
        public async Task<ActionResult<bool>> ProductStockAvaliable(string productName) => Ok(await productRepository.GetProductStock(productName));

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateProduct([FromBody] ProductCreateDto Product) => Ok(await mediator.Send(new CreateProductCommand(Product)));
        

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts() => Ok(await mediator.Send(new GetAllProductsQuery()));

        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id) => Ok(await mediator.Send(new GetProductByIdQuery(id)));

        [HttpDelete("[action]/{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id) => Ok(await mediator.Send(new DeleteProductCommand(id)));

    }
}
