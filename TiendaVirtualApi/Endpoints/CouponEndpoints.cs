using Application.DTOs.Coupons;
using Application.UseCases.Coupons;
using TiendaVirtualApi.Request;

namespace TiendaVirtualApi.Endpoints {
    public static class CouponEndpoints {
        public static void MapCouponsEndpoints(this IEndpointRouteBuilder app) {
            var group = app.MapGroup("/api/coupons").WithTags("Coupons");

            group.MapGet("/", async (GetAllCouponsUseCase getAll) => {
                var coupons = await getAll.ExecuteAsync();
                return Results.Ok(coupons);
            }).WithName("GetAllCoupons").WithSummary("Obtener todos los cupones")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK);

            group.MapPost("/", async (CreateCouponDto dto, CreateCouponUseCase create) => {
                try {
                    var coupon = await create.ExecuteAsync(dto);
                    return Results.Created($"/api/coupons/{coupon.Id}", coupon);
                } catch (InvalidOperationException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("CreateCoupon").WithSummary("Crear un cupón")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            group.MapPost("/validate", async (ValidateCouponRequest req, ValidateCouponUseCase validate) => {
                try {
                    var result = await validate.ExecuteAsync(req.Code, req.Details);
                    return Results.Ok(result);
                } catch (InvalidOperationException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                }
            }).WithName("ValidateCoupon").WithSummary("Validar un cupón contra el carrito antes de comprar")
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            group.MapPatch("/{id:int}/deactivate", async (int id, DeactivateCouponUseCase deactivate) => {
                try {
                    await deactivate.ExecuteAsync(id);
                    return Results.NoContent();
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("DeactivateCoupon").WithSummary("Desactivar un cupón")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
