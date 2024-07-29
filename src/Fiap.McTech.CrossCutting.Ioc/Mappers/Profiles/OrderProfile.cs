using AutoMapper;
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
