using System;
using Fiap.McTech.Application.Dtos.Cart;

namespace Fiap.McTech.Application.Interfaces
{
    public interface ICartAppService
    {
        Task<CartClientOutputDto> GetCartByClientIdAsync(Guid clientId);
        Task<CartClientOutputDto> CreateCartClientAsync(CartClientOutputDto cart);
        Task<CartClientOutputDto> UpdateCartClientAsync(Guid clientId, CartClientOutputDto cart);
        Task<String> DeleteCartClientAsync(Guid clientId);
    }
}