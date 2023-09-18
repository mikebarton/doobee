using System;

namespace Doobee.Engine.Index
{
    internal class NodeItem<TKey> where TKey : IComparable
    {
        public TKey Key { get; set; }
        public long? DataAddress { get; set; }
        public long Value { get; set; }
        public bool IsLeaf { get; set; }
    }
}
