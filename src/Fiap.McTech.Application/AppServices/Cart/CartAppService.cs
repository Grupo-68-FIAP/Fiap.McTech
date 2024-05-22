using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Dtos.Message;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Fiap.McTech.Domain.ValuesObjects;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Cart
{
    public class CartAppService : ICartAppService
    {

        private readonly ILogger<CartAppService> _logger;
        private readonly IClientAppService _clientAppService;
        private readonly IProductAppService _productAppService;
        private readonly ICartClientRepository _cartClientRepository;
        private readonly IMapper _mapper;

        public CartAppService(ILogger<CartAppService> logger, IClientAppService clientAppService, ICartClientRepository cartClientRepository, IProductAppService productAppService, IMapper mapper)
        {
            _logger = logger;
            _clientAppService = clientAppService;
            _productAppService = productAppService;
            _cartClientRepository = cartClientRepository;
            _mapper = mapper;
        }

        public async Task<CartClientOutputDto?> GetCartByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Retrieving cart ID {Id}.", id);

                var cartClient = await _cartClientRepository.GetByCartIdAsync(id);
                if (cartClient == null)
                {
                    _logger.LogInformation("Cart with ID {Id} not found.", id);
                    throw new EntityNotFoundException(string.Format("Cart with ID {0} not found.", id));
                }

                _logger.LogInformation("Cart with ID {Id} retrieved successfully.", id);

                return _mapper.Map<CartClientOutputDto>(cartClient);
            }
            catch (McTechException) { throw; }
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

                var cartClient = await _cartClientRepository.GetByClientIdAsync(clientId);
                if (cartClient == null)
                {
                    _logger.LogInformation("Cart with Client ID {Id} not found.", clientId);
                    throw new EntityNotFoundException(string.Format("Cart with Client ID {0} not found.", clientId));
                }

                _logger.LogInformation("Carts with Client ID {Id} retrieved successfully.", clientId);

                return _mapper.Map<CartClientOutputDto>(cartClient);
            }
            catch (McTechException) { throw; }
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
                //  It can create a cart with anonymity
                if (cartClientDto.ClientId != null)
                {
                    _logger.LogInformation("Checking client existence.");
                    await _clientAppService.GetClientByIdAsync((Guid)cartClientDto.ClientId);
                }
                else
                    _logger.LogInformation("Create a card with anonymous client.");


                var newCartClient = _mapper.Map<NewCartClientDto>(cartClientDto);

                _logger.LogInformation("Checking products existence.");
                foreach (var item in newCartClient.Items)
                {
                    var product = await _productAppService.GetProductByIdAsync(item.ProductId);
                    item.Name = product.Name;
                    item.Value = product.Value;
                }

                _logger.LogInformation("Creating a new cart.");
                var cartClient = _mapper.Map<Domain.Entities.Cart.CartClient>(newCartClient);
                cartClient.CalculateValueCart();

                var createdCartClient = await _cartClientRepository.AddAsync(cartClient);

                _logger.LogInformation("Cart created successfully with ID {Id}.", createdCartClient.Id);

                return _mapper.Map<CartClientOutputDto>(createdCartClient);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create cart");

                throw;
            }
        }

        public async Task<CartClientOutputDto> UpdateCartClientAsync(Guid id, CartClientInputDto cartClientDto)
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