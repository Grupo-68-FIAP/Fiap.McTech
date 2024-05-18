using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Dtos.Message;
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
        public async Task<CartClientOutputDto?> GetCartByIdAsync(Guid id)
		{
			try
			{
				_logger.LogInformation("Retrieving cart with Client ID {Id}.", id);

				var cartClient = await _cartClientRepository.GetByIdAsync(id);
				if (cartClient == null)
				{
					_logger.LogInformation("Cart with ID {Id} not found. Creating new.", id);
					return null; 
				}

				_logger.LogInformation("Cart with ID {Id} retrieved successfully.", id);

				return _mapper.Map<CartClientOutputDto>(cartClient);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to retrieve cart with client ID {Id}.", id);
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

				_logger.LogInformation("Cart created successfully with ID {Id}.", createdCartClient.Id);

				return _mapper.Map<CartClientOutputDto>(createdCartClient);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to create cart");
				throw;
			}
		}

        public async Task<CartClientOutputDto> UpdateCartClientAsync(Guid id, CartClientOutputDto cartClientDto)
		{
			try
			{
				var existingCart = await _cartClientRepository.GetByIdAsync(id);
				if (existingCart == null)
				{
					_logger.LogWarning("Cart with ID {Id} not found. Update aborted.", id);
					throw new InvalidOperationException("Cart not found.");
				}

				_logger.LogInformation("Updating cart with ID {Id}.", id);

				_mapper.Map(cartClientDto, existingCart);

				await _cartClientRepository.UpdateAsync(existingCart);

				_logger.LogInformation("Cart with ID {Id} updated successfully.", id);

				return _mapper.Map<CartClientOutputDto>(existingCart);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to update cart with ID {Id}.", id);
				throw;
			}
		}

		public async Task<MessageDto> DeleteCartClientAsync(Guid id)
		{
			try
			{
				_logger.LogInformation("Attempting to delete product with ID: {Id}", id);

				var existingCart = await _cartClientRepository.GetByIdAsync(id);
				if (existingCart == null)
				{
					_logger.LogWarning("Cart with ID {Id} not found. Deletion aborted.", id);

					return new MessageDto(false, "Cart not found.");
				}

				await _cartClientRepository.RemoveAsync(existingCart);

				_logger.LogInformation("Cart with ID {Id} deleted successfully.", id);

				return new MessageDto(true, "Cart deleted successfully.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete cart with ID: {Id}", id);
				throw;
			}
		}
    }
}