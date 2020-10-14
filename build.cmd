cd %~dp0src\EphIt
dotnet restore

dotnet build --configuration Release --no-restore

