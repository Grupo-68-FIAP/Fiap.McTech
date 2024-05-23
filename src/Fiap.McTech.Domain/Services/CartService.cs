using Fiap.McTech.Domain.Entities.Cart;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Cart;

namespace Fiap.McTech.Domain.Services
{
    public class CartService
    {
        private readonly ClientService _clientService;
        private readonly ProductService _productService;
        private readonly ICartClientRepository _cartClientRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public CartService(
            ClientService clientService,
            ProductService productService,
            ICartClientRepository cartClientRepository,
            ICartItemRepository cartItemRepository)
        {
            _clientService = clientService;
            _productService = productService;
            _cartClientRepository = cartClientRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<CartClient> GetCartAsync(Guid guid)
        {
            return await _cartClientRepository.GetByCartIdAsync(guid)
                ?? throw new EntityNotFoundException(string.Format("Cart with ID {0} not found.", guid));
        }

        public async Task<CartClient> GetCartByClientIdAsync(Guid clientId)
        {
            return await _cartClientRepository.GetByClientIdAsync(clientId)
                ?? throw new EntityNotFoundException(string.Format("Cart with Client ID {0} not found.", clientId));
        }

        public async Task<CartClient> CreateCartClientAsync(CartClient newCartClient)
        {
            if (newCartClient.ClientId != null)
            {
                await _clientService.GetAsync((Guid)newCartClient.ClientId);
            }

            foreach (var item in newCartClient.Items)
            {
                var product = await _productService.GetAsync(item.ProductId);
                item.Name = product.Name;
                item.Value = product.Value;
                item.CartClientId = newCartClient.Id;
            }

            newCartClient.CalculateValueCart();

            if (!newCartClient.IsValid()) throw new EntityValidationException("Cart invalid data.");

            return await _cartClientRepository.AddAsync(newCartClient);
        }

        public async Task DeleteCartClientAsync(Guid id)
        {
            var existingCart = await GetCartAsync(id);
            await _cartClientRepository.RemoveAsync(existingCart);
        }

        public async Task<CartClient> AddCartItemToCartClientAsync(Guid id, Guid productId)
        {
            var cartClient = await GetCartAsync(id);

            var product = await _productService.GetAsync(productId);

            var newCartItem = new CartItem(product.Name, 1, product.Value, product.Id, cartClient.Id);

            await _cartItemRepository.AddAsync(newCartItem);

            cartClient.CalculateValueCart();

            await _cartClientRepository.UpdateAsync(cartClient);

            return await GetCartAsync(id);
        }

        public async Task<CartClient> RemoveCartItemFromCartClientAsync(Guid cartItemId)
        {

            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId) 
                ?? throw new EntityNotFoundException(string.Format("Item with ID {0} not found.", cartItemId));

            var cartClientId = cartItem.CartClientId;
            await _cartItemRepository.RemoveAsync(cartItem);

            var cartClient = await GetCartAsync(cartClientId);
            cartClient.CalculateValueCart();
            await _cartClientRepository.UpdateAsync(cartClient);

            return cartClient;
        }
    }
}
