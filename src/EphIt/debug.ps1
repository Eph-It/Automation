$Uri = 'https://localhost:44354/'


Invoke-RestMethod -Uri 'https://localhost:44354/api/Script?Name=ScriptName3' -UseDefaultCredentials

$Body = @{
    'Name' = 'ScriptName3'
    'Description' = 'MyDes'
} | ConvertTo-Json

$Result = Invoke-RestMethod -Uri 'https://localhost:44354/api/Script' -UseDefaultCredentials -Method Post -Body $Body -ContentType 'application/json'

$null = Invoke-RestMethod -Uri 'https://localhost:44354/api/Script/11' -UseDefaultCredentials

$Body = @{
    'ScriptBody' = 'Write-Host "test2"'
    'ScriptLanguageId' = 1
} | ConvertTo-JSON

Invoke-RestMethod -Uri "https://localhost:44354/api/Script/$($Result)/Version" -UseDefaultCredentials -Method Post -Body $Body -ContentType 'application/json'
