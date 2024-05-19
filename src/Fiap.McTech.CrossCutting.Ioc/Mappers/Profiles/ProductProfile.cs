using AutoMapper;
using Fiap.McTech.Application.Dtos.Products.Add;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Domain.Entities.Products;

namespace Fiap.McTech.CrossCutting.Ioc.Mappers.Profiles
{
	public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			CreateMap<Product, ProductOutputDto>();
			CreateMap<ProductOutputDto, Product>();
			CreateMap<CreateProductInputDto, Product>();
			CreateMap<UpdateProductInputDto, Product>();
		}
	}
}