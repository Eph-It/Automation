$mod = Get-Module -Name Automation
if($null -ne $mod) {
    $mod
}
else {
    Write-Output -InputObject "No Module Found" -Verbose
}