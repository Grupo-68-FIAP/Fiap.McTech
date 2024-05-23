using Fiap.McTech.Domain.Entities.Products;
using Fiap.McTech.Domain.Exceptions;
using Fiap.McTech.Domain.Interfaces.Repositories.Products;

namespace Fiap.McTech.Domain.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetAsync(Guid guid)
        {
            return await _productRepository.GetByIdAsync(guid)
                ?? throw new EntityNotFoundException(string.Format("Product with ID {0} not found.", guid));
        }
    }
}
