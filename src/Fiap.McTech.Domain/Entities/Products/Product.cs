using Fiap.McTech.Domain.Enums; 

namespace Fiap.McTech.Domain.Entities.Products
{
	public class Product : EntityBase
	{
		public Product(string name, decimal value, string description, string image, ProductCategory category)
		{
			Name = name;
			Value = value;
			Description = description;
			Image = image;
			Category = category;
		}

		public string Name { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }
        public string Image { get; private set; }
        public ProductCategory Category { get; private set; }

		public override bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(Name) &&
				   Value > 0 &&
				   !string.IsNullOrWhiteSpace(Description) &&
				   !string.IsNullOrWhiteSpace(Image);
		}
	}
}