@ECHO OFF
call YBuild\vs2017var.cmd
YBuild\libgen.py SALARYFORECASTER --solution-name "SalaryForecaster" -G "Visual Studio 15"
