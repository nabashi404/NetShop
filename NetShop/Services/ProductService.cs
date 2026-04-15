using FluentResults;
using Microsoft.EntityFrameworkCore;
using NetShop.Data;
using NetShop.Interfaces;
using NetShop.Models.Dtos;
using Stripe;
using Product = NetShop.Models.Entities.Product;

namespace NetShop.Services;

public class ProductService(ApplicationDbContext context) : IProductService
{
    private readonly ApplicationDbContext _context = context;

    private readonly Stripe.ProductService _productService = new();
    private readonly PriceService _priceService = new();

    public async Task<Result<List<Product>>> GetAllAsync()
    {
        var products = await _context.Products.AsNoTracking().ToListAsync();

        return Result.Ok(products);
    }

    public async Task<Result<Product>> GetAsync(long id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return Result.Fail<Product>("Product not found");

        return Result.Ok(product);
    }

    public async Task<Result<Product>> CreateAsync(ProductDto productDto)
    {
        if (productDto.Price < 0) return Result.Fail("Price cannot be negative");

        var stripeProductOptions = new ProductCreateOptions
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Images = productDto.Images.Count() == 0 ? null : productDto.Images
        };

        var stripeProduct = await _productService.CreateAsync(stripeProductOptions);

        var unitAmount = (long)Math.Round(productDto.Price * 100m);

        var priceOptions = new PriceCreateOptions
        {
            UnitAmount = unitAmount,
            Currency = productDto.Currency.ToString(),
            Product = stripeProduct.Id
        };

        var stripePrice = await _priceService.CreateAsync(priceOptions);

        var product = new Product()
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Images = productDto.Images,
            Price = unitAmount,
            StockQuantity = productDto.StockQuantity,
            Status = productDto.Status,
            StripeProductId = stripeProduct.Id,
            StripePriceId = stripePrice.Id
        };

        await _context.Products.AddAsync(product);

        await _context.SaveChangesAsync();

        return Result.Ok(product);
    }

    public async Task<Result> UpdateAsync(long id, ProductDto productDto)
    {
        if (productDto.Price < 0) return Result.Fail("Price cannot be negative");

        var product = await _context.Products.FindAsync(id);

        if (product == null) return Result.Fail("Product not found");

        var stripeProductUpdateOptions = new ProductUpdateOptions
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Images = productDto.Images
        };

        await _productService.UpdateAsync(product.StripeProductId, stripeProductUpdateOptions);

        var unitAmount = (long)Math.Round(productDto.Price * 100m);

        var priceOptions = new PriceCreateOptions
        {
            UnitAmount = unitAmount,
            Currency = productDto.Currency.ToString(),
            Product = product.StripeProductId
        };

        var stripePrice = await _priceService.CreateAsync(priceOptions);

        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.Images = productDto.Images;
        product.Price = unitAmount;
        product.StockQuantity = productDto.StockQuantity;
        product.Status = productDto.Status;
        product.StripePriceId = stripePrice.Id;

        _context.Update(product);

        await _context.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(long id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return Result.Fail("Product not found");

        var updateOptions = new ProductUpdateOptions
        {
            Active = false
        };

        await _productService.UpdateAsync(product.StripeProductId, updateOptions);

        //await _productService.DeleteAsync(product.StripeProductId);

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return Result.Ok();
    }
}

