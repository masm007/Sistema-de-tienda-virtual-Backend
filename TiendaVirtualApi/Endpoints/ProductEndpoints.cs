using Application.DTOs.Products;
using Application.DTOs.User;
using Application.UseCases.Product;
using Application.UseCases.Users;
using System.Security.Claims;

namespace TiendaVirtualApi.Endpoints {
    public static class ProductEndpoints {
        public static void MapProdutcsEndpoints(this IEndpointRouteBuilder app) {
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
            }).WithName("GetAllProducts").WithSummary("Obtener todos los productos activos")
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

            group.MapPost("/", async (CreateProductDto prd, CreateProductUseCase create) => {
                try {
                    var product = await create.ExecuteAsync(prd);
                    return Results.Created($"/api/products/{product.Id}", product);
                } catch (InvalidOperationException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("CreateProduct").WithSummary("Crear un producto")
            .RequireAuthorization("AdminOnly")
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
