$BaseURI = 'https://localhost:44354'


$Tests = Get-ChildItem $PSScriptRoot -Filter '*.tests.ps1'

Foreach($test in $tests){
    Invoke-Pester -Script @{
        'Path' = $test.FullName
        'Parameters' = @{
            'BaseURI' = $BaseURI
        }
    }
}