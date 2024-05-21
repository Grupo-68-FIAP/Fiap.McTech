using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.Application.Dtos.Cart
{
    public class NewCartItemDto : CartItemInputDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; } = decimal.Zero;
    }
}