using Fiap.McTech.Domain.Enums; 

namespace Fiap.McTech.Domain.Entities.Products
{
	public class Product : EntityBase
	{
		//EF
        public Product() { }

        public Product(string name, decimal value, string description, string image, ProductCategory category)
		{
			Name = name;
			Value = value;
			Description = description;
			Image = image;
			Category = category;
		}

		public string Name { get; internal set; }
        public decimal Value { get; internal set; }
        public string Description { get; internal set; }
        public string Image { get; internal set; }
        public ProductCategory Category { get; internal set; }

		public override bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(Name) &&
				   Value > 0 &&
				   !string.IsNullOrWhiteSpace(Description) &&
				   !string.IsNullOrWhiteSpace(Image);
		}
	}
}