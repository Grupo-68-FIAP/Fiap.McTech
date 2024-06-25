using AutoMapper;
using Fiap.McTech.Application.Dtos.Orders;
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

            CreateMap<Order.Item, OrderOutputDto.Item>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Price * src.Quantity));

            // Mapeamento de CreateOrderInputDto para Order
            CreateMap<CreateOrderInputDto, Order>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CreateOrderInputDto.Item, Order.Item>()
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

            CreateMap<UpdateOrderInputDto.Item, Order.Item>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<Order, UpdateOrderInputDto>();
            CreateMap<Order.Item, UpdateOrderInputDto.Item>();

            CreateMap<CartClient, Order>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForCtorParam("clientId", opt => opt.MapFrom(src => src.ClientId))
                .ForCtorParam("totalAmount", opt => opt.MapFrom(src => src.AllValue));

            CreateMap<CartClient.Item, Order.Item>()
                .ForCtorParam("productId", opt => opt.MapFrom(src => src.ProductId))
                .ForCtorParam("productName", opt => opt.MapFrom(src => src.Name))
                .ForCtorParam("price", opt => opt.MapFrom(src => src.Value))
                .ForCtorParam("quantity", opt => opt.MapFrom(src => src.Quantity))
                .ForCtorParam("orderId", opt => opt.ExplicitExpansion());
        }
    }
}
