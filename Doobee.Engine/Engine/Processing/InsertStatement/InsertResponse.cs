using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing.Insert
{
    internal class InsertResponse : Response
    {
        public InsertResponse(string message, bool success) : base(message, success)
        {
        }
    }
}
