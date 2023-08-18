namespace Doobee.Engine.Index
{
    internal class NodeItem
    {
        public long Key { get; set; }
        public long? DataAddress { get; set; }
        public bool IsLeaf { get; set; }
    }
}
