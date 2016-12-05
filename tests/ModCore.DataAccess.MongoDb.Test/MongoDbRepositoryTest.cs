using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ModCore.Specifications.Base;
using ModCore.Abstraction.DataAccess;
using ModCore.Specifications.BuiltIns;
using System.Linq.Expressions;
using ModCore.Core.DataAccess;

namespace ModCore.DataAccess.MongoDb.Test
{
    [TestFixture]
    public class MongoDbRepositoryTest
    {
        private MongoDbRepository<TestObject> _repos;

        public MongoDbRepositoryTest()
        {
            //var builder = new ConfigurationBuilder()
            //  .AddJsonFile("appsettings.json");
            //var configuration = builder.Build();

            //var connectionString = configuration.GetSection("MongoDbSettings").GetChildren().First(a=>a.Key == "ConnectionString").Value;


            _repos = new MongoDbRepository<TestObject>("mongodb://localhost:27017/ModCoreDBTest");
        }

        [Test]
        public void InsertTest()
        {
            var testObject = GenerateTestObject();

            _repos.Insert(testObject);

            var fromDb = _repos.FindById(testObject.Id);

            PerformBasicAssert(testObject, fromDb);
        }

        [Test]
        public void UpdateTest()
        {
            var testObject = GenerateTestObject();
            _repos.Insert(testObject);

            testObject.Name = "UpdateNamed";

            _repos.Update(testObject);

            var fromDb = _repos.FindById(testObject.Id);


            PerformBasicAssert(testObject, fromDb);
        }

        [Test]
        public void DeleteTest()
        {
            var testObject = GenerateTestObject();

            _repos.Insert(testObject);

            var fromDb = _repos.FindById(testObject.Id);

            PerformBasicAssert(testObject, fromDb);

            _repos.DeleteById(testObject.Id);

            fromDb = _repos.FindById(testObject.Id);

            Assert.IsNull(fromDb);

        }

        [Test]
        public void UpdateAllTest()
        {
            var testList = new List<TestObject>();

            foreach (var num in Enumerable.Range(0,9))
            {
                var testObject = GenerateTestObject();
                _repos.Insert(testObject);
                testList.Add(testObject);
            }

            foreach (var testItem in testList)
            {
                testItem.Name = "Orange";
            }


            _repos.Update(testList);


            foreach (var testItem in testList)
            {
                var fromDb = _repos.FindById(testItem.Id);
                PerformBasicAssert(testItem, fromDb);
            }
        }

