using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.ViewModels.Clients;
using Fiap.McTech.Application.ViewModels.Orders;
using Fiap.McTech.Application.ViewModels.Payments;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Clients;
using Fiap.McTech.Domain.Entities.Orders;
using Fiap.McTech.Domain.Entities.Payments;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers
{
	public static class AutoMapperConfiguration
	{
		public static void RegisterMappings()
		{
			MapperConfiguration config = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<CartClient, CartClientOutputDto>();
				cfg.CreateMap<CartItem, CartItemOutputDto>();
				cfg.CreateMap<Client, ClientOutputDto>();
				cfg.CreateMap<Order, OrderOutputDto>();
				cfg.CreateMap<Payment, PaymentOutputDto>();
			});

			IMapper mapper = config.CreateMapper();
		}
	}
}