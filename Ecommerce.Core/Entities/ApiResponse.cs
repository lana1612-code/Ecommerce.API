using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class ApiResponse
    {
        public int? StatusCode {  get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public object?   Result { get; set; }
        public ApiResponse(int? statusCode =null , string? message = null , object? result =null) { 
        StatusCode = statusCode;
            Result = result;
            IsSuccess = statusCode >= 200 && statusCode <= 300;
            Message = message ?? getMessageForStatusCode(statusCode);

        }
        private string? getMessageForStatusCode(int? statusCode)
        {
            return statusCode switch
            {
                200 => "Successfully",
                201 => "Create Successfully",
                400 => "Bad Request",
                404 => "Not Found",
                500 => "Internal server error",
                _ => null
            };
        }
    }
}
