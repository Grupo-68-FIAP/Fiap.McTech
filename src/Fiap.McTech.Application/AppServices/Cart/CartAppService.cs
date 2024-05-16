using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Cart
{
    public class CartAppService : ICartAppService
    {

        private readonly ILogger<CartAppService> _logger;
		private readonly ICartClientRepository _cartClientRepository;

        private readonly ICartItemRepository _cartItemRepository;
		private readonly IMapper _mapper;

        public CartAppService(ILogger<CartAppService> logger, ICartClientRepository cartClientRepository, ICartItemRepository cartItemRepository, IMapper mapper)
		{
			_logger = logger;
			_cartClientRepository = cartClientRepository;
            _cartItemRepository = cartItemRepository;
			_mapper = mapper;
		}

        // Evoluir para vincular o carrinho ao cliente e buscar o carrinho por um identificador exclusivo, seja o Guid ou cpf/email
        public async Task<CartClientOutputDto?> GetCartByClientIdAsync(Guid clientId)
		{
			try
			{
				_logger.LogInformation("Retrieving cart with Client ID {ClientId}.", clientId);

				var cartClient = await _cartClientRepository.GetByClientIdAsync(clientId);
				if (cartClient == null)
				{
					_logger.LogInformation("Cart with ID {ClientId} not found. Creating new.", clientId);
					return new CartClientOutputDto(clientId); 
				}

				_logger.LogInformation("Cart with ID {ClientId} retrieved successfully.", clientId);

				return _mapper.Map<CartClientOutputDto>(cartClient);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to retrieve cart with client ID {ClientId}.", clientId);
				throw;
			}
		}

        public async Task<CartClientOutputDto> CreateCartClientAsync(CartClientOutputDto cartClientDto)
		{
			try
			{
				_logger.LogInformation("Creating a new cart.");

				var cartClient = _mapper.Map<Domain.Entities.Cart.CartClient>(cartClientDto);

				var createdCartClient = await _cartClientRepository.AddAsync(cartClient);

				_logger.LogInformation("Cart created successfully with ID {ClientId}.", createdCartClient.Id);

				return _mapper.Map<CartClientOutputDto>(createdCartClient);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to create cart");
				throw;
			}
		}

        public async Task<CartClientOutputDto> UpdateCartClientAsync(Guid clientId, CartClientOutputDto cartClientDto)
		{
			try
			{
				var existingCart = await _cartClientRepository.GetByIdAsync(clientId);
				if (existingCart == null)
				{
					_logger.LogWarning("Cart with ID {ClientId} not found. Update aborted.", clientId);
					throw new InvalidOperationException("Cart not found.");
				}

				_logger.LogInformation("Updating cart with ID {ClientId}.", clientId);

				_mapper.Map(cartClientDto, existingCart);

				await _cartClientRepository.UpdateAsync(existingCart);

				_logger.LogInformation("Cart with ID {ClientId} updated successfully.", clientId);

				return _mapper.Map<CartClientOutputDto>(existingCart);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to update cart with ID {ClientId}.", clientId);
				throw;
			}
		}

		public async Task<String> DeleteCartClientAsync(Guid clientId)
		{
			try
			{
				_logger.LogInformation("Attempting to delete product with ID: {ClientId}", clientId);

				var existingCart = await _cartClientRepository.GetByIdAsync(clientId);
				if (existingCart == null)
				{
					_logger.LogWarning("Cart with ID {ClientId} not found. Deletion aborted.", clientId);

					return "Cart not found.";
				}

				await _cartClientRepository.RemoveAsync(existingCart);

				_logger.LogInformation("Cart with ID {ClientId} deleted successfully.", clientId);

				return "Cart deleted successfully.";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete cart with ID: {ClientId}", clientId);
				throw;
			}
		}
    }
}