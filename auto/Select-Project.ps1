$autoDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$projectRoot = [IO.Path]::Combine($autoDir, "..", "projects")

[array]$projectNames = [IO.Directory]::GetDirectories($projectRoot) | % { [IO.Path]::GetFileName($_) } | sort


for ($i = 0; $i -lt $projectNames.Count; $i++) {
    
    Write-Host ([string]::Format("{0:D2}) {1}", $i+1, $projectNames[$i]))
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