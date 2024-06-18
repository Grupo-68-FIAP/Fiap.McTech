using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles
{
    public class CartClientProfile : Profile
    {
        public CartClientProfile()
        {
            CreateMap<CartClient, CartClientOutputDto>();
            CreateMap<CartClientInputDto, NewCartClientDto>();
            CreateMap<NewCartClientDto, CartClient>()
                .ConstructUsing(src => new CartClient(src.ClientId, src.AllValue));

            // Itens mappers
            CreateMap<CartClient.Item, CartClientOutputDto.Item>();
            CreateMap<CartClientInputDto.Item, NewCartClientDto.Item>();
            CreateMap<NewCartClientDto.Item, CartClient.Item>()
                .ForCtorParam("name", o => o.MapFrom(src => src.Name))
                .ForCtorParam("quantity", o => o.MapFrom(src => src.Quantity))
                .ForCtorParam("value", o => o.MapFrom(src => src.Value))
                .ForCtorParam("productId", o => o.MapFrom(src => src.ProductId))
                .ForCtorParam("cartClientId", o => o.ExplicitExpansion());
        }
    }
}
