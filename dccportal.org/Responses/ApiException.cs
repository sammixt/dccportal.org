using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dccportal.org.Responses
{
    public class ApiException : ApiResponse
    {
         public ApiException()
        {
        }

        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }
    }
}