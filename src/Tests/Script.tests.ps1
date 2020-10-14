Param(
    $BaseURI
)

$ScriptBase = "$BaseURI/api/Script"

Describe "Create and retrieve script" {
    $Name = "MyScriptName"
    $Description = "MyDescription"
    It "Should create a script and return the id"{
        $Body = @{
            'Name' = $Name
            'Description' = $Description
        } | ConvertTo-Json
        $Script:Result = Invoke-RestMethod -Uri $ScriptBase -UseDefaultCredentials -Method Post -Body $Body -ContentType 'application/json'
        $Script:Result | Should -BeGreaterThan 0
    }
    It "Should get the script created"{
        $ScriptResult = Invoke-RestMethod -Uri "$ScriptBase/$($Script:Result)" -UseDefaultCredentials
        $ScriptResult.Name | Should -be $Name
        $ScriptResult.Description | Should -be $Description
    }
}