Set-Location $PSScriptRoot

Set-Location ..\EphIt\PowerShell\EnterpriseAutomation

dotnet publish --configuration Release --self-contained true

Import-Module .\bin\Release\netcoreapp3.1\publish\EnterpriseAutomation.dll
$ScriptIdName = "Test$(Get-Random -Minimum 1 -Maximum 293847239847298374)"

try{
    New-EAScript -Name $ScriptIdName -Description "MyDes" -Body "Test" -Server localhost -Port 55
}
catch{
    Write-Output $_.Exception.Message
}

Pop-Location

