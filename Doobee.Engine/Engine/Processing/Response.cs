using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing
{
    internal abstract class Response
    {
        protected Response(string message, bool success) 
        {
            Message = message;
            Success = success;
        }
        public string Message { get; }
        public bool Success { get; }
    }
}
