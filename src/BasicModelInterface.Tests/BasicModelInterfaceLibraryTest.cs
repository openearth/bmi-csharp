using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace BasicModelInterface.Tests
{
    [TestFixture]
    public class BasicModelInterfaceLibraryTest
    {
        private const string library  = @"..\..\..\SampleNativeLibraries\SampleCppLibrary\bin\Debug\SampleCppLibrary.dll";

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

        [Test]
        public void GetValues()
        {
            IBasicModelInterface library = new BasicModelInterfaceLibrary(BasicModelInterfaceLibraryTest.library);

            var ptr = IntPtr.Zero;
            get_var_values2(ref ptr);

            var v0 = GetArrayValue(ptr, 0);
            var v1 = GetArrayValue(ptr, 1);

            var values = library.GetValues<double>("v1");

            Console.WriteLine(values[0]);
            Console.WriteLine(values[1]);
        }

        private unsafe double GetArrayValue(IntPtr ptr, int index)
        {
            return *((double*) ptr + index);
        }

        [DllImport(library, CallingConvention = CallingConvention.Cdecl)]
        static extern void get_var_values2(ref IntPtr values);
    }
}