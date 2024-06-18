using AutoMapper;
using Fiap.McTech.Application.Dtos.Orders;
using Fiap.McTech.Application.Dtos.Orders.Add;
using Fiap.McTech.Application.Dtos.Orders.Update;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Orders;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Mapeamento de Order para OrderOutputDto
            CreateMap<Order, OrderOutputDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.Name : null))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, OrderItemOutputDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price * src.Quantity));

            // Mapeamento de CreateOrderInputDto para Order
            CreateMap<CreateOrderInputDto, Order>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CreateOrderItemInputDto, OrderItem>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            // Mapeamento de UpdateOrderInputDto para Order
            CreateMap<UpdateOrderInputDto, Order>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<UpdateOrderItemInputDto, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<Order, UpdateOrderInputDto>();
            CreateMap<OrderItem, UpdateOrderItemInputDto>();

            CreateMap<CartClient, Order>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.AllValue))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CartClient.Item, OrderItem>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Value))
                ;
        }
    }
}
