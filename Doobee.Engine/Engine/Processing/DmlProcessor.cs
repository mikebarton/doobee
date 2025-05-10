using Doobee.Engine.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing
{
    internal abstract class DmlProcessor<T> : ProcessorBase where T : Statement
    {
        public override Type StatementType => typeof(T);

        public override Task<Response> Process(Statement value)
        {
            return ProcessConcrete((T)value);
        }

        protected abstract Task<Response> ProcessConcrete(T value);
    }
}
