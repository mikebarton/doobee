using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doobee.Engine.Engine.Statements.CreateTable;

namespace Doobee.Engine.Engine.Processing
{
    internal abstract class Statement
    {
        public bool IsDdl() => this is CreateTableStatement;
    }
}
