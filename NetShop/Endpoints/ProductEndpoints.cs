using Microsoft.AspNetCore.Authorization;
using NetShop.Interfaces;
using NetShop.Models.Dtos;

namespace NetShop.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("/api/products");

        var adminAuthorize = new AuthorizeAttribute { Roles = "Admin" };

        route.MapGet("/", GetAll);
        route.MapGet("/{id}", Get);
        route.MapPost("/", Create).RequireAuthorization(adminAuthorize);
        route.MapPut("/{id}", Update).RequireAuthorization(adminAuthorize);
        route.MapDelete("/{id}", Delete).RequireAuthorization(adminAuthorize);
    }

    static async Task<IResult> GetAll(IProductService service)
    {
        var result = await service.GetAllAsync();

        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
    }

    static async Task<IResult> Get(IProductService service, long id)
    {
        var result = await service.GetAsync(id);

        return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Errors);
    }

    static async Task<IResult> Create(IProductService service, ProductDto productDto)
    {
        var result = await service.CreateAsync(productDto);

        return result.IsSuccess ? Results.Created($"/api/products/{result.Value.Id}", result.Value) : Results.BadRequest(result.Errors);
    }

    static async Task<IResult> Update(IProductService service, long id, ProductDto productDto)
    {
        var result = await service.UpdateAsync(id, productDto);

        return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
    }

    static async Task<IResult> Delete(IProductService service, long id)
    {
        var result = await service.DeleteAsync(id);

        return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
    }
}

