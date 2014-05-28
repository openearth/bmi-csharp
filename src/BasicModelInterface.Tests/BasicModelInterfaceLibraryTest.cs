using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace BasicModelInterface.Tests
{
    [TestFixture]
    public class BasicModelInterfaceLibraryTest
    {
        private const string LibraryC = @"..\..\..\..\bin\Debug\model-c.dll";

        private IBasicModelInterface library;

        [SetUp]
        public void SetUp()
        {
            library = new BasicModelInterfaceLibrary(LibraryC);
        }

        [Test]
        public void InitializeAndFinish()
        {
            const string configFilePath = "empty";

            library.Initialize(configFilePath);
            library.Finish();
        }

        [Test]
        public void InitializeTwoTimes()
        {
            const string configFilePath = "empty";
            library.Initialize(configFilePath);
            library.Initialize(configFilePath);
        }

        [Test]
        public void GetVariableNames()
        {
            Assert.AreEqual(3, library.VariableNames.Length);
            Assert.AreEqual("arr1", library.VariableNames[0]);
            Assert.AreEqual("arr2", library.VariableNames[1]);
            Assert.AreEqual("arr3", library.VariableNames[2]);
        }

        [Test]
        public void Run()
        {
            BasicModelInterfaceLibrary.Run(LibraryC, "test.config");
        }

        [Test]
        public void GetValues_1D_Double()
        {
            var values = library.GetValues(library.VariableNames[0]);

            var valuesExpected = new[] { 3.0, 2.0, 1.0 };

            Assert.AreEqual(valuesExpected, values);
        }

        [Test]
        public void GetValues_2D_Int()
        {
            var values = library.GetValues(library.VariableNames[1]);

            var valuesExpected = new[,] { { 3, 2, 1 }, { 6, 4, 2 } };

            Assert.AreEqual(valuesExpected, values);
        }

        [Test]
        public void SetLogger()
        {
            library.Logger = (level, message) => Console.WriteLine("{0}: {1}", level, message);
            library.Initialize(string.Empty);
            library.Finish();
        }
    }
}