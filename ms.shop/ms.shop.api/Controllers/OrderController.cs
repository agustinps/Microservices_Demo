using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms.shop.application.Commands;
using ms.shop.application.DTOs;
using ms.shop.application.Notifications;
using ms.shop.application.Queries;
using ms.shop.application.Services;

namespace ms.shop.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IStorageApi storageApi;        

        public OrderController(IMediator mediator, IStorageApi storageApi)
        {
            this.mediator = mediator;
            this.storageApi = storageApi;            
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> CreateOrder([FromBody] OrderCreateDto order)
        {
            //Verificamos si hay stock del producto mediante el servicio que creamos utilizando Refit
            //para llamar al método creado en el microservicio ms.storage
            if (!await storageApi.StockAvailable(order.Product))
            {
                //Si NO hay stock lanzamos el envento de cancelación de orden,
                //que enviará mail al cliente mediante OrderCanceledEmailHandler
                //y guardará el registro con valores que indiquen pedido anulado, cantidad = 0, etc... mediante OrderCanceledSaveHandler
                await mediator.Publish(new OrderCancelNotification(order));
                return Ok("Producto sin stock, pedido no realizado");
            }
            else
                return Ok(await mediator.Send(new CreateOrderCommand(order)));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders() => Ok(await mediator.Send(new GetAllOrdersQuery()));

        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id) => Ok(await mediator.Send(new GetOrderByIdQuery(id)));

        [HttpDelete("[action]/{id:int}")]
        public async Task<ActionResult> DeleteOrder(int id) => Ok(await mediator.Send(new DeleteOrderCommand(id)));

    }
}
