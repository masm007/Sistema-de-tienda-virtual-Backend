using Application.DTOs.Images;
using Application.DTOs.Products;
using Application.DTOs.User;
using Application.UseCases.Product;
using Application.UseCases.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaVirtualApi.Request;

namespace TiendaVirtualApi.Endpoints {
    public static class ProductEndpoints {
        public static void MapProductsEndpoints(this IEndpointRouteBuilder app) {
            var group = app.MapGroup("/api/products").WithTags("Products");

            group.MapGet("/{id:int}", async (int id, GetProductByIdUseCase getByIdUseCase) => {
                try {
                    var product = await getByIdUseCase.ExecuteAsync(id);
                    return Results.Ok(product);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetProductById").WithSummary("Obtener producto por su id")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);

            group.MapGet("/", async (GetAllActiveProductsUseCase allActivesUseCase) => {
                try {
                    var products = await allActivesUseCase.ExecuteAsync();
                    return Results.Ok(products);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetAllActiveProducts").WithSummary("Obtener todos los productos activos")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);

            group.MapGet("/admin", async (GetAllProductsUseCase allUseCase) => {
                try {
                    var products = await allUseCase.ExecuteAsync();
                    return Results.Ok(products);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetAllProducts").WithSummary("Obtener todos los productos")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);

            
            group.MapPost("/", async ([FromForm] CreateProductRequest req, CreateProductUseCase create) => {
                if (req.Images == null || req.Images.Count == 0) {
                    return Results.BadRequest(new { error = "Debe enviar al menos una imagen" });
                }
                try {
                    var prd = new CreateProductDto(req.Name, req.Description, req.Price, req.Quantity,
                        req.Images.Select(img => new ProductImageUploadDto(img.OpenReadStream(), 
                        img.FileName)).ToList(), req.Sku, req.CategoryId);
                    var product = await create.ExecuteAsync(prd);
                    return Results.Created($"/api/products/{product.Id}", product);
                } catch (InvalidOperationException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("CreateProduct").WithSummary("Crear un producto")
            .RequireAuthorization("AdminOnly")
            // Se deshabilita Antiforgery porque la API usa autenticación JWT (Bearer)
            // y no formularios/cookies de sesión tradicionales.
            .DisableAntiforgery()
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);
            
            group.MapDelete("/{id:int}", async (int id, DeleteProductUseCase deleteUseCase) => {
                try {
                    await deleteUseCase.ExecuteAsync(id);
                    return Results.NoContent();
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("DeleteProduct").WithSummary("Eliminar producto por su id")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);

        }
    }
}
