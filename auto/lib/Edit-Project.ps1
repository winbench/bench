param (
    $projectName = $(& ([IO.Path]::Combine([IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition), "Select-Project.ps1"))),
    [switch]$debug
)

if (!$projectName) { return }

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"
. "$scriptsLib\appconfig.lib.ps1"

Set-Debugging $debug

$projectPath = Get-ProjectPath $projectName
$projectName = Get-ProjectName $projectName
$editor = App-Exe (Get-ConfigValue EditorApp)
if (!$editor) {
    throw "Editor not found"
} else {
    $editor = [IO.Path]::GetFileName($editor)
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

$defaultMainFiles = @(
    "README.md",
    "src/main.js",
    "src/app.js",
    "src/index.js",
    "index.js"
)
$searchFiles = $defaultMainFiles + (Get-MainFiles $projectPath)
[Array]::Reverse($searchFiles)

$foundFiles = @()
foreach ($s in $searchFiles) {
    $path = [IO.Path]::Combine($projectPath, $s)
    Debug "Test for main file: $path"
    if (Test-Path $path -PathType Leaf) {
        $foundFiles += $path
        break
    }
}

Debug "Found main file(s): $foundFiles"

Run-Detached $editor $projectPath @foundFiles
