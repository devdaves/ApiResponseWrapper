using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ApiResponseWrapper
{
    public class ApiResponse<T> where T: class
    {
        public string RequestId { get; set; }

        public int StatusCode { get; set; }

        public T Result { get; set; }

        public ApiError Error { get; set; }

        public ApiResponse(HttpStatusCode statusCode, string requestId, T result = null, ApiError error = null)
        {
            StatusCode = (int)statusCode;
            RequestId = requestId;
            Result = result;
            Error = error;
        }
    }
}
