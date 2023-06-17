using Microsoft.AspNetCore.Diagnostics;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Service.Exceptions;
using System.Text.Json;

namespace NLayerApp.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                // Run; middleware geçişe izin vermedi buradan geriye dön anlamına geliyor.
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500,
                    };

                    context.Response.StatusCode = statusCode;

                    CustomResponseDto<NoContentDto> response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });
            });
        }
    }
}
