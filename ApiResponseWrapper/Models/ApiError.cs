using System;
using System.Collections.Generic;
using System.Text;

namespace ApiResponseWrapper.Models
{
    public class ApiError
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
