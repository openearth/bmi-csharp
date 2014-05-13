using NUnit.Framework;

namespace BasicModelInterface.Tests
{
    [TestFixture]
    public class BasicModelInterfaceLibraryTest
    {
        private const string library  = @"..\..\..\..\SampleNativeLibraries\SampleCppLibrary\bin\Debug\SampleCppLibrary.dll";

        [Test]
        public void InitializeAndFinish()
        {
            IBasicModelInterface library = new BasicModelInterfaceLibrary(BasicModelInterfaceLibraryTest.library);

            const string configFilePath = "empty";

            library.Initialize(configFilePath);
            library.Finish();
        }

        [Test]
        public void InitializeTwoTimes()
        {
            IBasicModelInterface library = new BasicModelInterfaceLibrary(BasicModelInterfaceLibraryTest.library);

            const string configFilePath = "empty";
            library.Initialize(configFilePath);
            library.Initialize(configFilePath);
        }

        [Test]
        public void GetVariableNames()
        {
            IBasicModelInterface library = new BasicModelInterfaceLibrary(BasicModelInterfaceLibraryTest.library);

            Assert.AreEqual(1, library.VariableNames.Length);
            Assert.AreEqual("test", library.VariableNames[0]);
        }

        [Test]
        [Ignore("incomplete")]
        public void SetLogger()
        {
            IBasicModelInterface library = new BasicModelInterfaceLibrary(BasicModelInterfaceLibraryTest.library);

            //library.SetLogger();
        }

        [Test]
        public void Run()
        {
            BasicModelInterfaceLibrary.Run(library, "test.config");
        }
    }
}