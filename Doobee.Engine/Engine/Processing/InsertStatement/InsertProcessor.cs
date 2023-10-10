using Doobee.Engine.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing.InsertStatement
{
    internal class InsertProcessor : DmlProcessor<InsertStatment>
    {
        protected override Task<Response> ProcessConcrete(InsertStatment value, SchemaManager schemaManager)
        {
            throw new NotImplementedException();
        }
    }
}
