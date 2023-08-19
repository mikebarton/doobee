﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Doobee.Engine.Index
{
    internal interface INodeDataContext : IDisposable
    {
        DataNode Read(long address);
        long Add(DataNode node);
        void Update(DataNode node);
        void Flush();
        DataNode ReadRootNode();
        void SetRootNode(DataNode node);
    }
}
