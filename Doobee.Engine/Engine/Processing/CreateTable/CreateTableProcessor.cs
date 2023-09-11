using Doobee.Engine.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine.Processing.CreateTable
{
    internal class CreateTableProcessor : DdlProcessor<CreateTableStatement>
    {        

        public override Task<Response> Process<T>(T value, SchemaManager schemaManager)
        {
            throw new NotImplementedException();
        }
    }
}
