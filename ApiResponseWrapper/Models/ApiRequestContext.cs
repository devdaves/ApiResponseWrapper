using System;
using System.Collections.Generic;
using System.Text;

namespace ApiResponseWrapper.Models
{
    public interface IApiRequestContext
    {
        string Id { get; }
    }

    public class ApiRequestContext : IApiRequestContext
    {
        public string Id { get; }

        public ApiRequestContext(string id)
        {
            this.Id = id ?? Guid.NewGuid().ToString();
        }
    }
}
