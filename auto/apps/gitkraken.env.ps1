$appData = Get-ConfigValue AppDataDir
$gitKrakenConfig = Resolve-Path "$appData\.gitkraken\config"

if ($gitKrakenConfig)
{
    $projectsDir = Get-ConfigValue ProjectRootDir
    $projectsDir = $projectsDir.Replace("\", "\\")
    $enc = New-Object System.Text.UTF8Encoding ($False)
    $json = [IO.File]::ReadAllText($gitKrakenConfig, $enc)
    [regex]$p = "`"projectsDirectory`"\s*:\s*`".*?`""
    $json = $p.Replace($json, "`"projectsDirectory`":`"$projectsDir`"")
    [IO.File]::WriteAllText($gitKrakenConfig, $json, $enc)
}
