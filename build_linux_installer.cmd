@echo off
setlocal EnableExtensions

set "SCRIPT_DIR=%~dp0"
set "CONFIGURATION=Release"
set "RID=linux-x64"
set "DIST_DIR=%SCRIPT_DIR%artifacts\publish\%RID%"
set "OUTPUT_DIR=%SCRIPT_DIR%artifacts\installer\linux"
set "PACKAGE_DIR=%OUTPUT_DIR%\package_temp"
set "FULL_VERSION="
set "PACKAGE_VERSION="
set "PACKAGE_NAME=salaryforecaster"
set "PACKAGE_DISPLAY_NAME=SalaryForecaster"
set "OUTPUT_DEB=SalaryForecaster_%RID%_%FULL_VERSION%.deb"

pushd "%SCRIPT_DIR%" >nul

for /f "usebackq delims=" %%I in (`powershell -NoProfile -ExecutionPolicy Bypass -Command "$tag = git describe --tags --abbrev=0 2>$null; if ($LASTEXITCODE -eq 0 -and $tag) { $tag.Trim() }"`) do set "FULL_VERSION=%%I"

if not defined FULL_VERSION (
    echo Failed to determine the latest git tag.
    popd >nul
    exit /b 1
)

if /i "%FULL_VERSION:~0,1%"=="v" set "FULL_VERSION=%FULL_VERSION:~1%"
for /f "tokens=1 delims=-" %%I in ("%FULL_VERSION%") do set "PACKAGE_VERSION=%%I"

if not defined PACKAGE_VERSION (
    echo Failed to derive package version from git tag.
    popd >nul
    exit /b 1
)

set "OUTPUT_DEB=SalaryForecaster_%RID%_%FULL_VERSION%.deb"

where wsl >nul 2>&1
if errorlevel 1 (
    echo WSL is required to build the Linux installer.
    popd >nul
    exit /b 1
)

wsl bash -lc "command -v dpkg-deb >/dev/null"
if errorlevel 1 (
    echo dpkg-deb is not available in WSL.
    popd >nul
    exit /b 1
)

if exist "%DIST_DIR%" rmdir /s /q "%DIST_DIR%"
if exist "%PACKAGE_DIR%" rmdir /s /q "%PACKAGE_DIR%"
if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"

echo Publishing Linux build...
dotnet publish "SalaryForecast.Desktop\SalaryForecast.Desktop.csproj" -c %CONFIGURATION% -r %RID% -p:SelfContained=true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugSymbols=false -p:DebugType=None -p:Version=%PACKAGE_VERSION% -p:InformationalVersion=%FULL_VERSION% -o "%DIST_DIR%"
set "BUILD_EXIT_CODE=%ERRORLEVEL%"
if %BUILD_EXIT_CODE% neq 0 goto :fail

del /q "%DIST_DIR%\*.pdb" >nul 2>&1

powershell -NoProfile -ExecutionPolicy Bypass -Command "Add-Type -AssemblyName System.Drawing; $icon = New-Object System.Drawing.Icon('%SCRIPT_DIR%SalaryForecast.Desktop\Resources\receive_cash.ico'); $bitmap = $icon.ToBitmap(); $bitmap.Save('%DIST_DIR%\receive_cash.png', [System.Drawing.Imaging.ImageFormat]::Png); $bitmap.Dispose(); $icon.Dispose();"
set "BUILD_EXIT_CODE=%ERRORLEVEL%"
if %BUILD_EXIT_CODE% neq 0 goto :fail

mkdir "%PACKAGE_DIR%\DEBIAN"
mkdir "%PACKAGE_DIR%\opt\SalaryForecaster"
mkdir "%PACKAGE_DIR%\usr\bin"
mkdir "%PACKAGE_DIR%\usr\share\applications"

xcopy /E /H /I "%DIST_DIR%\*" "%PACKAGE_DIR%\opt\SalaryForecaster\" >nul
set "BUILD_EXIT_CODE=%ERRORLEVEL%"
if %BUILD_EXIT_CODE% geq 4 goto :fail

