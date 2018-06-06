using System;
using System.Collections.Generic;
using System.Text;

namespace ApiResponseWrapper
{
    public class ApiError
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
