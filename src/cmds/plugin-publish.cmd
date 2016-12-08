@echo off
@echo PluginProjectName: %1 PluginName: %2

SET "binLoc=\bin\Debug\netstandard1.6"
SET "pluginLoc=ModCore.Www\Plugins\"
SET "viewLoc=\Views"
REM @echo "%binLoc%"
REM @echo "%pluginLoc%"
REM @echo "%pluginLoc%%2%"

cd ..
cd ..
@echo CURRENT DIR 
cd
@echo COMMAND TO RUN

REM Copy all of the assemblies to the plugin folder in the ModCore.www project
@echo xcopy "plugin-src\%1%binLoc%" "%pluginLoc%%2%" /c /i /s /e /y
xcopy  /c /i /s /e /y /v  "plugin-src\%1%binLoc%" "%pluginLoc%%2%"

REM If there is a Views folder in the plugin then copy it as well
IF EXIST "plugin-src\%1%ViewLoc%" (
@echo xcopy "plugin-src\%1%ViewLoc%" "%pluginLoc%%2%ViewLoc%" /c /i /s /e /y
xcopy  /c /i /s /e /y /v  "plugin-src\%1%ViewLoc%" "%pluginLoc%%2%\Views"
)

REM Delete any ModCore specific assembilies as they will be rejected by the plugin manager and do not need to be copied.
del "%pluginLoc%%2%\ModCore*.*"