using System;
using System.Collections.Generic;
using System.Text;

namespace ApiResponseWrapper.Models
{
    public class ApiException : Exception
    {
        public int Code { get; set; }

        public ApiException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
