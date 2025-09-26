using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.helpers
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Type => "Error";
        public int Code { get; set; }

        public ErrorResponse(string message, int code)
        {
            Message = message;
            Code = code;
        }
    }
}
