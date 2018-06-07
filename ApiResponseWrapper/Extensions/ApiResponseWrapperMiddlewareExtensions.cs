using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ApiResponseWrapper.Extensions
{
    public static class ApiResponseWrapperMiddlewareExtensions
    {
        public static IServiceCollection AddApiResponseMiddleware(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (services.All(s => s.ServiceType != typeof(ApiResponseWrapperMiddleware)))
            {
                services.AddTransient<ApiResponseWrapperMiddleware>();
            }

            if (services.All(s => s.ServiceType != typeof(JsonSerializerSettings)))
            {
                services.AddSingleton<JsonSerializerSettings>(sp => new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                });
            }

            return services;
        }

        public static void UseApiResponseWrapperMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiResponseWrapperMiddleware>();
        }
    }
}
