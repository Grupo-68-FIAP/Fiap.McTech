﻿using System;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Dtos.Message;

namespace Fiap.McTech.Application.Interfaces
{
    public interface ICartAppService
    {
        Task<CartClientOutputDto?> GetCartByIdAsync(Guid id);
        Task<CartClientOutputDto?> GetCartByClientIdAsync(Guid clientId);
        Task<CartClientOutputDto> CreateCartClientAsync(CartClientInputDto cart);
        Task<CartClientOutputDto> AddCartItemToCartClientAsync(Guid id, Guid productId);
        Task<CartClientOutputDto> RemoveCartItemFromCartClientAsync(Guid cartItemId);
        Task<MessageDto> DeleteCartClientAsync(Guid clientId);
    }
}