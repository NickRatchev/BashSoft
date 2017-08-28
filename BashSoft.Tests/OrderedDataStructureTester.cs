namespace BashSoft.Tests
{
    using System;
    using System.Collections.Generic;
    using BashSoft.Contracts;
    using BashSoft.DataStructures;
    using NUnit.Framework;

    [TestFixture]
    public class OrderedDataStructureTester
    {
        private ISimpleOrderedBag<string> names;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            this.names = new SimpleSortedList<string>();
        }

        [Test]
        public void TestEmptyCtor()
        {
            // Arrange
            this.names = new SimpleSortedList<string>();

            // Assert
            Assert.AreEqual(16, this.names.Capacity);
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestCtorWithInitialCapacity()
        {
            // Arrange
            this.names = new SimpleSortedList<string>(20);

            // Assert
            Assert.AreEqual(20, this.names.Capacity);
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestCtorWithAllParams()
        {
            // Arrange
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase, 30);

            // Assert
            Assert.AreEqual(30, this.names.Capacity);
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestCtorWithInitialComparer()
        {
            // Arrange
            this.names = new SimpleSortedList<string>(StringComparer.OrdinalIgnoreCase);

            // Assert
            Assert.AreEqual(16, this.names.Capacity);
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestAndIncreaseSize()
        {
            // Act
            this.names.Add("Nasko");

            // Assert
            Assert.AreEqual(1, this.names.Size);
        }

        [Test]
        public void TestAddNullThrowsException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.names.Add(null));
        }

        [Test]
        public void TestAddUnsortedDataIsHeldSorted()
        {
            // Act
            this.names.Add("Rosen");
            this.names.Add("Georgi");
            this.names.Add("Balkan");

            // Assert
            CollectionAssert.AreEqual(new string[] { "Balkan", "Georgi", "Rosen" }, this.names);
        }

        [Test]
        [TestCase(20)]
        [TestCase(35)]
        [TestCase(72)]
        public void TestAddingMoreThanInitialCapacity(int count)
        {
            // Act
            for (int i = 0; i < count; i++)
            {
                this.names.Add("KM Janko");
            }

            // Assert
            Assert.AreEqual(count, this.names.Size);
        }

        [Test]
        [TestCase(8)]
        public void TestAddingAllFromCollectionIncreasesSize(int count)
        {
            // Arrange
            List<string> myList = new List<string>();
            for (int i = 0; i < count; i++)
            {
                myList.Add("KM Janko");
            }

            // Act
            this.names.AddAll(myList);

            // Assert
            Assert.AreEqual(myList.Count, this.names.Size);
        }

        [Test]
        public void TestAddingAllFromNullThrowsException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.names.AddAll(null));
        }

        [Test]
        public void TestAddAllKeepsSorted()
        {
            // Arrange
            List<string> myList = new List<string>();
            myList.Add("Georgi");
            myList.Add("Balkan");
            myList.Add("Rosen");

            // Act
            this.names.AddAll(myList);
            myList.Sort();

            // Assert
            CollectionAssert.AreEqual(myList, this.names);
        }

        [Test]
        public void TestRemoveValidElementDecreasesSize()
        {
            // Act
            this.names.Add("KM Janko");
            this.names.Remove("KM Janko");

            // Assert
            Assert.AreEqual(0, this.names.Size);
        }

        [Test]
        public void TestRemoveValidElementRemovesSelectedOne()
        {
            // Act
            this.names.Add("Ivan");
            this.names.Add("Nasko");
            this.names.Remove("Ivan");

            // Assert
            CollectionAssert.AreEqual(new string[] { "Nasko" }, this.names);
        }

        [Test]
        public void TestRemovingNullThrowsException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.names.Remove(null));
        }

        [Test]
        public void TestJoinWithNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.names.JoinWith(null));
        }

        [Test]
        [TestCase(", ")]
        public void TestJoinWorksFine(string joiner)
        {
            // Arrange
            this.names.AddAll(new string[] { "Ivan", "Nasko" });

            // Act
            string result = this.names.JoinWith(joiner);

            // Assert
            Assert.AreEqual("Ivan" + joiner + "Nasko", result);
        }
    }
}