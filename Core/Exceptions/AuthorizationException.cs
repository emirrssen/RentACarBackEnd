using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class AuthorizationException : ExceptionBase
    {
        public AuthorizationException(string message) : base(HttpStatusCode.Unauthorized, message)
        {
            Title = "Authorization Denied!";
        }
    }
}
