Param(
    [string]$MigrationName,
    [switch]$IncludeSqlFolderUpdates
)

Push-Location $PSScriptRoot

#dotnet ef migrations add $MigrationName --startup-project ..\..\EphIt.Server

if(-not $IncludeSqlFolderUpdates){
    Pop-Location
    return
}

$MigrationFile = Get-ChildItem ".\Migrations" -Filter "*_$($MigrationName).cs"
if($MigrationFile.Count -ne 1) { throw 'Error getting migration file';return }


$UpdateSqlFiles = @()
$SqlStatements = @()

$SqlStatementTemplate = @"

migrationBuilder.Sql(@"

{0}

");
"@

$SqlFiles = Get-ChildItem -Path ".\Sql" -Filter '*.sql' -Recurse
Foreach($sqlFile in $SqlFiles){
    $Content = Get-Content $sqlFile.FullName -Raw
    $SnapShotFile = ".\Migrations\Sql.Snapshot\$($sqlFile.Directory.Name)\$($sqlFile.Name)"
    if(Test-Path $SnapShotFile){
        $SnapShotContent = Get-Content $SnapShotFile -Raw
        if($SnapShotContent -ne $Content){
            $UpdateSqlFiles += $sqlFile    
        }
    }
    else{
        $UpdateSqlFiles += $sqlFile
    }
}

Foreach($sqlFile in $UpdateSqlFiles){
    $SnapShotFile = ".\Migrations\Sql.Snapshot\$($sqlFile.Directory.Name)\$($sqlFile.Name)"
    $null = Copy-Item $sqlFile.FullName $SnapShotFile -Force
    $SqlContent = Get-Content $SnapShotFile -Raw
    $SqlContent = $SqlContent.Replace('"','\"')
    $SqlStatement = $SqlStatementTemplate -f $SqlContent
    $SqlStatements += $SqlStatement
}

$SqlFiles = Get-ChildItem -Path ".\Migrations\Sql.Snapshot" -Filter '*.sql' -Recurse

Foreach($sqlFile in $SqlFiles){
    $ExistingFile = ".\Sql\$($sqlFile.Directory.Name)\$($sqlFile.Name)"
    if(-not ( Test-Path $ExistingFile )){
        $ObjectType = $sqlFile.Directory.Name
        $SqlStatements += $SqlStatementTemplate -f "DROP $($ObjectType) IF EXISTS $($sqlFile.BaseName)"
        Remove-Item $sqlFile.Fullname -Force
    }
}

$InsertString = ""

foreach($sqlStatement in $SqlStatements){
    $InsertString += $SqlStatement.Replace("`n","`n            ")
}

if([string]::IsNullOrEmpty($InsertString)){
    Pop-Location
    exit    
}

$MigrationFileContentArray = Get-Content $MigrationFile.FullName
$MigrationFileContent = ""
$inserted = $false
for ($i = 0; $i -lt $MigrationFileContentArray.Count; $i++) {
    if(-not [string]::IsNullOrEmpty($MigrationFileContentArray[$i]) -and $inserted -eq $false ){
        if($MigrationFileContentArray[$i].Trim() -eq '}' -and $MigrationFileContentArray[$i + 2].ToLower().Contains('protected override void down(migrationbuilder migrationbuilder)')){
            $MigrationFileContent += "$($InsertString)$([System.Environment]::NewLine)"
            $inserted = $true
        }
    }
    $MigrationFileContent += "$($MigrationFileContentArray[$i])$([System.Environment]::NewLine)"
}

$MigrationFileContent | Out-File $MigrationFile.FullName -Force -Encoding utf8

Pop-Location
