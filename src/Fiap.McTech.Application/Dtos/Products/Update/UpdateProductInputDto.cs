using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.Dtos.Products.Update
{
    /// <summary>
    /// Represents the input data for updating a product.
    /// </summary>
	public class UpdateProductInputDto
    {
        /// <summary>
        /// Gets or sets the ID of the product to be updated.
        /// </summary>
		public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the new name of the product.
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Gets or sets the new value of the product.
        /// </summary>
		public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the new description of the product.
        /// </summary>
		public string Description { get; set; }

        /// <summary>
        /// Gets or sets the new image URL of the product.
        /// </summary>
		public string Image { get; set; }

        /// <summary>
        /// Gets or sets the new category of the product.
        /// </summary>
		public ProductCategory Category { get; set; }
    }
}
