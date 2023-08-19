using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Index
{
    internal partial class DataNode
    {
        public void Load(byte[] data)
        {
            var offset = 0;

            var hasMinItem = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);

            var minItemsBytes = data.Skip(offset).Take(GetNodeItemSize());
            offset += GetNodeItemSize();

            if (hasMinItem)
                _minItem = ToNodeItem(minItemsBytes.ToArray());

            var count = BitConverter.ToInt32(data, offset);
            offset += sizeof(int);

            for (int i = 0; i < _branchingFactor; i++)
            {
                var itemBytes = data.Skip(offset).Take(GetNodeItemSize());
                var item = ToNodeItem(itemBytes.ToArray());
                offset += GetNodeItemSize();
                if (item != null)
                    _items.Add(item.Key, item);
            }

            if (count != _items.Count())
                throw new Exception("when loading the node, the number of nodeItems has not equal to the expected amount");

            var hasParent = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);

            var parentData = BitConverter.ToInt64(data, offset);
            offset += sizeof(long);
            if (hasParent)
            {
                _parentAddress = parentData;
            }

            //var hasAddress = BitConverter.ToBoolean(dataStorage.Read(offset, sizeof(bool)), 0);
            //offset += sizeof(bool);

            //if (hasAddress)
            //{
            //    _nodeAddress = BitConverter.ToInt64(dataStorage.Read(offset, sizeof(long)), 0);
            //    offset += sizeof(long);
            //}
        }



        public byte[] ToByteArray()
        {
            var data = new List<byte>();

            var hasMinItem = _minItem != null;
            data.AddRange(BitConverter.GetBytes(hasMinItem));
            data.AddRange(GetNodeItemBytes(_minItem));

            data.AddRange(BitConverter.GetBytes(_items.Count));
            for (int i = 0; i < _branchingFactor; i++)
            {
                NodeItem item = null;
                if (i < _items.Count)
                    item = _items.Values[i];

                data.AddRange(GetNodeItemBytes(item));
            }

            data.AddRange(BitConverter.GetBytes(_parentAddress.HasValue));
            data.AddRange(BitConverter.GetBytes(_parentAddress.HasValue ? _parentAddress.Value : default(long)));

            //if (_nodeAddress.HasValue)
            //    data.AddRange(BitConverter.GetBytes(_nodeAddress.Value));

            return data.ToArray();
        }

        private NodeItem ToNodeItem(byte[] data)
        {
            var offset = 0;
            NodeItem item = null;
            var hasNodeItem = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasNodeItem)
            {
                item = new NodeItem();
                item.Key = BitConverter.ToInt64(data, offset);
                offset += sizeof(long);

                var hasAddress = BitConverter.ToBoolean(data, offset);
                offset += sizeof(bool);
                var addressData = BitConverter.ToInt64(data, offset);
                offset += sizeof(long);
                if (hasAddress)
                {
                    item.DataAddress = addressData;
                }
                item.Value = BitConverter.ToInt64(data, offset); 
                offset += sizeof(long);
                item.IsLeaf = BitConverter.ToBoolean(data, offset);
                offset += sizeof(bool);
            }

            return item;
        }

        private byte[] GetNodeItemBytes(NodeItem item)
        {
            List<byte> data = new List<byte>();
            if (item != null)
            {
                data.AddRange(BitConverter.GetBytes(true));
                data.AddRange(BitConverter.GetBytes(item.Key));
                data.AddRange(BitConverter.GetBytes(item.DataAddress.HasValue));
                data.AddRange(BitConverter.GetBytes(item.DataAddress.HasValue ? item.DataAddress.Value : default(long)));
                data.AddRange(BitConverter.GetBytes(item.Value));
                data.AddRange(BitConverter.GetBytes(item.IsLeaf));
            }
            else
            {
                data.AddRange(Enumerable.Repeat(default(byte), GetNodeItemSize()));
            }
            return data.ToArray();
        }

        public static int GetNodeSize(int branchingFactor)
        {
            var size = 0;
            size += sizeof(bool); // hasMinItem
            size += GetNodeItemSize(); // minItem 
            size += sizeof(int); // number of items
            size += GetNodeItemSize() * branchingFactor; // all the nodeitems
            size += sizeof(bool); // has parent address?
            size += sizeof(long); // parent address
            return size;
        }



        private static int GetNodeItemSize()
        {
            var size = 0;

            size += sizeof(bool); //has node?
            size += sizeof(long); // key
            size += sizeof(bool); // has dataaddress
            size += sizeof(long); // dataaddress
            size += sizeof(long); // value
            size += sizeof(bool); // isleaf?
            return size;
        }
    }
}
