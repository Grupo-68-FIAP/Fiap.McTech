using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemOutputDto>();
            CreateMap<CartItemOutputDto, CartItem>();
            CreateMap<CartItem, CartItemInputDto>();
            CreateMap<CartItemInputDto, CartItem>();
        }
    }
}