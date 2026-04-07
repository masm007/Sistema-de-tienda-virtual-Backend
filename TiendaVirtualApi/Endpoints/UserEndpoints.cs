using Application.DTOs.User;
using Application.DTOs.Users;
using Application.UseCases.Users;

namespace TiendaVirtualApi.Endpoints {
    public static class UserEndpoints {
        public static void MapUsersEndpoints(this IEndpointRouteBuilder app) {
            var group = app.MapGroup("/api/users").WithTags("Users").RequireAuthorization();

            group.MapGet("/{id:int}", async (int id, GetUserByIdUseCase idUseCase) => {
                try {
                    var person = await idUseCase.ExecuteAsync(id);
                    return Results.Ok(person);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetUserById").WithSummary("Obtener usuario por su id")
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
            .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound);

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

            group.MapPost("/login", async (LoginUserDto user, LoginUserUseCase login) => {
                try {
                    var person = await login.Execute(user);
                    return Results.Ok(person);
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

            group.MapPut("/{id:int}", async (int id, EditUserDto user, UpdateUserUseCase update) => {
                if (id != user.Id) {
                    return Results.BadRequest(new { error = "El id de la ruta no coincide con el del cuerpo" });
                }
                try {
                    var person = await update.ExecuteAsync(user);
                    return Results.Ok(person);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception e) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("UpdateUser").WithSummary("Actualiza un usuario")
                .Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

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
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);

        }
    }
}
