using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Enums;
 
namespace Fiap.McTech.Domain.Entities.Catalogs
{
	public class Catalog : EntityBase
	{
		public Catalog(Guid catalogId, string name, ProductCategory category, List<Product> products)
		{
			CatalogId = catalogId;
			Name = name;
			Category = category;
			Products = products;
		}

		public Guid CatalogId { get; private set; }
		public string Name { get; private set; } = string.Empty;
		public ProductCategory Category { get; private set; } = ProductCategory.None;
		public List<Product> Products { get; private set; }= new List<Product>();

		public override bool IsValid()
		{
			return CatalogId != Guid.Empty &&
				   !string.IsNullOrWhiteSpace(Name) &&
				   Category != ProductCategory.None &&
				   Products != null;
		}
	}
}