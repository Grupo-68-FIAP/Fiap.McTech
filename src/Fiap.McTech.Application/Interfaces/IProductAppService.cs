using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Add;
using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Domain.Enums;

namespace Fiap.McTech.Application.Interfaces
{
    /// <summary>
    /// Interface for managing products.
    /// </summary>
    public interface IProductAppService
    {
        /// <summary>
        /// Asynchronously retrieves all products available in the system.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. The resulting task contains a list of product output data transfer objects (DTOs) (<see cref="ProductOutputDto"/>).
        /// </returns>
        /// <remarks>
        /// This method is asynchronous, allowing other operations to continue while products are being retrieved.
        /// </remarks>
        Task<List<ProductOutputDto>> GetAllProductsAsync();

        /// <summary>
        /// Asynchronously retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The resulting task contains the product output data transfer object (DTO) (<see cref="ProductOutputDto"/>) corresponding to the specified identifier.
        /// </returns>
        /// <remarks>
        /// This method is asynchronous, allowing other operations to continue while the product is being retrieved.
        /// </remarks>
        Task<ProductOutputDto> GetProductByIdAsync(Guid productId);


        /// <summary>
        /// Retrieves products by category asynchronously.
        /// </summary>
        /// <param name="category">The category of products to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of product output DTOs.</returns>
        Task<List<ProductOutputDto>> GetProductsByCategoryAsync(ProductCategory category);

        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        /// <param name="productDto">The input DTO containing information for creating the product.</param>
        /// <returns>A task representing the asynchronous operation that returns the created product output DTO.</returns>
        Task<ProductOutputDto> CreateProductAsync(CreateProductInputDto productDto);

        /// <summary>
        /// Updates a product asynchronously.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to update.</param>
        /// <param name="productDto">The input DTO containing information for updating the product.</param>
        /// <returns>A task representing the asynchronous operation that returns the updated product output DTO.</returns>
        Task<ProductOutputDto> UpdateProductAsync(Guid productId, UpdateProductInputDto productDto);

        /// <summary>
        /// Deletes a product asynchronously.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteProductAsync(Guid productId);
    }
}
