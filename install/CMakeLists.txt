# Файл сборки инсталляторов SalaryForecaster
set(Y_INSTALLER_SCRIPT "SalaryForecaster")

if("${YBUILD_GIT_BRANCH_NAME}" STREQUAL "light_version")
    set(Y_INSTALLER_SCRIPT "SalaryForecaster_Light")
endif()

YBUILD_ADD_INSTALLER_NSIS(
        Installer
        "${Y_INSTALLER_SCRIPT}"
        BOOTSTRAPPER_SCRIPT
            bootstrapper.nsi)

add_dependencies(Installer SalaryForecast.Desktop)
