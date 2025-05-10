using Doobee.Engine.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing
{
    internal abstract class ProcessorBase
    {       
        public bool CanProcess(Statement statement)
        {
            return StatementType == statement.GetType();
        }

        public abstract Type StatementType { get; }

        public abstract Task<Response> Process(Statement value);
    }
}
