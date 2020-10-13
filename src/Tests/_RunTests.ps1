$BaseURI = 'https://localhost:44354/'

$CurrentSid = ([System.Security.Principal.WindowsIdentity]::GetCurrent()).User.Value



$Tests = Get-ChildItem $PSScriptRoot -Filter '*.tests.ps1'

Foreach($test in $tests){
    Invoke-Pester -Script @{
        'Path' = $test.FullName
        'Parameters' = @{
            'BaseURI' = $BaseURI
        }
    }
}