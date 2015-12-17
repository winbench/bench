param (
    $projectName = $(& ([IO.Path]::Combine([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition), "Select-Project.ps1"))),
    [switch]$debug
)

if (!$projectName) { return }

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\common.lib.ps1"
. "$scriptsLib\config.lib.ps1"
. "$scriptsLib\fs.lib.ps1"

Set-Debugging $debug
$_ = Set-StopOnError $True

if ([IO.Path]::IsPathRooted($projectName)) {
    $projectPath = $projectName
    $projectName = [IO.Path]::GetFileName($projectPath)
} else {
    Debug "Resolving project dir for: $projectName"
    $projectRoot = Safe-Dir $(Get-ConfigDir ProjectRootDir)
    $projectPath = Resolve-Path "$projectRoot\$projectName"
}

cd $projectPath

function Get-MainFiles($projectPath) {

    function Replace-Dates ($path) {
        $p = [regex]"\`$DATE\:([^\`$]+)\`$"
        $now = [DateTime]::Now
        $me = [System.Text.RegularExpressions.MatchEvaluator] { 
            param($m) 
            return $now.ToString($m.Groups[1]) 
        }
        $result = $p.Replace($path, $me)
        return $result
    }

    $mainFileList = "$projectPath\config\mainfiles"
    if (Test-Path $mainFileList) {
        return Get-Content $mainFileList -Encoding UTF8 `
            | % { Replace-Dates $_ }
    } else {
        return @()
    }
}

$searchFiles = Get-MainFiles $projectPath

$searchFiles += "index.js"
$searchFiles += "src/index.js"
$searchFiles += "src/app.js"
$searchFiles += "src/main.js"
$searchFiles += "README.md"

$foundFiles = @()
foreach ($s in $searchFiles) {
    $path = [IO.Path]::Combine($projectpath, $s)
    if (Test-Path $path -PathType Leaf) {
        $foundFiles += $path
        break
    }
}

Run-Detached code $projectPath @foundFiles
