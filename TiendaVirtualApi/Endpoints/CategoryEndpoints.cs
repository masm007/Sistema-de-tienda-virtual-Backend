using Application.DTOs.Categories;
using Application.DTOs.Products;
using Application.DTOs.User;
using Application.UseCases.Category;
using Application.UseCases.Product;
using Application.UseCases.Users;
using System.Security.Claims;

namespace TiendaVirtualApi.Endpoints {
    public static class CategoryEndpoints {
        public static void MapCategoriesEndpoints(this IEndpointRouteBuilder app) {
            var group = app.MapGroup("/api/categories").WithTags("Categories");

            group.MapGet("/{id:int}", async (int id, GetCategoryByIdUseCase getByIdUseCase) => {
                try {
                    var category = await getByIdUseCase.ExecuteAsync(id);
                    return Results.Ok(category);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetCategoryById").WithSummary("Obtener una categoria por su id")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);

            group.MapGet("/", async (GetAllCategoriesUseCase allUseCase) => {
                try {
                    var products = await allUseCase.ExecuteAsync();
                    return Results.Ok(products);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetAllCategories").WithSummary("Obtener todas las categorias")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (CreateCategoryDto cat, CreateCategoryUseCase create) => {
                try {
                    var category = await create.ExecuteAsync(cat);
                    return Results.Created($"/api/categories/{category.Id}", category);
                } catch (InvalidOperationException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("CreateCategory").WithSummary("Crear una categoria")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:int}", async (int id, CategoryDto dto, UpdateCategoryUseCase update) => {
                if (id != dto.Id) {
                    return Results.BadRequest(new {
                        error = "El id de la ruta no coincide con el del cuerpo."
                    });
                }
                try {
                    var category = await update.ExecuteAsync(dto);
                    return Results.Ok(category);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("UpdateCategory").WithSummary("Actualiza una categoria")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:int}", async (int id, DeleteCategoryUseCase deleteUseCase) => {
                try {
                    await deleteUseCase.ExecuteAsync(id);
                    return Results.NoContent();
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("DeleteCategory").WithSummary("Eliminar categoria por su id")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);

        }
    }
}
