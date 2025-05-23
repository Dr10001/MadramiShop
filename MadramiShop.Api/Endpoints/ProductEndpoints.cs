﻿using MadramiShop.Api.Services;
using MadramiShop.Shared.Dtos;
using Microsoft.AspNetCore.Builder;

namespace MadramiShop.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/products", async (ProductService service) 
            => Results.Ok(await service.GetProductsAsync()))
            .Produces<ProductDto[]>()
            .WithName("Products");
        return app;
    }

}
