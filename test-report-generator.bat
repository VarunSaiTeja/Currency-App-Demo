@echo off
REM Delete all old test results
rmdir /s /q test-results
rmdir /s /q coverage-report

REM Run tests and collect coverage, outputting to TestResults
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings --results-directory test-results

REM Generate HTML report from the latest coverage file in TestResults
reportgenerator -reports:"test-results/**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

REM Open the HTML report in the default browser
start coverage-report\index.html

echo Coverage report generated in coverage-report\
pause