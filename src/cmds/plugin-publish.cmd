@echo off
@echo PluginProjectName: %1 PluginName: %2

SET "binLoc=\bin\Debug\netstandard1.6"
SET "pluginLoc=ModCore.Www\Plugins\"

REM @echo "%binLoc%"
REM @echo "%pluginLoc%"
REM @echo "%pluginLoc%%2%"

cd ..
cd ..
@echo CURRENT DIR 
cd
@echo COMMAND TO RUN
@echo xcopy "plugin-src\%1%binLoc%" "%pluginLoc%%2%" /c /i /s /e /y
xcopy  /c /i /s /e /y /v  "plugin-src\%1%binLoc%" "%pluginLoc%%2%"
del "%pluginLoc%%2%\ModCore*.*"