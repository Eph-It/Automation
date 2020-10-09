Get-ChildItem "$PsScriptRoot\Models" -Filter '*.cs' | Remove-Item -Force

Push-Location $PsScriptRoot

dotnet ef dbcontext scaffold "Server=Lab-CM.Home.Lab;Database=EphIt;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --startup-project ..\EphIt.Service -o Models

