# Файл сборки для проекта SalaryForecast.Desktop

YBUILD_ADD_STANDARD_PROJECT(SalaryForecast.Desktop CSEXE)

add_dependencies(SalaryForecast.Desktop SalaryForecast.Core YLocalization YMugenExtensions)