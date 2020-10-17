Param(
    $AppPoolName = 'EphItAutomationPool',
    $SiteName = 'EphItAutomation',
    $Port = 6600,
    [bool]$HttpS = $false,
    $PhysicalPath = "C:\inetpub\EphItAutomation"
)

$ErrorActionPreference = 'Stop'

Import-Module WebAdministration -Force

$SitePath = "IIS:\Sites\$SiteName"
$PoolPath = "IIS:\AppPools\$AppPoolName"

if(Test-Path $PoolPath){
    $null = Stop-WebAppPool -Name $AppPoolName
}

if(Test-Path $SitePath){
    $null = Stop-Website -Name $SiteName
}
$null = Remove-Item $PhysicalPath -Force -Recurse 
$null = New-Item $PhysicalPath -ItemType Directory -Force
$null = Copy-Item "$PSScriptRoot\*" "$PhysicalPath" -Recurse -Force

if( -Not (Test-Path $PoolPath))
{
    $null = New-WebAppPool -Name $AppPoolName -Force
}

if( -Not (Test-Path $SitePath))
{
    $null = New-Website -Name $SiteName -ApplicationPool $AppPoolName -Force -HostHeader "localhost" -PhysicalPath $PhysicalPath -Port $Port
}

$null = Set-WebConfigurationProperty -Filter "/system.webServer/security/authentication/windowsAuthentication" -PSPath $SitePath -Name Enabled -Value True
$null = Set-WebConfigurationProperty -Filter "/system.webServer/security/authentication/anonymousAuthentication" -PSPath $SitePath -Name Enabled -Value False

$null = Start-WebSite -Name $SiteName
$null = Start-WebAppPool -Name $AppPoolName