@echo off
rem Copies the external dependency .dlls into this directory.
rem Assumes that the .dlls are already built and are in the same root directory
rem as the Amaranth project.

copy "..\..\..\bramble\bin\Release\net8.0\Bramble.Core.dll"
copy "..\..\..\malison\bin\Release\net8.0-windows\Malison.Core.dll"
copy "..\..\..\malison\bin\Release\net8.0-windows\Malison.WinForms.dll"