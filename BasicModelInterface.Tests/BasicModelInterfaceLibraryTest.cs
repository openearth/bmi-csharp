using NUnit.Framework;

namespace BasicModelInterface.Tests
{
    [TestFixture]
    public class BasicModelInterfaceLibraryTest
    {
        private const string libraryPath  = @"..\..\..\SampleNativeLibraries\SampleCppLibrary\bin\Debug\SampleCppLibrary.dll";

        [Test]
        public void InitializeAndFinish()
        {
            IBasicModelInterface library = new BasicModelInterfaceLibrary(libraryPath);

            const string configFilePath = "empty";

            library.Initialize(configFilePath);
            library.Finish();
        }

        [Test]
        public void InitializeTwoTimes()
        {
            IBasicModelInterface library = new BasicModelInterfaceLibrary(libraryPath);

            const string configFilePath = "empty";
            library.Initialize(configFilePath);
            library.Initialize(configFilePath);
        }
    }
}