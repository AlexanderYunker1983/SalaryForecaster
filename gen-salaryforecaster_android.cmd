@ECHO OFF
call YBuild\vs2017var.cmd
YBuild\libgen.py SALARYFORECASTER_ANDROID --solution-name "SalaryForecaster.Android" --custom-build-dir-suffix "_ANDROID" -G "Visual Studio 15"
