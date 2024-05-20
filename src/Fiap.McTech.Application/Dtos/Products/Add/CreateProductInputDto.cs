using Fiap.McTech.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fiap.McTech.Application.Dtos.Products.Add
{
    public class CreateProductInputDto
    {
        public CreateProductInputDto(string name, decimal value, string description, ProductCategory category)
        {
            Name = name;
            Value = value;
            Description = description;
            Category = category;
        }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Value { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Image { get; set; }
        [Required]
        public ProductCategory Category { get; set; }
    }
}