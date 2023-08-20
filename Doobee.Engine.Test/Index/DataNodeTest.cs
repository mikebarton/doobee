using Doobee.Engine.Index;
using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using NUnit.Framework;

namespace Doobee.Engine.Test.Index
{
    [TestFixture]
    internal class DataNodeTest
    {
        private INodeDataContext NodeContext { get; set; }
        private IDataStorage Storage { get; set; }


        [SetUp]
        public void Setup()
        {
            Storage = new MemoryStorage();            
        }

        private DataNode CreateTarget(int branchingFactor, IDataStorage? storage = null)
        {
            var item = new DataNode(new NodeDataContext(storage ?? Storage, branchingFactor), branchingFactor);
            return item;
        }

        [Test]
        public void Can_createtarget()
        {
            var target = CreateTarget(5);
            target.ShouldNotBeNull();
        }

        [Test]
        public void Can_query_empty_data_node()
        {
            var target = CreateTarget(5);
            var result = target.Query(1);
            result.ShouldBe(-1);
        }

        [Test]
        public void Can_insert_single_item()
        {
            var target = CreateTarget(5);
            Assert.IsNotNull(target);
            target.Insert(3, 8);
            target = CreateTarget(5, Storage);
            var result = target.Query(3);
            result.ShouldBe(8);
        }

        [Test]
        public void Can_insert_existing_key_with_different_address()
        {
            var target = CreateTarget(5);
            Assert.IsNotNull(target);
            target.Insert(3, 8);
            target = CreateTarget(5, Storage);
            var result = target.Query(3);
            result.ShouldBe(8);
            target.Insert(3, 12);
            target = CreateTarget(5, Storage);
            result = target.Query(3);
            result.ShouldBe(12);
        }

        [Test]
        public void Can_split_node()
        {
            var target = CreateTarget(3);
            Assert.IsNotNull(target);
            target.Insert(3, 8);
            target = CreateTarget(3, Storage);
            var three = target.Query(3);
            three.ShouldBe(8);
            target.Insert(6, 9);
            target = CreateTarget(3, Storage);
            var threeB = target.Query(3);
            threeB.ShouldBe(8);
            var six = target.Query(6);
            six.ShouldBe(9);
            target.Insert(99, 12);
            target = CreateTarget(3, Storage);
            var threeC = target.Query(3);
            threeC.ShouldBe(8);
            var sixB = target.Query(6);
            sixB.ShouldBe(9);
            var ninetyNine = target.Query(99);
            ninetyNine.ShouldBe(12);
            target.Insert(120, 22);
            target = CreateTarget(3, Storage);
            var threeD = target.Query(3);
            threeD.ShouldBe(8);
            var sixC = target.Query(6);
            sixC.ShouldBe(9);
            var ninetyNineB = target.Query(99);
            ninetyNineB.ShouldBe(12);
            var oneTwenty = target.Query(120);
            oneTwenty.ShouldBe(22);
        }

        [Test]
        public void Can_split_node_with_reverse_keys()
        {
            var target = CreateTarget(3);
            target.ShouldNotBeNull();

            target.Insert(120, 22);
            target = CreateTarget(3, Storage);
            var oneTwenty = target.Query(120);
            oneTwenty.ShouldBe(22);

            target.Insert(99, 12);
            target = CreateTarget(3, Storage);
            var ninetyNineB = target.Query(99);
            ninetyNineB.ShouldBe(12);
            var oneTwentyB = target.Query(120);
            oneTwentyB.ShouldBe(22);

            target.Insert(6, 9);
            target = CreateTarget(3, Storage);
            var six = target.Query(6);
            six.ShouldBe(9);
            var ninetyNine = target.Query(99);
            ninetyNine.ShouldBe(12);
            var oneTwentyC = target.Query(120);
            oneTwentyC.ShouldBe(22);

            target.Insert(3, 8);
            target = CreateTarget(3, Storage);
            var three = target.Query(3);
            three.ShouldBe(8);

            var threeB = target.Query(6);
            threeB.ShouldBe(9);
            var threeC = target.Query(99);
            threeC.ShouldBe(12);
            var sixB = target.Query(120);
            sixB.ShouldBe(22);

        }

