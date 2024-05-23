using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Dtos.Message;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Cart
{
    public class CartAppService : ICartAppService
    {

        private readonly ILogger<CartAppService> _logger;
        private readonly CartService _cartService;
        private readonly IMapper _mapper;

        public CartAppService(ILogger<CartAppService> logger, CartService cartService, IMapper mapper)
        {
            _logger = logger;
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task<CartClientOutputDto?> GetCartByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving cart ID {Id}.", id);

                var cartClient = await _cartService.GetCartAsync(id);

                _logger.LogInformation("Cart with ID {Id} retrieved successfully.", id);

                return _mapper.Map<CartClientOutputDto>(cartClient);
            }
            catch (McTechException ex)
            {
                _logger.LogError(ex, "Domain: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve cart with client ID {Id}.", id);
                throw;
            }
        }

        public async Task<CartClientOutputDto?> GetCartByClientIdAsync(Guid clientId)
        {
            try
            {
                _logger.LogInformation("Retrieving cart with Client ID {Id}.", clientId);

                var cartClient = await _cartService.GetCartByClientIdAsync(clientId);

                _logger.LogInformation("Carts with Client ID {Id} retrieved successfully.", clientId);

                return _mapper.Map<CartClientOutputDto>(cartClient);
            }
            catch (McTechException ex)
            {
                _logger.LogError(ex, "Domain: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve cart with client ID {Id}.", clientId);
                throw;
            }
        }

        public async Task<CartClientOutputDto> CreateCartClientAsync(CartClientInputDto cartClientDto)
        {
            try
            {
                // TODO: remover a NewCartClientDto e fazer a conversão direta de CartClientInputDto -> CartClient
                var newCartClient = _mapper.Map<NewCartClientDto>(cartClientDto);

                var cartClient = _mapper.Map<Domain.Entities.Cart.CartClient>(newCartClient);

                _logger.LogInformation("Creating a new cart.");

                var createdCartClient = await _cartService.CreateCartClientAsync(cartClient);

                _logger.LogInformation("Cart created successfully with ID {Id}.", createdCartClient.Id);

                return _mapper.Map<CartClientOutputDto>(createdCartClient);
            }
            catch (McTechException ex)
            {
                _logger.LogError(ex, "Domain: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create cart");

                throw;
            }
        }

        public async Task<CartClientOutputDto> AddCartItemToCartClientAsync(Guid id, Guid productId)
        {
            try
            {
                _logger.LogInformation("Adding new item to cart with ID {Id}.", id);

                var cartClient = await _cartService.AddCartItemToCartClientAsync(id, productId);

                _logger.LogInformation("Added new item to cart with ID {Id}.", id);

                return _mapper.Map<CartClientOutputDto>(cartClient);
            }
            catch (McTechException ex)
            {
                _logger.LogError(ex, "Domain: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update cart");

                throw;
            }
        }

        public async Task<CartClientOutputDto> RemoveCartItemFromCartClientAsync(Guid cartItemId)
        {
            try
            {
                _logger.LogInformation("Attempting to delete Card Item ID {cartItemId}.", cartItemId);

                var cart = await _cartService.RemoveCartItemFromCartClientAsync(cartItemId);

                _logger.LogInformation("Cart Item with ID {Id} deleted successfully.", cartItemId);

                return _mapper.Map<CartClientOutputDto>(cart);
            }
            catch (McTechException ex)
            {
                _logger.LogError(ex, "Domain: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete cart item with ID: {Id}", cartItemId);
                throw;
            }
        }

        public async Task<MessageDto> DeleteCartClientAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete cart with ID: {Id}", id);

                await _cartService.DeleteCartClientAsync(id);

                _logger.LogInformation("Cart with ID {Id} deleted successfully.", id);

                return new MessageDto(true, "Cart deleted successfully.");
            }
            catch (McTechException ex)
            {
                _logger.LogError(ex, "Domain: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete cart with ID: {Id}", id);
                throw;
            }
        }
    }
}