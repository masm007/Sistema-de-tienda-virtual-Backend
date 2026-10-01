using Application.UseCases.Settings;
using TiendaVirtualApi.Request;

namespace TiendaVirtualApi.Endpoints {
    public static class TaxSettingEndpoints {
        public static void MapTaxSettingEndpoints(this IEndpointRouteBuilder app) {
            var group = app.MapGroup("/api/settings/tax").WithTags("Settings");

            group.MapGet("/", async (GetTaxSettingUseCase getTax) => {
                try {
                    var setting = await getTax.ExecuteAsync();
                    return Results.Ok(setting);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                }
            }).WithName("GetTaxSetting").WithSummary("Obtener el porcentaje de IVA configurado")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            group.MapPut("/", async (UpdateTaxSettingRequest req, UpdateTaxSettingUseCase update) => {
                try {
                    var setting = await update.ExecuteAsync(req.IvaPercentage);
                    return Results.Ok(setting);
                } catch (InvalidOperationException e) {
                    return Results.NotFound(new { error = e.Message });
                } catch (ArgumentException e) {
                    return Results.BadRequest(new { error = e.Message });
                } catch (Exception) {
                    return Results.InternalServerError("Ocurrió un error interno");
                }
            }).WithName("UpdateTaxSetting").WithSummary("Actualizar el porcentaje de IVA")
            .RequireAuthorization("AdminOnly")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
