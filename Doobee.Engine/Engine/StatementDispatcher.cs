using Doobee.Engine.Engine.Processing;
using Doobee.Engine.Engine.Processing.CreateTable;
using Doobee.Engine.Schema;
using Doobee.Engine.Storage;
using Doobee.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine
{
    internal class StatementDispatcher
    {
        private readonly IEnumerable<ProcessorBase> _processors;

        public StatementDispatcher(IEnumerable<ProcessorBase> processors)
        {
            _processors = processors;
        }

        public async Task<List<Response>> ProcessStatements(IReadOnlyList<Statement> statements)
        {
            var responses = statements.Select(async x =>
            {
                var processor = _processors.Single(y => y.CanProcess(x));
                var response = await processor.Process(x);
                return response;
            }).ToList();

            await Task.WhenAll(responses);

            return responses.Select(x=>x.Result).ToList();
        }
    }
}
