using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Responses
{
    public class ApiResponse
    {
         private int v;

        public ApiResponse()
        {
        }

        public ApiResponse(int v)
        {
            this.v = v;
        }

        public ApiResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }


        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request you have made",
                401 => "You are not Authorized",
                404 => "Resource was not found",
                500 => "Errors are the path to the dark side. Error leads to anger. Anger leads to hate. Hate leads to career change",
                _ => null
            };
        }
    }
}