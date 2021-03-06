# instructions from: https://docs.sonarqube.org/latest/analysis/scan/sonarscanner-for-msbuild/
# as of writing, install the dotnet-sonarscanner tool first:
# dotnet tool install --global dotnet-sonarscanner --version 4.8.0
# coverlet: dotnet tool install --global coverlet.console

dotnet sonarscanner begin /key:"WyldeMountain" /d:sonar.cs.opencover.reportsPaths=WyldeMountain.Web.Tests/coverage.opencover.xml /d:sonar.javascript.lcov.reportPaths=ClientApp/coverage/lcov.info /d:sonar.coverage.exclusions="**Test*.cs","**/*.test*.tsx","**/node_modules/**"
.\run-tests.ps1
dotnet sonarscanner end