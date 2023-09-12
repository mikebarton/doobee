using Doobee.Engine.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing
{
    internal abstract class DmlProcessor<T> : ProcessorBase<T>
    {
        public override Type StatementType => typeof(T);
    }
}
