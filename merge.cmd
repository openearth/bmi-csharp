if not exist target md target

cd src\BasicModelInterfaceRunner\bin\Debug

..\..\..\..\tools\ILMerge.exe /targetplatform:"v4,c:\windows\Microsoft.NET\Framework\v4.0.30319" /target:exe /out:bmi-runner-merged.exe bmi-runner.exe BasicModelInterface.dll DocoptNet.dll 

move /Y bmi-runner-merged.exe ..\..\..\..\target\bmi-runner.exe
