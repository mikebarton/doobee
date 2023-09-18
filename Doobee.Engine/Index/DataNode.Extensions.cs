using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Doobee.Engine.Index
{
    internal partial class DataNode<TKey> where TKey : IComparable
    {
        public void Load(byte[] data)
        {
            var offset = 0;            

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

            data.AddRange(BitConverter.GetBytes(_items.Count));
            for (int i = 0; i < _branchingFactor; i++)
            {
                NodeItem<TKey> item = null;
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

        private NodeItem<TKey> ToNodeItem(byte[] data)
        {
            var offset = 0;
            NodeItem<TKey> item = null;
            var hasNodeItem = BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            if (hasNodeItem)
            {
                item = new NodeItem<TKey>();
                item.Key = GetKey(data, ref offset);
                

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

        private TKey GetKey(byte[] data, ref int offset)
        {
            object key = null;
            if(typeof(TKey) == typeof(long))
            {
                key = BitConverter.ToInt64(data, offset);
                offset += sizeof(long);
            }
            else if(typeof(TKey) == typeof(int))
            {
                key = BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
            }
            else if (typeof(TKey) == typeof(short))
            {
                key = BitConverter.ToInt16(data, offset);
                offset += sizeof(short);
            }
            else if (typeof(TKey) == typeof(Guid))
            {
                key = new Guid(data.Skip(offset).Take(16).ToArray());
                offset += 16;
            }
            else if(typeof(TKey) == typeof(float))
            {
                key = BitConverter.ToSingle(data, offset);
                offset += sizeof(float);
            }
            else if(typeof(TKey) == typeof(double))
            {
                key = BitConverter.ToDouble(data, offset);
                offset += sizeof(double);
            }
            else if(typeof(TKey) == typeof(DateTime))
            {
                var ticks = BitConverter.ToInt64(data, offset);
                key = new DateTime(ticks);
                offset += sizeof(long);
            }
            else if (typeof(TKey) == typeof(DateOnly))
            {
                var ticks = BitConverter.ToInt64(data, offset);
                key = DateOnly.FromDateTime(new DateTime(ticks));
                offset += sizeof(long);
            }
            else
            {
                throw new NotSupportedException("Indices must int16, int32, int64, Guid, float, double, DateTime or DateOnly");
            }

            return (TKey)Convert.ChangeType(key, typeof(TKey));
        }

        private byte[] GetBytes(TKey key)
        {
            byte[] data;
            if (typeof(TKey) == typeof(long))
            {
                data = BitConverter.GetBytes((long)Convert.ChangeType(key, typeof(long)));
            }
            else if (typeof(TKey) == typeof(int))
            {
                data = BitConverter.GetBytes((int)Convert.ChangeType(key, typeof(int)));
            }
            else if (typeof(TKey) == typeof(short))
            {
                data = BitConverter.GetBytes((short)Convert.ChangeType(key, typeof(short)));
            }
            else if (typeof(TKey) == typeof(Guid))
            {
                data = ((Guid)Convert.ChangeType(key, typeof(Guid))).ToByteArray();
            }
            else if (typeof(TKey) == typeof(float))
            {
                data = BitConverter.GetBytes((float)Convert.ChangeType(key, typeof(float)));
            }
            else if (typeof(TKey) == typeof(double))
            {
                data = BitConverter.GetBytes((double)Convert.ChangeType(key, typeof(double)));
            }
            else if (typeof(TKey) == typeof(DateTime))
            {
                var ticks = ((DateTime)Convert.ChangeType(key, typeof(DateTime))).Ticks;
                data = BitConverter.GetBytes(ticks);
            }
            else if (typeof(TKey) == typeof(DateOnly))
            {
                var date = (DateOnly)Convert.ChangeType(key, typeof(DateOnly));
                var ticks = date.ToDateTime(TimeOnly.MinValue).Ticks;
                data = BitConverter.GetBytes(ticks);
            }
            else
            {
                throw new NotSupportedException("Indices must int16, int32, int64, Guid, float, double, DateTime or DateOnly");
            }

            return data;
        }

        private byte[] GetNodeItemBytes(NodeItem<TKey> item)
        {
            List<byte> data = new List<byte>();
            if (item != null)
            {
                data.AddRange(BitConverter.GetBytes(true));
                data.AddRange(GetBytes(item.Key));
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
            size += GetKeySize(); // key
            size += sizeof(bool); // has dataaddress
            size += sizeof(long); // dataaddress
            size += sizeof(long); // value
            size += sizeof(bool); // isleaf?
            return size;
        }

        private static int GetKeySize()
        {
            if (typeof(TKey) == typeof(long))
            {
                return sizeof(long);
            }
            else if (typeof(TKey) == typeof(int))
            {
                return sizeof(int);
            }
            else if (typeof(TKey) == typeof(short))
            {
                return sizeof(short);
            }
            else if (typeof(TKey) == typeof(Guid))
            {
                return 16;
            }
            else if (typeof(TKey) == typeof(float))
            {
                return sizeof(float);
            }
            else if (typeof(TKey) == typeof(double))
            {
                return sizeof(double);
            }
            else if (typeof(TKey) == typeof(DateTime))
            {
                return sizeof(long);
            }
            else if (typeof(TKey) == typeof(DateOnly))
            {
                return sizeof(long);
            }
            else
            {
                throw new NotSupportedException("Indices must int16, int32, int64, Guid, float, double, DateTime or DateOnly");
            }
        }
    }
}
