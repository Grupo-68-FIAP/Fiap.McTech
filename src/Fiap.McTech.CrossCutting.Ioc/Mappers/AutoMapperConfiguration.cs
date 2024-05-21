using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Dtos.Clients;
using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Payments;
using Fiap.McTech.Domain.Interfaces.Repositories.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers
{
    public static class AutoMapperConfiguration
    {
        public static void RegisterMappings(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var clientRepository = serviceProvider.GetService<IClientRepository>();

            MapperConfiguration config = new(cfg =>
            {
                // Register Profiles
                cfg.CreateMap<Payment, PaymentOutputDto>();

                cfg.AddProfile<CartClientProfile>();
                cfg.AddProfile<CartItemProfile>();
                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<ClientProfile>();
                cfg.AddProfile<ProductProfile>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}