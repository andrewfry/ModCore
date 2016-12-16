@echo off
@echo PluginProjectName: %1 PluginName: %2

SET "binLoc=\bin\Debug\netstandard1.6"
SET "pluginLoc=ModCore.Www\Plugins\"
SET "viewLoc=\Views"
SET "adminLoc=\Admin"
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
REM Delete any ModCore specific assembilies as they will be rejected by the plugin manager and do not need to be copied.
del "%pluginLoc%%2%\ModCore*.*"