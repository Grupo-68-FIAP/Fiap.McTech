using AutoMapper;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Add;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Interfaces;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;
using Microsoft.Extensions.Logging;

namespace Fiap.McTech.Application.AppServices.Product
{
    /// <summary>
    /// Service for managing products.
    /// </summary>
    public class ProductAppService : IProductAppService
    {
        private readonly ILogger<ProductAppService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAppService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="productRepository">The product repository.</param>
        /// <param name="mapper">The mapper.</param>
        public ProductAppService(
            ILogger<ProductAppService> logger,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _logger = logger;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<ProductOutputDto> CreateProductAsync(CreateProductInputDto productDto)
        {
            try
            {
                _logger.LogInformation("Creating a new product.");

                var product = _mapper.Map<Domain.Entities.Products.Product>(productDto);

                if (!product.IsValid()) throw new EntityValidationException("Product invalid data.");

                var createdProduct = await _productRepository.AddAsync(product);

                _logger.LogInformation("Product created successfully with ID {ProductId}.", createdProduct.Id);

                return _mapper.Map<ProductOutputDto>(createdProduct);
            }
            catch (McTechException ex)
            {
                _logger.LogError(ex, "Application: {msg}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create product");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ProductOutputDto> GetProductByIdAsync(Guid productId)
        {
            try
            {
                _logger.LogInformation("Retrieving product with ID {ProductId}.", productId);

                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogInformation("Product with ID {ProductId} not found.", productId);
                    throw new EntityNotFoundException(string.Format("Product with ID {0} not found.", productId));
                }

                _logger.LogInformation("Product with ID {ProductId} retrieved successfully.", productId);

                return _mapper.Map<ProductOutputDto>(product);
            }
            catch (McTechException) { throw; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve product with ID {ProductId}.", productId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ProductOutputDto>> GetAllProductsAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all products.");

                var products = await _productRepository.GetAll();
                if (products == null || !products.Any())
                {
                    _logger.LogInformation("No products found.");
                    return new List<ProductOutputDto>();
                }

                _logger.LogInformation("Retrieved products successfully.");

                return _mapper.Map<List<ProductOutputDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve products");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ProductOutputDto> UpdateProductAsync(Guid productId, UpdateProductInputDto productDto)
        {
            try
            {
                var existingProduct = await _productRepository.GetByIdAsync(productId)
                    ?? throw new EntityNotFoundException(string.Format("Product with ID {0} not found.", productId));

                _logger.LogInformation("Updating product with ID {ProductId}.", productId);

                _mapper.Map(productDto, existingProduct);

                if (!existingProduct.IsValid()) throw new EntityValidationException("Product invalid data.");

                await _productRepository.UpdateAsync(existingProduct);

                _logger.LogInformation("Product with ID {ProductId} updated successfully.", productId);

                return _mapper.Map<ProductOutputDto>(existingProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update product with ID {ProductId}.", productId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task DeleteProductAsync(Guid productId)
        {
            try
            {
                _logger.LogInformation("Attempting to delete product with ID: {ProductId}", productId);

                var existingProduct = await _productRepository.GetByIdAsync(productId)
                    ?? throw new EntityNotFoundException(string.Format("Product with ID {0} not found.", productId));

                await _productRepository.RemoveAsync(existingProduct);

                _logger.LogInformation("Product with ID {ProductId} deleted successfully.", productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product with ID: {ProductId}", productId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ProductOutputDto>> GetProductsByCategoryAsync(ProductCategory category)
        {
            try
            {
                _logger.LogInformation("Retrieving products by category {category}.", category);

                var products = await _productRepository.GetProductsByCategoryAsync(category);

                return _mapper.Map<List<ProductOutputDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products by category {Category}.", category);
                throw;
            }
        }
    }
}
