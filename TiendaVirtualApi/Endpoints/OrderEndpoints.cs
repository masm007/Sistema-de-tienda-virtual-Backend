using Application.DTOs.Orders;
using Application.UseCases.Orders;
using Application.UseCases.Product;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TiendaVirtualApi.Endpoints {
    public static class OrderEndpoints {
        public static void MapOrdersEndpoints(this IEndpointRouteBuilder app) {
            var group = app.MapGroup("/api/orders").WithTags("Orders");

            group.MapGet("/admin/", async (GetAllOrdersUseCase getAllOrdersUseCase) => {
                   try {
                       var orders = await getAllOrdersUseCase.ExecuteAsync();
                       return Results.Ok(orders);
                   } catch (InvalidOperationException e) {
                       return Results.NotFound(new { error = e.Message });
                   }
               }).WithName("GetAllOrdersForAdmin")
            .WithSummary("Obtener todas las ordenes de todos los usuario para admin")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/admin/{orderNumber}", async (string orderNumber,
                GetOrderByOrderNumberForAdminUseCase getByOrdNumberForAdminUseCase) => {
                    try {
                        var order = await getByOrdNumberForAdminUseCase.ExecuteAsync(orderNumber);
                        return Results.Ok(order);
                    } catch (InvalidOperationException e) {
                        return Results.NotFound(new { error = e.Message });
                    }
                }).WithName("GetOrderByOrderNumberForAdmin")
            .WithSummary("Obtener una orden por su numero de orden para admin")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/{orderNumber}", async (string orderNumber, 
                GetOrderByOrderNumberForUserUseCase getByIdUseCase, HttpContext httpContext) => {
                try {
                    var userIdClaim = httpContext.User.FindFirst(
                    ClaimTypes.NameIdentifier);
                    if (userIdClaim == null) {
                        return Results.Unauthorized();
                    }
                    //conversion más segura
                    if (!int.TryParse(userIdClaim.Value, out int userId)) {
                        return Results.Unauthorized();
                    }
                    var order = await getByIdUseCase.ExecuteAsync(orderNumber, userId);
                    return Results.Ok(order);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetOrderByOrderNumberForUser")
            .WithSummary("Obtener una orden por su numero de orden para usuario")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

            group.MapGet("/", async (GetAllOrdersByUserIdUseCase getByIdUseCase, 
                HttpContext httpContext) => {
                    try {
                        var userIdClaim = httpContext.User.FindFirst(
                            ClaimTypes.NameIdentifier);
                        if (userIdClaim == null) {
                            return Results.Unauthorized();
                        }
                        //conversion más segura
                        if (!int.TryParse(userIdClaim.Value, out int userId)) {
                            return Results.Unauthorized();
                        }
                        var orders = await getByIdUseCase.ExecuteAsync(userId);
                        return Results.Ok(orders);
                    } catch (InvalidOperationException e) {
                        return Results.NotFound(new { error = e.Message });
                    }
                }).WithName("GetAllOrdersForUser")
            .WithSummary("Obtener todas las ordenes para un usuario")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPost("/", async (CreateOrderDto dto, CreateOrderUseCase createOrderUseCase, 
                HttpContext httpContext) => {
                try {
                    var userIdClaim = httpContext.User.FindFirst(
                    ClaimTypes.NameIdentifier);
                    if (userIdClaim == null) {
                        return Results.Unauthorized();
                    }
                    //conversion más segura
                    if (!int.TryParse(userIdClaim.Value, out int userId)) {
                        return Results.Unauthorized();
                    }
                    var order = await createOrderUseCase.ExecuteAsync(dto, userId);
                    return Results.Created($"/api/orders/{order.OrderNumber}", order);
                    } catch (InvalidOperationException e) {
                        return Results.NotFound(new { error = e.Message });
                    } catch (ArgumentException e) {
                        return Results.BadRequest(new { error = e.Message });
                    }
                }).WithName("CreateOrder").WithSummary("Crear una orden")
            .RequireAuthorization()
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPatch("/admin/{orderNumber}", async (string orderNumber, UpdateOrderDto dto, 
                UpdateOrderUseCase updateOrderUseCase) => {
                    try {
                        await updateOrderUseCase.ExecuteAsync(dto, orderNumber);
                        return Results.NoContent();
                    } catch (InvalidOperationException e) {
                        return Results.NotFound(new { error = e.Message });
                    } catch (ArgumentException e) {
                        return Results.BadRequest(new { error = e.Message });
                    }
                }).WithName("UpdateOrder").WithSummary("Actualizar el estado de una orden")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPatch("/admin/{orderNumber}/cancel", async (string orderNumber, CancelOrderUseCase cancelOrderUseCase) => {
                    try {
                        await cancelOrderUseCase.ExecuteAsync(orderNumber);
                        return Results.NoContent();
                    } catch (InvalidOperationException e) {
                        return Results.NotFound(new { error = e.Message });
                    } catch (ArgumentException e) {
                        return Results.BadRequest(new { error = e.Message });
                    }
                }).WithName("CancelOrder").WithSummary("Establecer cancelado como estado de una orden")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        }
    }
}
