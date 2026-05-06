using Application.DTOs.User;
using Application.DTOs.Users;
using Application.UseCases.RefreshToken;
using Application.UseCases.Users;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TiendaVirtualApi.Endpoints {
    public static class UserEndpoints {
        public static void MapUsersEndpoints(this IEndpointRouteBuilder app) {
            var group = app.MapGroup("/api/users").WithTags("Users");

            group.MapGet("/me", async (ClaimsPrincipal user, GetUserByIdUseCase getById) => {
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var person = await getById.ExecuteAsync(userId);
                return Results.Ok(person);
            }).WithName("GetMyInfo").WithSummary("Obtener tu perfil")
            .RequireAuthorization();

            group.MapGet("/{id:int}", async (int id, GetUserByIdUseCase idUseCase) => {
                try {
                    var person = await idUseCase.ExecuteAsync(id);
                    return Results.Ok(person);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetUserById").WithSummary("Obtener usuario por su id")
            .RequireAuthorization("AdminOnly")
            //Produces(200).Produces(404);
            //.Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);

            group.MapGet("/", async (GetAllUsersUseCase allUseCase) => {
                try {
                    var persons = await allUseCase.ExecuteAsync();
                    return Results.Ok(persons);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetAllUsers").WithSummary("Obtener usuarios")
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminOnly");

            group.MapPost("/", async (CreateUserDto user, CreateUserUseCase create) => {
                try {
                    var person = await create.ExecuteAsync(user);
                    return Results.Created($"/api/users/{person.Id}", person);
                } catch (InvalidOperationException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("CreateUser").WithSummary("Crea un usuario")
            .AllowAnonymous()
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPost("/login", async (HttpContext context, LoginUserDto user, LoginUserUseCase login) => {
                try {
                    var result = await login.Execute(user);
                    context.Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions {
                        HttpOnly = true,
                        //Secure = true,
                        Secure = context.Request.IsHttps,
                        SameSite = SameSiteMode.Strict,
                        //SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
                    return Results.Ok(result.User);
                } catch (UnauthorizedAccessException e) {
                    return Results.Unauthorized();
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("Login").WithSummary("Login del sistema")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPost("/refresh", async (HttpContext context, GeneralRefreshTokenUseCase rf) => {
                var refreshToken = context.Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken)) {
                    return Results.Unauthorized();
                }
                try {
                    var response = await rf.Execute(refreshToken);
                    context.Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions {
                        HttpOnly = true,
                        //Secure = true,
                        Secure = context.Request.IsHttps,
                        SameSite = SameSiteMode.Strict,
                        //SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
                    return Results.Ok(response.User);
                } catch (UnauthorizedAccessException e) {
                    return Results.Unauthorized();
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            });

            group.MapPut("/me", async (ClaimsPrincipal user, EditUserDto dto, UpdateUserUseCase update) => {
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (userId != dto.Id) {
                    return Results.BadRequest(new { error = "El id de la ruta no coincide con el que intenta editar" });
                }
                try {
                    var person = await update.ExecuteAsync(dto);
                    return Results.Ok(person);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("UpdateUser").WithSummary("Actualiza un usuario")
                .RequireAuthorization()
                .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);


            group.MapDelete("/me", async (ClaimsPrincipal user, DeleteUserUseCase delete) => {
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                try {
                    await delete.ExecuteAsync(userId);
                    //return Results.Ok($"Se ha eliminado el usuario con el id {id}");
                    return Results.NoContent();
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("DeleteMyAccount").WithSummary("Eliminar mi cuenta")
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:int}", async (int id, DeleteUserUseCase delete) => {
                try {
                    await delete.ExecuteAsync(id);
                    //return Results.Ok($"Se ha eliminado el usuario con el id {id}");
                    return Results.NoContent();
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("DeleteUser").WithSummary("Eliminar usuario por su id")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);

        }
    }
}
