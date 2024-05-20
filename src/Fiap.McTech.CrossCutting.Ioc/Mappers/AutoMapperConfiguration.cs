using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Payments;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers
{
    public static class AutoMapperConfiguration
    {
        public static void RegisterMappings()
        {
            MapperConfiguration config = new(cfg =>
            {
                // Register Profiles
                cfg.CreateMap<CartClient, CartClientOutputDto>();
                cfg.CreateMap<CartItem, CartItemOutputDto>();
                cfg.CreateMap<Client, ClientOutputDto>();
                cfg.CreateMap<Client, ClientInputDto>();
                cfg.CreateMap<Payment, PaymentOutputDto>();

                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<ClientProfile>();
                cfg.AddProfile<ProductProfile>();
            });

            IMapper mapper = config.CreateMapper();
        }
    }
}