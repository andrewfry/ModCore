@echo off
@echo PluginProjectName: %1 PluginName: %2

SET "binLoc=\bin\Debug\netstandard1.6"
SET "pluginLoc=ModCore.Www\Plugins\"
SET "viewLoc=\Views"
SET "adminLoc=\Admin"
SET "areaLoc=\Areas"
SET "contentLoc=\Content"
REM @echo "%binLoc%"
REM @echo "%pluginLoc%"
REM @echo "%pluginLoc%%2%"

cd ..
cd ..
@echo CURRENT DIR 
cd
@echo COMMAND TO RUN

REM Copy all of the assemblies to the plugin folder in the ModCore.www project
@echo COPY DLLs
@echo xcopy "plugin-src\%1%%binLoc%" "%pluginLoc%%2%" /c /i /s /e /y
xcopy  /c /i /s /e /y /v  "plugin-src\%1%binLoc%" "%pluginLoc%%2%"


IF EXIST "plugin-src\%1%viewLoc%" (
@echo COPY VIEWS Folder
@echo xcopy "plugin-src\%1%viewLoc%" "%pluginLoc%%2%viewLoc%" /c /i /s /e /y
xcopy  /c /i /s /e /y /v  "plugin-src\%1%viewLoc%" "%pluginLoc%%2%\Views"
)

IF EXIST "plugin-src\%1%adminLoc%" (
@echo COPY ADMIN Folder
@echo xcopy "plugin-src\%1%adminLoc%" "%pluginLoc%%2%adminLoc%" /c /i /s /e /y
xcopy  /c /i /s /e /y /v  "plugin-src\%1%adminLoc%" "%pluginLoc%%2%\Admin"
)

IF EXIST "plugin-src\%1%areaLoc%" (
@echo COPY AREAS Folder
@echo xcopy "plugin-src\%1%areaLoc%" "%pluginLoc%%2%areaLoc%" /c /i /s /e /y
xcopy  /c /i /s /e /y /v  "plugin-src\%1%areaLoc%\*.cshtml" "%pluginLoc%%2%\Areas"
)

IF EXIST "plugin-src\%1%contentLoc%" (
@echo COPY CONTENT Folder
@echo xcopy "plugin-src\%1%contentLoc%" "%pluginLoc%%2%contentLoc%" /c /i /s /e /y
xcopy  /c /i /s /e /y /v  "plugin-src\%1%contentLoc%" "%pluginLoc%%2%\Content"
)



REM Delete any ModCore specific assembilies as they will be rejected by the plugin manager and do not need to be copied.
del "%pluginLoc%%2%\ModCore*.*"