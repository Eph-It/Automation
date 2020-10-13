
$Parent = (Get-Item $PSScriptRoot).Parent.FullName

$SqlProjbin = "$Parent\EphIt.Sql\bin"

$dpacFiles = Get-ChildItem -Path $SqlProjbin -Filter 'EphIt.SQL.dacpac' -Recurse
$PreviousLatest = (Get-Date).AddYears(-3)
$LatestFile = $null
foreach($dFile in $dpacFiles){
    if($dFile.LastWriteTime -gt $PreviousLatest){
        $LatestFile = $dFile
        $PreviousLatest = $dFile.LastWriteTime
    }
}

Copy-Item $LatestFile.FullName "$PSScriptRoot\" -Force