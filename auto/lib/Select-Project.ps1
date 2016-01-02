param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

Set-Debugging $debug

$projectRoot = Safe-Dir (Get-ConfigPathValue ProjectRootDir)

[array]$projectNames = [IO.Directory]::GetDirectories($projectRoot) `
    | % { [IO.Path]::GetFileName($_) } `
    | sort

if ($projectNames.Count -eq 0) {
    Write-Host "No projects found."
    Pause
    return $null
}

for ($i = 0; $i -lt $projectNames.Count; $i++) {
    
    Write-Host ([string]::Format("{0,3}) {1}", $i+1, $projectNames[$i]))
}
Write-Host ""

$selectedName = $null
do {
    $number = Read-host "Project Number"
    try {
        $number = [int]::Parse($number)
        if ($number -gt 0 -and $number -le $projectNames.Count) {
            $selectedName = $projectNames[$number - 1]
        }
    } catch { }
} while ($selectedName -eq $null)

Write-Host "Selected Project: $selectedName"
Write-Host ""

return $selectedName
