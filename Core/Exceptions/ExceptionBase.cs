using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public abstract class ExceptionBase : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string Title { get; set; }

        public ExceptionBase(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
