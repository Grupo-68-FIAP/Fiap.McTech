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
            CreateMap<CartClientOutputDto, CartClient>();
            CreateMap<CartClient, CartClientInputDto>();
            CreateMap<CartClientInputDto, CartClient>();
        }
    }
}