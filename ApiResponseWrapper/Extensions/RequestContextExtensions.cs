using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiResponseWrapper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ApiResponseWrapper.Extensions
{
    public static class ApiRequestContextExtensions
    {
        public const string ApiResponseWrapperRequestIdHeaderKey = "apiResponseWrapperRequestId";

        public static IServiceCollection AddApiRequestContext(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (services.All(s => s.ServiceType != typeof(IApiRequestContext)))
            {
                services.AddScoped<IApiRequestContext, ApiRequestContext>(sp =>
                {
                    var http = sp.GetService<IHttpContextAccessor>();
                    return new ApiRequestContext(http?.HttpContext?.Request?.Headers?[ApiResponseWrapperRequestIdHeaderKey]);
                });
            }

            return services;
        }
    }
}
