using System;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Dtos.Message;

namespace Fiap.McTech.Application.Interfaces
{
    public interface ICartAppService
    {
        Task<CartClientOutputDto?> GetCartByIdAsync(Guid clientId);
        Task<CartClientOutputDto> CreateCartClientAsync(CartClientInputDto cart);
        Task<CartClientOutputDto> UpdateCartClientAsync(Guid clientId, CartClientOutputDto cart);
        Task<MessageDto> DeleteCartClientAsync(Guid clientId);
    }
}