> "%PACKAGE_DIR%\DEBIAN\control" echo Package: %PACKAGE_NAME%
>>"%PACKAGE_DIR%\DEBIAN\control" echo Version: %PACKAGE_VERSION%
>>"%PACKAGE_DIR%\DEBIAN\control" echo Section: utils
>>"%PACKAGE_DIR%\DEBIAN\control" echo Priority: optional
>>"%PACKAGE_DIR%\DEBIAN\control" echo Architecture: amd64
>>"%PACKAGE_DIR%\DEBIAN\control" echo Maintainer: Yunker
>>"%PACKAGE_DIR%\DEBIAN\control" echo Depends: libx11-6, libice6, libsm6, libfontconfig1, ca-certificates, tzdata, libc6, libgcc1 ^| libgcc-s1, libgssapi-krb5-2, libstdc++6, zlib1g, libssl1.0.0 ^| libssl1.0.2 ^| libssl1.1 ^| libssl3, libicu ^| libicu74 ^| libicu72 ^| libicu71 ^| libicu70 ^| libicu69 ^| libicu68 ^| libicu67 ^| libicu66 ^| libicu65 ^| libicu63 ^| libicu60 ^| libicu57 ^| libicu55 ^| libicu52
>>"%PACKAGE_DIR%\DEBIAN\control" echo Description: SalaryForecaster desktop application

> "%PACKAGE_DIR%\usr\bin\salaryforecaster" echo #!/bin/sh
>>"%PACKAGE_DIR%\usr\bin\salaryforecaster" echo exec /opt/SalaryForecaster/SalaryForecast.Desktop "$@"

> "%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo [Desktop Entry]
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo Version=1.0
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo Type=Application
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo Name=%PACKAGE_DISPLAY_NAME%
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo Comment=Salary forecasting desktop application
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo Exec=salaryforecaster
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo Icon=/opt/SalaryForecaster/receive_cash.png
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo Terminal=false
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo Categories=Office^;Finance^;
>>"%PACKAGE_DIR%\usr\share\applications\salaryforecaster.desktop" echo StartupWMClass=SalaryForecast.Desktop

for /f "usebackq delims=" %%I in (`wsl wslpath -a "%PACKAGE_DIR%"`) do set "PACKAGE_DIR_UNIX=%%I"
for /f "usebackq delims=" %%I in (`wsl wslpath -a "%OUTPUT_DIR%"`) do set "OUTPUT_DIR_UNIX=%%I"

if not defined PACKAGE_DIR_UNIX (
    echo Failed to convert package directory path for WSL.
    set "BUILD_EXIT_CODE=1"
    goto :fail
)

if not defined OUTPUT_DIR_UNIX (
    echo Failed to convert output directory path for WSL.
    set "BUILD_EXIT_CODE=1"
    goto :fail
)

wsl rm -rf /tmp/salaryforecaster_pkg "/tmp/%OUTPUT_DEB%"
wsl mkdir -p /tmp/salaryforecaster_pkg
wsl cp -r "%PACKAGE_DIR_UNIX%/"* /tmp/salaryforecaster_pkg/
wsl bash -lc "set -e; sed -i 's/\r$//' /tmp/salaryforecaster_pkg/DEBIAN/control /tmp/salaryforecaster_pkg/usr/share/applications/salaryforecaster.desktop /tmp/salaryforecaster_pkg/usr/bin/salaryforecaster; find /tmp/salaryforecaster_pkg -type d -exec chmod 755 {} \; ; find /tmp/salaryforecaster_pkg -type f -exec chmod 644 {} \; ; chmod +x /tmp/salaryforecaster_pkg/opt/SalaryForecaster/SalaryForecast.Desktop /tmp/salaryforecaster_pkg/usr/bin/salaryforecaster; dpkg-deb --root-owner-group -Zgzip --build /tmp/salaryforecaster_pkg /tmp/%OUTPUT_DEB%"
set "BUILD_EXIT_CODE=%ERRORLEVEL%"
if %BUILD_EXIT_CODE% neq 0 goto :fail

wsl cp "/tmp/%OUTPUT_DEB%" "%OUTPUT_DIR_UNIX%/%OUTPUT_DEB%"
set "BUILD_EXIT_CODE=%ERRORLEVEL%"
if %BUILD_EXIT_CODE% neq 0 goto :fail

wsl rm -rf /tmp/salaryforecaster_pkg "/tmp/%OUTPUT_DEB%"

echo.
echo Build completed successfully.
echo DEB: "%OUTPUT_DIR%\%OUTPUT_DEB%"

popd >nul
exit /b 0

:fail
echo.
echo Build failed with exit code %BUILD_EXIT_CODE%.
popd >nul
exit /b %BUILD_EXIT_CODE%
