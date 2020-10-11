$Parent = (Get-Item $PsScriptRoot).Parent.FullName

Get-ChildItem "$PsScriptRoot\Models" -Filter '*.cs' | Remove-Item -Force

Get-ChildItem "$Parent\EphIt.Db.Models\Generated" -Filter '*.cs' | Remove-Item -Force

Push-Location $PsScriptRoot

dotnet ef dbcontext scaffold "Server=Lab-CM.Home.Lab;Database=EphIt;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --startup-project ..\..\EphIt.DummyProject -o Models

Get-ChildItem "$PsScriptRoot\Models" -Exclude 'EphItContext.cs' | ForEach-Object {
    Move-Item -Path $_.FullName -Destination "$Parent\EphIt.Db.Models\Generated\" -Force
}