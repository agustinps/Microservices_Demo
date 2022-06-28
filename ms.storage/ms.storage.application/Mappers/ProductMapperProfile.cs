using AutoMapper;
using ms.rabbitmq.Events;
using ms.storage.application.DTOs;
using ms.storage.domain.Entities;

namespace ms.storage.application.Mappers
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();            
            CreateMap<OrderPreparedEvent, OrderCreatedEvent>().ReverseMap();
            CreateMap<OrderShippedEvent, OrderCreatedEvent>().ReverseMap();
        }
    }
}
