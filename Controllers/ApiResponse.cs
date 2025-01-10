using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_Web_API.Controllers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCods { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        // constructor for successfull response
        private ApiResponse(bool success, T? data, int statusCode, List<string>? errors , string message = "")
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCods = statusCode;
            Errors = errors;
            TimeStamp = DateTime.UtcNow;
        }

        // static method to create a successfull response
        public static ApiResponse<T> SuccessResponse(T data, int statusCode, string message = "")
        {
            return new ApiResponse<T>(true, data, statusCode, null, message);
        }

        // static method to create an error response
        public static ApiResponse<T> ErrorResponse(List<string> errors, int statusCode, string message = "")
        {
            return new ApiResponse<T>(false, default(T), statusCode, errors, message);
        }




    }
}