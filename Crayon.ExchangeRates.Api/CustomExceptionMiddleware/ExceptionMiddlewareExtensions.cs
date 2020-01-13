﻿using Microsoft.AspNetCore.Builder;

namespace Crayon.ExchangeRates.Api.CustomExceptionMiddleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}