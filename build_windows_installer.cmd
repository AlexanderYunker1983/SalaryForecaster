@echo off
setlocal EnableExtensions

set "SCRIPT_DIR=%~dp0"
set "CONFIGURATION=Release"
set "OUTPUT_DIR=%SCRIPT_DIR%artifacts\installer\%CONFIGURATION%"
set "TAG_VERSION="
set "TAG_FILE=%TEMP%\salaryforecaster_latest_tag.txt"

pushd "%SCRIPT_DIR%" >nul

git describe --tags --abbrev=0 > "%TAG_FILE%" 2>nul
if %ERRORLEVEL% equ 0 set /p TAG_VERSION=<"%TAG_FILE%"
if exist "%TAG_FILE%" del "%TAG_FILE%" >nul 2>&1

if not defined TAG_VERSION (
    echo Failed to determine the latest git tag.
    popd >nul
    exit /b 1
)

if /i "%TAG_VERSION:~0,1%"=="v" set "TAG_VERSION=%TAG_VERSION:~1%"

set "OUTPUT_NAME=SalartForecaster_%TAG_VERSION%"

if exist "%OUTPUT_DIR%" rmdir /s /q "%OUTPUT_DIR%"

echo Building %OUTPUT_NAME%.msi...
dotnet build "SalaryForecast.Installer\SalaryForecast.Installer.wixproj" -c %CONFIGURATION% -p:OutputName=%OUTPUT_NAME%
set "BUILD_EXIT_CODE=%ERRORLEVEL%"

if %BUILD_EXIT_CODE% neq 0 (
    echo.
    echo Build failed with exit code %BUILD_EXIT_CODE%.
) else (
    echo.
    echo Build completed successfully.
    echo MSI: "%OUTPUT_DIR%\%OUTPUT_NAME%.msi"
)

popd >nul
exit /b %BUILD_EXIT_CODE%
