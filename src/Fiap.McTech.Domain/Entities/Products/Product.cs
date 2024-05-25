using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Domain.Entities.Products
{
    /// <summary>
    /// Represents a product in the system.
    /// </summary>
    public class Product : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="value">The price of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="image">The image of the product encoded in Base64.</param>
        /// <param name="category">The category of the product.</param>
        public Product(string name, decimal value, string description, string image, ProductCategory category)
        {
            Name = name;
            Value = value;
            Description = description;
            Image = image;
            Category = category;
        }
        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the price of the product.
        /// </summary>
        public decimal Value { get; private set; }

        /// <summary>
        /// Gets the description of the product.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the image of the product encoded in Base64.
        /// </summary>
        public string? Image { get; private set; }

        /// <summary>
        /// Gets the category of the product.
        /// </summary>
        public ProductCategory Category { get; private set; }

        /// <summary>
        /// Determines whether the product is valid.
        /// </summary>
        /// <returns>True if the product is valid; otherwise, false.</returns>
		public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   Value > 0 &&
                   !string.IsNullOrWhiteSpace(Description);
        }
    }
}
