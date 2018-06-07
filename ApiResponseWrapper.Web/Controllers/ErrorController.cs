using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiResponseWrapper.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiResponseWrapper.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("api/error/notfound")]
        public IActionResult NotFoundError()
        {
            return this.NotFound();
        }

        [HttpGet("api/error/unauthorized")]
        public IActionResult UnAuthorizedError()
        {
            return this.Unauthorized();
        }

        [HttpGet("api/error/badrequest")]
        public IActionResult BadRequestError()
        {
            return this.BadRequest();
        }

        [HttpGet("api/error/apiexception")]
        public IActionResult ApiException()
        {
            throw new ApiException(100, "This is an error message");
        }

        [HttpGet("api/error/exception")]
        public IActionResult Exception()
        {
            throw new Exception("generic exception");
        }
    }
}