rmdir dist /S /Y
mkdir dist
mkdir dist\src
mkdir dist\src\doc
mkdir dist\src\lib
mkdir dist\src\MXP
mkdir dist\src\MXPTests
mkdir dist\bin
mkdir dist\bin\doc

copy * dist\src
Xcopy MXP dist\src\MXP /S /Y
Xcopy MXPTests dist\src\MXPTests /S /Y
copy lib\*.dll dist\src\lib
copy lib\*.config dist\src\lib
copy lib\*.mdb dist\src\lib

call runprebuild.bat
call runbuild.bat
call documentation.bat
xcopy doc dist\src\doc /S

xcopy doc dist\bin\doc /S
copy lib\*.dll dist\bin
copy lib\*.config dist\bin
copy lib\*.mdb dist\bin

move dist\src dist\mxp-csharp-0.5-0.1-src
move dist\bin dist\mxp-csharp-0.5-0.1-bin
