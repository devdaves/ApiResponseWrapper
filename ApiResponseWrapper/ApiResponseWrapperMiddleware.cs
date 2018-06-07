using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using ApiResponseWrapper.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiResponseWrapper
{
    public class ApiResponseWrapperMiddleware : IMiddleware
    {
        private readonly JsonSerializerSettings settings;
        private readonly IApiRequestContext apiRequestContext;

        public ApiResponseWrapperMiddleware(JsonSerializerSettings settings, IApiRequestContext apiRequestContext)
        {
            this.settings = settings;
            this.apiRequestContext = apiRequestContext;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Path.Value.ToLower().Contains("swagger"))
            {
                var currentBody = context.Response.Body;

                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    try
                    {
                        await next(context);
                        
                        context.Response.Body = currentBody;
                        await this.UpdateContextResponse(context, GetActionResult(memoryStream), BuildError(context.Response));
                    }
                    catch (Exception exception)
                    {
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        context.Response.Body = currentBody;
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        await this.UpdateContextResponse(context, null, BuildError(exception));
                    }
                }
            }
            else
            {
                await next(context); // let swagger request go through
            }
        }

        private static object GetActionResult(MemoryStream memoryStream)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            return JsonConvert.DeserializeObject(new StreamReader(memoryStream).ReadToEnd());
        }

        private async Task UpdateContextResponse(HttpContext context, object currentBodyObject, ApiError error = null)
        {
            var result = new ApiResponse<object>(
                (HttpStatusCode)context.Response.StatusCode, 
                apiRequestContext.Id, 
                currentBodyObject, 
                error);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(result, settings));
        }

        private ApiError BuildError(HttpResponse response)
        {
            if (IsSuccessStatusCode(response.StatusCode))
            {
                return null;
            }

            return new ApiError
            {
                Code = response.StatusCode,
                Message = ((HttpStatusCode)response.StatusCode).ToString()
            };
        }

        private ApiError BuildError(Exception exception)
        {
            var error = new ApiError
            {
#if DEBUG
                StackTrace = exception.StackTrace
#endif
            };
            
            if (exception is ApiException ex)
            {
                error.Code = ex.Code;
                error.Message = ex.Message;
            }
            else
            {
                error.Code = -1;
#if DEBUG
                error.Message = exception.GetBaseException().Message;
#else
                error.message = "An unhandled error occured";
#endif
            }

            return error;
        }

        private bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= 200 && statusCode <= 299;
        }
    }
}
