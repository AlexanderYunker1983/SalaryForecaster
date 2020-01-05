@ECHO OFF
call YBuild\vs2017var.cmd
YBuild\libgen.py SALARYFORECASTER --solution-name "SalaryForecaster" --release -G "NMake Makefiles/VS15" && cd _build_SALARYFORECASTER_NMake_VS15 && nmake all
