$Uri = 'https://localhost:44354/'


Invoke-RestMethod -Uri 'https://localhost:44354/api/Script?Name=ScriptName3' -UseDefaultCredentials

$Body = @{
    'Name' = 'ScriptName3'
    'Description' = 'MyDes'
} | ConvertTo-Json

Invoke-RestMethod -Uri 'https://localhost:44354/api/Script' -UseDefaultCredentials -Method Post -Body $Body -ContentType 'application/json'

Invoke-RestMethod -Uri 'https://localhost:44354/api/Script/11' -UseDefaultCredentials