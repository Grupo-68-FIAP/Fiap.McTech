using Fiap.McTech.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fiap.McTech.Application.Dtos.Products.Add
{
    /// <summary>
    /// Represents the input data for creating a new product.
    /// </summary>
    public class CreateProductInputDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProductInputDto"/> class.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <param name="value">The value of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="category">The category of the product.</param>
        public CreateProductInputDto(string name, decimal value, string description, ProductCategory category)
        {
            Name = name;
            Value = value;
            Description = description;
            Category = category;
        }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the product.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the image URL of the product.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the category of the product.
        /// </summary>
        [Required]
        public ProductCategory Category { get; set; }
    }
}
