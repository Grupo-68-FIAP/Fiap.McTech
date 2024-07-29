using AutoMapper;
using Fiap.McTech.Application.Dtos.Payments;
using Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles;
using Fiap.McTech.Domain.Entities.Payments;
using Microsoft.Extensions.DependencyInjection;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers
{
    public static class AutoMapperConfiguration
    {
        public static void RegisterMappings(this IServiceCollection services)
        {
            MapperConfiguration config = new(cfg =>
            {
                // Register Profiles
                cfg.CreateMap<Payment, PaymentOutputDto>();

                cfg.AddProfile<CartClientProfile>();
                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<ClientProfile>();
                cfg.AddProfile<ProductProfile>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