        [Test]
        public void Can_split_root()
        {
            var target = CreateTarget(3);
            target.ShouldNotBeNull();
            target.Insert(3, 8);
            target.Insert(6, 9);
            target.Insert(99, 12);
            target = CreateTarget(3, Storage);
            var three = target.Query(3);
            three.ShouldBe(8);
            var six = target.Query(6);
            six.ShouldBe(9);
            var ninetyNine = target.Query(99);
            ninetyNine.ShouldBe(12);
        }

        [Test]
        public void Can_split_root_with_reverse_keys()
        {
            var target = CreateTarget(3);
            target.ShouldNotBeNull();
            target.Insert(99, 12);
            target.Insert(6, 9);
            target.Insert(3, 8);
            target.Insert(1, 4);
            target = CreateTarget(3, Storage);
            var one = target.Query(1);
            one.ShouldBe(4);
            var three = target.Query(3);
            three.ShouldBe(8);
            var six = target.Query(6);
            six.ShouldBe(9);
            var ninetyNine = target.Query(99);
            ninetyNine.ShouldBe(12);
        }

        [Test]
        public void Can_query_single_node_no_minitem_key_less_than_items()
        {
            var target = CreateTarget(10);
            target.Insert(100, 101);
            target.Insert(80, 81);
            target.Insert(60, 61);
            target.Insert(40, 41);
            target = CreateTarget(10, Storage);
            var result = target.Query(20);
            result.ShouldBe(-1);
        }

        [Test]
        public void Can_query_multi_level_node_key_less_than_items()
        {
            var target = CreateTarget(10);
            target.Insert(120, 121);
            target.Insert(110, 111);
            target.Insert(100, 101);
            target.Insert(90, 91);
            target.Insert(80, 81);
            target.Insert(70, 71);
            target.Insert(60, 61);
            target.Insert(50, 51);
            target.Insert(40, 41);
            target.Insert(30, 31);
            target.Insert(20, 21);
            target.Insert(10, 11);
            target = CreateTarget(10, Storage);
            var result = target.Query(5);
            result.ShouldBe(-1);
        }

        [Test]
        public void Can_query_multi_level_node_key_equal_to_inserted()
        {
            var target = CreateTarget(10);
            target.Insert(120, 121);
            target.Insert(110, 111);
            target.Insert(100, 101);
            target.Insert(90, 91);
            target.Insert(80, 81);
            target.Insert(70, 71);
            target.Insert(60, 61);
            target.Insert(50, 51);
            target.Insert(40, 41);
            target.Insert(30, 31);
            target.Insert(20, 21);
            target.Insert(10, 11);
            target = CreateTarget(10, Storage);
            var result = target.Query(10);
            result.ShouldBe(11);
        }


        [Test]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(15)]
        [TestCase(30)]
        [TestCase(60)]
        [TestCase(100)]
        [TestCase(200)]
        [TestCase(500)]
        public void Can_run_with_random_data(int branchingTestor)
        {
            var data = GetRandomLong(30, 100);
            //_nextAddress = data.Values.Max() + 1;
            var target = CreateTarget(branchingTestor);
            foreach (var item in data)
            {
                try
                {
                    target.Insert(item.Key, item.Value);
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            target = CreateTarget(branchingTestor, Storage);
            foreach (var item in data)
            {
                try
                {
                    var val = target.Query(item.Key);
                    val.ShouldBe(item.Value);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        [Test]
        [Explicit]
        [TestCase(500)]
        public void Can_run_with_random_data_growing(int branchingTestor)
        {
            var max = 1000;

            for (int i = 0; i < max; i++)
            {
                var data = GetRandomLong(i, 10000);
                var target = CreateTarget(branchingTestor);
                foreach (var item in data)
                {
                    try
                    {
                        target.Insert(item.Key, item.Value);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }

                target = CreateTarget(branchingTestor, Storage);
                foreach (var item in data)
                {
                    try
                    {
                        var val = target.Query(item.Key);
                        val.ShouldBe(item.Value);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
            }

        }

        private Random random = new Random(1);
        private static object _lock = new object();
        private Dictionary<long, long> GetRandomLong(int size, int keyRange)
        {
            Dictionary<long, long> data = new Dictionary<long, long>();

            while (data.Count < size)
            {
                long key, val = 0;

                lock (_lock)
                    key = random.Next(1, keyRange + 1);

                lock (_lock)
                    val = random.Next(1, int.MaxValue);

                if (!data.ContainsKey(key))
                    data.Add(key, key);
            }

            return data;
        }
    }
}
