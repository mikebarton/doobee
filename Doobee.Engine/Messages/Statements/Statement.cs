using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Messages.Statements
{
    internal abstract class Statement
    {
        public bool IsDdl() => this is CreateTableStatement;
    }
}
