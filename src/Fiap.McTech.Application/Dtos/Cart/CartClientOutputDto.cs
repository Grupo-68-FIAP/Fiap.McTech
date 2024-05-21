using Fiap.McTech.Domain.Entities.Cart;

namespace Fiap.McTech.Application.Dtos.Cart
{
    public class CartClientOutputDto
	{
        public CartClientOutputDto(Guid id, Guid? clientId, decimal allValue, List<CartItemOutputDto> items)
        {
            Id = id;
            ClientId = clientId;
            AllValue = allValue;
            Items = items;
        }

        public Guid Id { get; set; }
		public Guid? ClientId { get; set; }
		public decimal AllValue { get; set; }
		public List<CartItemOutputDto> Items { get; set; } = new List<CartItemOutputDto>();
	}
}