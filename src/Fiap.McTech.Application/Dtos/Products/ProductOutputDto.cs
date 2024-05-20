using Fiap.McTech.Domain.Enums; 

namespace Fiap.McTech.Application.Dtos.Products
{
	public class ProductOutputDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Value { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public ProductCategory Category { get; set; }
	}
}