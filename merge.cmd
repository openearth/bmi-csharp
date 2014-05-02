if not exist target md target

cd BasicModelInterfaceRunner\bin\Debug

..\..\..\tools\ILMerge.exe /targetplatform:"v4,c:\windows\Microsoft.NET\Framework\v4.0.30319" /target:exe /out:bmi_runner_merged.exe bmi_runner.exe BasicModelInterface.dll NDesk.Options.dll 

move /Y bmi_runner_merged.exe ..\..\..\target\bmi_runner.exe
