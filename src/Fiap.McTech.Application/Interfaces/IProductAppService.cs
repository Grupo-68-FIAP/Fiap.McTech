﻿using Fiap.McTech.Application.Dtos.Products.Update;
using Fiap.McTech.Application.Dtos.Products;
using Fiap.McTech.Application.Dtos.Products.Delete;
using Fiap.McTech.Domain.Enums;
using Fiap.McTech.Application.Dtos.Products.Add;

namespace Fiap.McTech.Application.Interfaces
{
	public interface IProductAppService
	{
		Task<ProductOutputDto> GetProductByIdAsync(Guid productId);
		Task<List<ProductOutputDto>> GetAllProductsAsync();
		Task<ProductOutputDto> CreateProductAsync(CreateProductInputDto productDto);
		Task<ProductOutputDto> UpdateProductAsync(Guid productId, UpdateProductInputDto productDto);
		Task<DeleteProductOutputDto> DeleteProductAsync(Guid productId);
        Task<List<ProductOutputDto>> GetProductsByCategoryAsync(ProductCategory category);
    }
}