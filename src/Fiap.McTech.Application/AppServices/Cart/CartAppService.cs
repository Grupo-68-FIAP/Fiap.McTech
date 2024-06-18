using AutoMapper;
using Fiap.McTech.Application.Dtos.Cart;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Cart
{
    /// <summary>
    /// Provides functionality to manage shopping carts.
    /// </summary>
    public class CartAppService : ICartAppService
    {

        private readonly ILogger<CartAppService> _logger;
        private readonly IClientAppService _clientAppService;
        private readonly IProductAppService _productAppService;
        private readonly ICartClientRepository _cartClientRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartAppService"/> class.
        /// </summary>
        /// <param name="cartClientRepository">The repository for cart clients.</param>
        /// <param name="productAppService">The service for managing products.</param>
        /// <param name="clientAppService">The service for managing clients.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">The mapper.</param>
        public CartAppService(
            ICartClientRepository cartClientRepository,
            IProductAppService productAppService,
            IClientAppService clientAppService,
            ILogger<CartAppService> logger,
            IMapper mapper)
        {
            _cartClientRepository = cartClientRepository;
            _clientAppService = clientAppService;
            _productAppService = productAppService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<CartClientOutputDto?> GetCartByIdAsync(Guid id)
        {

            _logger.LogInformation("Retrieving cart ID {Id}.", id);

            var cartClient = await GetCartAsync(id);

            _logger.LogInformation("Cart with ID {Id} retrieved successfully.", id);

            return _mapper.Map<CartClientOutputDto>(cartClient);
        }

        /// <inheritdoc/>
        public async Task<CartClientOutputDto?> GetCartByClientIdAsync(Guid clientId)
        {

            _logger.LogInformation("Retrieving cart with Client ID {Id}.", clientId);

            var cartClient = await _cartClientRepository.GetByClientIdAsync(clientId)
                ?? throw new EntityNotFoundException(string.Format("Cart with Client ID {0} not found.", clientId));

            _logger.LogInformation("Carts with Client ID {Id} retrieved successfully.", clientId);

            return _mapper.Map<CartClientOutputDto>(cartClient);

        }

        /// <inheritdoc/>
        public async Task<CartClientOutputDto> CreateCartClientAsync(CartClientInputDto cart)
        {
            var newCartClient = _mapper.Map<NewCartClientDto>(cart);

            if (newCartClient.ClientId != null)
            {
                await _clientAppService.GetClientByIdAsync((Guid) newCartClient.ClientId);
            }

            foreach (var item in newCartClient.Items)
            {
                var product = await _productAppService.GetProductByIdAsync(item.ProductId);
                item.Name = product.Name;
                item.Value = product.Value;
            }

            var cartClient = _mapper.Map<Domain.Entities.Cart.CartClient>(newCartClient);

            cartClient.CalculateValueCart();

            if (!cartClient.IsValid())
                throw new EntityValidationException("Cart invalid data.");

            _logger.LogInformation("Creating a new cart.");

            var createdCartClient = await _cartClientRepository.AddAsync(cartClient);

            _logger.LogInformation("Cart created successfully with ID {Id}.", createdCartClient.Id);

            return _mapper.Map<CartClientOutputDto>(createdCartClient);
        }

        /// <inheritdoc/>
        public async Task<CartClientOutputDto> AddCartItemToCartClientAsync(Guid cartId, Guid productId, int quantities = 1)
        {
            _logger.LogInformation("Adding new item to cart with ID {Id}.", cartId);

            var cartClient = await GetCartAsync(cartId);

            var productOutput = await _productAppService.GetProductByIdAsync(productId);

            var product = _mapper.Map<Product>(productOutput);

            cartClient.AddProduct(product, quantities);

            await _cartClientRepository.UpdateAsync(cartClient);

            _logger.LogInformation("Added new item to cart with ID {Id}.", cartId);

            return _mapper.Map<CartClientOutputDto>(await GetCartAsync(cartId));
        }

        /// <inheritdoc/>
        public async Task<CartClientOutputDto> RemoveCartItemFromCartClientAsync(Guid cartId, Guid productId)
        {
            _logger.LogInformation("Attempting to delete Product with ID {Id} from Cart with ID {CartId}.", productId, cartId);

            var cart = await GetCartAsync(cartId);

            cart.RemoveProduct(productId);

            await _cartClientRepository.UpdateAsync(cart);

            _logger.LogInformation("Product with ID {Id} deleted from Cart with ID {CartId} successfully.", productId, cartId);

            return _mapper.Map<CartClientOutputDto>(cart);
        }

        /// <inheritdoc/>
        public async Task DeleteCartClientAsync(Guid id)
        {
            _logger.LogInformation("Attempting to delete cart with ID: {Id}", id);

            var existingCart = await GetCartAsync(id);

            await _cartClientRepository.RemoveAsync(existingCart);

            _logger.LogInformation("Cart with ID {Id} deleted successfully.", id);
        }

        private async Task<CartClient> GetCartAsync(Guid id)
        {
            return await _cartClientRepository.GetByCartIdAsync(id)
                ?? throw new EntityNotFoundException(string.Format("Cart with ID {0} not found.", id));
        }
    }
}
