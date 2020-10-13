$Parent = (Get-Item $PsScriptRoot).Parent.FullName

Get-ChildItem "$PsScriptRoot\Models" -Filter '*.cs' | Remove-Item -Force

Get-ChildItem "$Parent\EphIt.Db.Models\Generated" -Filter '*.cs' | Remove-Item -Force

Push-Location $PsScriptRoot

dotnet ef dbcontext scaffold "Server=Lab-CM.Home.Lab;Database=EphIt;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --startup-project ..\..\EphIt.DummyProject -o Models

Get-ChildItem "$PsScriptRoot\Models" -Exclude 'EphItContext.cs' | ForEach-Object {
    Move-Item -Path $_.FullName -Destination "$Parent\EphIt.Db.Models\Generated\" -Force
}

$Template = @'
using System;

namespace EphIt.Db.Models
{{
    public partial class {0}
    {{
        public {0}()
        {{
            
        }}

        public {0}({1} obj)
        {{
{2}
        }}

{3}

    }}
}}
'@
$OutputArray = @()

$Files = Get-ChildItem "$Parent\EphIt.Db.Models\Generated" -Filter '*.cs'
$Files | Foreach-Object {
    $ClassName = $_.BaseName
    $NewClassName = "VM$($ClassName)"
    $SetterString = @()
    $PropertyString = @()
    $Content = Get-Content $_.FullName
    foreach($line in $Content){
        if($line.ToLower().Contains('{ get; set; }')){
            if(-not $line.ToLower().Contains(' virtual ')){
                $PropertyString += $line
                $PropertyName = ($line.split('{')[0]).Trim().Split(' ')[-1]
                $SetterString += "            $PropertyName = obj.$($PropertyName);"
            }
        }
    }
    $Output = $Template -f @( $NewClassName, $ClassName, ($SetterString -join [system.environment]::NewLine),($PropertyString -join [system.environment]::NewLine) )
    $Output | Out-File -Force -FilePath "$Parent\EphIt.Db.Models\Generated\$($NewClassName).cs" -Encoding utf8
}
