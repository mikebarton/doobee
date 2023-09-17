using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Engine
{
    internal class DatabaseConfiguration
    {
        public Guid EngineId { get; set; }
        public string? FileStorageRootPath { get; set; }
        public int IndexBranchingFactor { get; set; } = 50;
    }
}
