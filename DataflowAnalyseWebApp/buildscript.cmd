@echo off

setlocal
set msbuild="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"

%msbuild% _ci.msbuild /nologo /m /v:m /t:Compile /p:Configuration=Debug

pause
endlocal