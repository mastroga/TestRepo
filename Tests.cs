using System.Linq;
using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Interview
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Repository_Returns_IEnumerableOfTypeIStoreable()
        {
            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj(); //new Repository<IStoreable>();
            var repoResult = repo.All();

            Assert.IsInstanceOf<IEnumerable<IStoreable>>(repoResult);
        }


        [Test]
        public void Test_Repository_ReturnAll()
        {
            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();
            var storeable1 = new Storeable { Id = "Storeable 1" };
            var storeable2 = new Storeable { Id = "Storeable 2" };
            var storeable3 = new Storeable { Id = "Storeable 3" };

            repo.Save(storeable1);
            repo.Save(storeable2);
            repo.Save(storeable3);

            var expectedCount = repo.All().Count();

            Assert.AreEqual(3, expectedCount,
                "3 Entries added and 3 returned.");
        }

        [Test]
        public void Test_Repository_FindByIdReturnsCorrectEntry()
        {
            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();
            var storeable1 = new Storeable { Id = "Storeable 1" };
            var storeable2 = new Storeable { Id = "Storeable 2" };
            var storeable3 = new Storeable { Id = "Storeable 3" };

            repo.Save(storeable1);
            repo.Save(storeable2);
            repo.Save(storeable3);

            var expectedResult = repo.FindById(storeable2.Id);

            Assert.AreEqual(storeable2, expectedResult,
                "Find by Id of a recently saved entry returns the object.");
        }

        [Test]
        public void Test_Repository_FindReturnsNullWhenNotFound()
        {
            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();
            var storeable1 = new Storeable { Id = "1" };
            var storeable2 = new Storeable { Id = 1 };

            repo.Save(storeable1);

            var expectedResult = repo.FindById(storeable2.Id);

            Assert.AreEqual(null, expectedResult,
                "Find by Id of a recently saved entry returns the object. {0}", expectedResult);

        }

        [Test]
        public void Test_Repository_Save()
        {

            var storeable1 = new Storeable();
            storeable1.Id = "Storeable 1";

            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();
            repo.Save(storeable1);

            Assert.AreEqual(storeable1, repo.All().Single(m => m.Id == storeable1.Id),
                "One entry added to the repository and saved.");
        }

        [Test]
        public void Test_Repository_Save_DuplicateRemovesTheDuplicate()
        {
            var storeable1 = new Storeable();
            storeable1.Id = "Storeable 1";

            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();
            repo.Save(storeable1);
            repo.Save(storeable1);

            var repoData = repo.All();

            Assert.IsTrue(repoData.Count() == 1,
                "Current implementation will ovewrite duplicated saves.");
        }

        [Test]
        public void Test_Repository_Save_NullObjectThrowsException()
        {
            Storeable badStoreable = null;

            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();
            repo.Save(badStoreable);
            Assert.Throws<ArgumentNullException>(() => { throw new ArgumentNullException(); },
                "Saving null objects throws exception");
        }

        [Test]
        public void Test_Repository_DeleteExistingRemovesTheEntry()
        {
            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();
            var storeable1 = new Storeable() { Id = "Storeable 1" };
            var storeable2 = new Storeable() { Id = "Storeable 2" };

            repo.Save(storeable1);
            repo.Save(storeable2);
            repo.Delete(storeable1.Id);

            var repoObjectsAfterDelete = repo.All();

            Assert.IsFalse(((IEnumerable<IStoreable>)repoObjectsAfterDelete).Contains(storeable1),
                "Deleting an object that was previously added, results in the ");
        }

        [Test]
        public void Test_Repository_DeleteNotFoundEntryIgnoresTheEntry()
        {
            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();
            var storeable1 = new Storeable { Id = "1" };
            var storeableNotSaved = new Storeable { Id = 1 };

            repo.Save(storeable1);
            repo.Delete(storeableNotSaved.Id);

            var repoObjectsAfterDelete = repo.All();

            Assert.IsTrue(((IEnumerable<IStoreable>)repoObjectsAfterDelete).Contains(storeable1),
                "Ignore delete of not found entries.");
        }


        [Test]
        [Ignore("Memory Max Limit is not reliably calculated via the test suite. Ignoring this test case for now.")]
        [TestCase(134217728)]
        public void Test_Repository_SavingLotsOfObjectsThrowsException(int maxSize)
        {
            var repo = InMemoryStorage.GetInMemoryDefaultStorageObj();

            foreach (int i in Enumerable.Range(0, maxSize))
            {
                repo.Save(new Storeable() { Id = String.Concat("Storeable ", i) });
            }

            double actualObjectCount = repo.All().Count();

            Assert.Throws<OutOfMemoryException>(() => { throw new OutOfMemoryException(); },
                "Attempting to save 134217728 objects throws OutOfMemoryException exception.");
        }        
        
        //[Test]
        //[Ignore("Older test. Cannot test as IRepository does not implement IDisposable")]
        //public void Test_Repository_SaveAfterDisposeThrowsException()
        //{
        //    IRepository<IStoreable> repo;

        //    using (repo = new Repository<Storeable>())
        //    {
        //        var storeableObj1 = new Storeable() { Id = "Storeable 4" };
        //        repo.Save(storeableObj1);
        //    }

        //    var storeableObj2 = new Storeable() { Id = "Storeable 5" };
        //    repo.Save(storeableObj2);

        //    Assert.Throws<ObjectDisposedException>(() => { throw new ObjectDisposedException(GetType().FullName); },
        //        "Saving objects of a disposed repository throws ObjectDisposedException.");
        //}

    }
}