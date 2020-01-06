# Файл сборки инсталляторов SalaryForecaster
set(Y_INSTALLER_SCRIPT "SalaryForecaster")

if ("${Y_SYSTEM}" STREQUAL "SALARYFORECASTER_ANDROID")

    add_dependencies(Installer SalaryForecaster)
else()
    YBUILD_ADD_INSTALLER_NSIS(
        Installer
        "${Y_INSTALLER_SCRIPT}"
        BOOTSTRAPPER_SCRIPT
            bootstrapper.nsi)
        
    add_dependencies(Installer SalaryForecast.Desktop)
endif()
