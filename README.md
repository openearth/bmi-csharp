bmi-csharp
==========

Basic Model Interface for C#. 

A simple API that can be used to run native simulation components which implement BMI interface: [bmi.h](https://github.com/openearth/bmi/blob/master/models/include/bmi.h).

Provides

* C# version of the API: [IBasicModelInterface.cs](https://github.com/openearth/bmi-csharp/blob/master/src/BasicModelInterface/IBasicModelInterface.cs).
* Universal wrapper that can be used to load any native (C/C++/FORTRAN) BMI library: [BasicModelInterfaceLibrary.cs](https://github.com/openearth/bmi-csharp/blob/master/src/BasicModelInterface/BasicModelInterfaceLibrary.cs).

See [BasicModelInterfaceLibraryTest.cs](https://github.com/openearth/bmi-csharp/blob/master/src/BasicModelInterface.Tests/BasicModelInterfaceLibraryTest.cs) for examples on how to use it.

In general:

> var lib = new BasicModelInterfaceLibraryTest("native_library");
> 
> lib.Initialize("config_file");
> 
> lib.Update(-1);
> 
> var a = lib.GetValues("variable_name");
> 
> lib.SetValues("variable_name", a);
>
> lib.Finish();

