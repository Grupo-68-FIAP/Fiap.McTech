using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.Application.Dtos.Cart
{
    public class CartItemOutputDto
    {
        public CartItemOutputDto(Guid id, Guid productId, Guid cartClientId, string name, int quantity, decimal value)
        {
            Id = id;
            ProductId = productId;
            CartClientId = cartClientId;
            Name = name;
            Quantity = quantity;
            Value = value;
        }

        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid CartClientId { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal Value { get; private set; }
    }
}