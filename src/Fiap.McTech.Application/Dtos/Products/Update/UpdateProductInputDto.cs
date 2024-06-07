using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.Dtos.Products.Update
{
    /// <summary>
    /// Represents the input data for updating a product.
    /// </summary>
    public class UpdateProductInputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductInputDto"/> class.
        /// </summary>
        /// <param name="id">The ID of the product to be updated.</param>
        /// <param name="name">The new name of the product.</param>
        /// <param name="value">The new value of the product.</param>
        /// <param name="description">The new description of the product.</param>
        /// <param name="image">The new image URL of the product.</param>
        /// <param name="category">The new category of the product.</param>
        public UpdateProductInputDto(Guid id, string name, decimal value, string description, string? image, ProductCategory category)
        {
            Id = id;
            Name = name;
            Value = value;
            Description = description;
            Image = image;
            Category = category;
        }

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
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the new category of the product.
        /// </summary>
        public ProductCategory Category { get; set; }
    }
}
