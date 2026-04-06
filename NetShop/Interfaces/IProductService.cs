using FluentResults;
using NetShop.Models.Dtos;
using NetShop.Models.Entities;

namespace NetShop.Interfaces;

public interface IProductService
{
    Task<Result<List<Product>>> GetAllAsync();
    Task<Result<Product>> GetAsync(long id);
    Task<Result<Product>> CreateAsync(ProductDto productDto);
    Task<Result> UpdateAsync(long id, ProductDto productDto);
    Task<Result> DeleteAsync(long id);
}
