using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.Dtos.Products
{
    /// <summary>
    /// Represents the output data for a product.
    /// </summary>
	public class ProductOutputDto
    {
        /// <summary>
        /// Gets or sets the ID of the product.
        /// </summary>
		public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the product.
        /// </summary>
		public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
		public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the product.
        /// </summary>
		public string Image { get; set; }

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
		public ProductCategory Category { get; set; }
    }
}