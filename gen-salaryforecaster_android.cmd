@ECHO OFF
call YBuild\vs2017var.cmd
YBuild\libgen.py SALARYFORECASTER_ANDROID --solution-name "SalaryForecaster.Android" -G "Visual Studio 15"
