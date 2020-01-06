@ECHO OFF
call YBuild\vs2017var.cmd
YBuild\libgen.py SALARYFORECASTER_ANDROID --solution-name "SalaryForecaster.Android" --release -G "NMake Makefiles/VS15" && cd _build_SALARYFORECASTER_ANDROID_NMake_VS15 && nmake all