        [Test]
        public void DeleteAllTest()
        {
            var testList = new List<TestObject>();

            foreach (var num in Enumerable.Range(0, 9))
            {
                var testObject = GenerateTestObject();
                testObject.Name = "Orange";
                _repos.Insert(testObject);
                testList.Add(testObject);
            }

            _repos.DeleteAll(new TestWithName("Orange"));


            foreach (var testItem in testList)
            {
                var fromDb = _repos.FindById(testItem.Id);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public void InsertManyTest()
        {
            var testList = new List<TestObject>();

            foreach (var num in Enumerable.Range(0, 9))
            {
                var testObject = GenerateTestObject();
                testList.Add(testObject);
            }

             _repos.Insert(testList);


            foreach (var testItem in testList)
            {
                testItem.Name = "Orange";
            }

             _repos.Update(testList);

            var fromDbList = _repos.FindAll(new TestWithName("Orange")).ToList();


            foreach (var testItem in testList)
            {
                var fromDb = fromDbList.Single(a => a.Id == testItem.Id);

                PerformBasicAssert(testItem, fromDb);
            }
        }

        [Test]
        public async Task InsertAsyncTest()
        {
            var testObject =  GenerateTestObject();

            await _repos.InsertAsync(testObject);

            var fromDb = await _repos.FindByIdAsync(testObject.Id);

            PerformBasicAssert(testObject, fromDb);
        }

        [Test]
        public async Task UpdateAsyncTest()
        {
            var testObject = GenerateTestObject();
           await  _repos.InsertAsync(testObject);

            testObject.Name = "UpdateNamed";

            await _repos.UpdateAsync(testObject);

            var fromDb =  await _repos.FindByIdAsync(testObject.Id);


            PerformBasicAssert(testObject, fromDb);
        }

        [Test]
        public async Task DeleteAsyncTest()
        {
            var testObject = GenerateTestObject();

            await _repos.InsertAsync(testObject);

            var fromDb = await _repos.FindByIdAsync(testObject.Id);

            PerformBasicAssert(testObject, fromDb);

            await _repos.DeleteByIdAsync(testObject.Id);

            fromDb = await _repos.FindByIdAsync(testObject.Id);

            Assert.IsNull(fromDb);

        }

        [Test]
        public async Task UpdateAllAsyncTest()
        {
            var testList = new List<TestObject>();

            foreach (var num in Enumerable.Range(0, 9))
            {
                var testObject = GenerateTestObject();
                await _repos.InsertAsync(testObject);
                testList.Add(testObject);
            }

            foreach (var testItem in testList)
            {
                testItem.Name = "Orange";
            }


           await _repos.UpdateAsync(testList);


            foreach (var testItem in testList)
            {
                var fromDb = await _repos.FindByIdAsync(testItem.Id);
                PerformBasicAssert(testItem, fromDb);
            }
        }

        [Test]
        public async Task InsertManyAsyncTest()
        {
            var testList = new List<TestObject>();

            foreach (var num in Enumerable.Range(0, 9))
            {
                var testObject = GenerateTestObject();
                testList.Add(testObject);
            }

            await _repos.InsertAsync(testList);


            foreach (var testItem in testList)
            {
                testItem.Name = "Orange";
            }

            await _repos.UpdateAsync(testList);

            var fromDbList = _repos.FindAllAsync(new TestWithName("Orange")).Result.ToList();


            foreach (var testItem in testList)
            {
                var fromDb = fromDbList.Single(a => a.Id == testItem.Id);

                PerformBasicAssert(testItem, fromDb);
            }
        }

        [Test]
        public async Task DeleteAllAsyncTest()
        {
            var testList = new List<TestObject>();

            foreach (var num in Enumerable.Range(0, 9))
            {
                var testObject = GenerateTestObject();
                testObject.Name = "Orange";
                await _repos.InsertAsync(testObject);
                testList.Add(testObject);
            }

            await _repos.DeleteAllAsync(new TestWithName("Orange"));


            foreach (var testItem in testList)
            {
                var fromDb = await _repos.FindByIdAsync(testItem.Id);
                Assert.IsNull(fromDb);
            }
        }

        [Test]
        public async Task FilterAsyncTest()
        {
            var testList = new List<TestObject>();

            foreach (var num in Enumerable.Range(0, 9))
            {
                var testObject = GenerateTestObject();
                _repos.Insert(testObject);
                testList.Add(testObject);
            }

            IPagedRequest filterRequest = new PagedRequest();
            filterRequest.PageSize = 5;
            filterRequest.CurrentPage = 1;

            var specification = new NotBlankName();
            var result = await _repos.FindAllByPageAsync(specification, filterRequest);

            Assert.IsTrue(result.PageSize == 5);
            Assert.IsTrue(result.CurrentPage == 1);
            Assert.IsTrue(result.TotalPages == 2);
            Assert.IsTrue(result.TotalResults == 10);
            Assert.IsTrue(result.CurrentPageResults.Count == 5);

        }

        private void PerformBasicAssert(TestObject testObject, TestObject fromDb)
        {
            Assert.IsTrue(testObject != fromDb);
            Assert.IsTrue(testObject.Name == fromDb.Name);
            Assert.IsTrue(testObject.Price == fromDb.Price);
        }

        private TestObject GenerateTestObject()
        {
            var random = new Random(9);


            return new TestObject
            {
                Name = "Test Object" + random.NextDouble().ToString(),
                Price = 4.5 + random.NextDouble()
            };
        }




      

}

    internal class TestWithName : Specification<TestObject>
    {
        string _name;

        public TestWithName(string name)
        {
            _name = name;
        }

        public override Expression<Func<TestObject, bool>> IsSatisifiedBy()
        {
            return x => x.Name == _name;
        }
    }

    internal class NotBlankName : Specification<TestObject>
    {

        public NotBlankName()
        {
        }

        public override Expression<Func<TestObject, bool>> IsSatisifiedBy()
        {
            return x => x.Name != null && x.Name != "";
        }
    }

}
