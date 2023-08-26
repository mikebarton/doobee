using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Sql
{
    internal class CreateTableStatement
    {
        private string CreateTableTemplate = "create table {0} ({1})";
        private string TableNameRegex = "\\w{1,20}";
        private string ColumnTemplate = "[short|int|long|text|binary|bit|]";


    }
}
