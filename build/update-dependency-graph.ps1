$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = [IO.Path]::GetDirectoryName($myDir)
$scriptsDir = Resolve-Path "$rootDir\auto\lib"
$docsDir = Resolve-Path "$rootDir\docs"

$sourceDirs = @(
    $rootDir,
    "$rootDir\auto",
    "$rootDir\auto\lib",
    "$rootDir\actions"
)
$constNodes = @(
    "BenchDashboard.exe",
    "BenchLib.dll"
)
$filter = @("*.bat", "*.cmd", "*.ps1")
$exclude = @("runps.cmd", "env.cmd")
$targetFile = "$docsDir\src-static\graph\dependencies.gw"
$imageFile =  "$docsDir\static\img\dependencies.svg"
$graphLabel = "Dependencies"

$nodeStyles = @(
    @(@("*.bat"), @{ 
        "color"="#86AE1E";
        "style"="filled";
        "fillcolor"="#86AE1E";
        "fontcolor"="#FFFFFF";
    }),
    @(@("*\actions\*.cmd"), @{ 
        "color"="#267D41";
        "style"="filled";
        "fillcolor"="#267D41";
        "fontcolor"="#FFFFFF";
    }),
    @(@("*.cmd"), @{ 
        "color"="#339999";
        "style"="filled";
        "fillcolor"="#339999";
        "fontcolor"="#FFFFFF";
    }),
    @(@("*.lib.ps1"), @{
        "color"="#0072C6";
        "style"="filled";
        "fillcolor"="#0072C6";
        "fontcolor"="#FFFFFF";
    }),
    @(@("*.ps1"), @{
        "color"="#1BA1E2";
        "style"="filled";
        "fillcolor"="#1BA1E2";
        "fontcolor"="#FFFFFF";
    }),
    @(@("*.exe", "*.dll"), @{
        "color"="#86AE1E";
        "style"="filled";
        "fillcolor"="#86AE1E";
        "fontcolor"="#FFFFFF";
    })
)

function format-style($style) {
    $f = ""
    foreach ($k in $style.Keys) {
        if ($f.Length -gt 0) { $f += ", " }
        $f += "$k=`"$($style[$k])`""
    }
    return $f
}

function get-style($n) {
    foreach ($s in $nodeStyles) {
        $filter = $s[0]
        foreach ($f in $filter) {
            if ($n -like $f) {
                return " [$(format-style $s[1])]"
            }
        }
    }
    return "";
}

[array]$files = $sourceDirs `
    | % { Get-ChildItem "$_\*" -Include $filter } `
    | % { $_.FullName }

if (Test-Path $targetFile) { del $targetFile }
$s = New-Object System.IO.StreamWriter ($targetFile, [Text.Encoding]::UTF8)
function println($line) {
    $s.WriteLine($line)
}
println "digraph `"$graphLabel`" {"
println "  graph [rankdir=LR];"
println ([IO.File]::ReadAllText("$myDir\style.txt"))

foreach ($n in $constNodes) {
    println "  `"$n`"$(get-style $n);"
}
foreach ($f in $files) {
    $n = [IO.Path]::GetFileName($f)
    if ($exclude -icontains $n) { continue }
    println "  `"$n`"$(get-style $f);"
}
foreach ($f in $files) {
    $n = [IO.Path]::GetFileName($f)
    if ($exclude -icontains $n) { continue }
    [string]$txt = [IO.File]::ReadAllText($f)
    foreach ($cn in $constNodes) {
        if ($txt.Contains($cn)) {
            println "  `"$n`" -> `"$cn`";"
        }
    }
    foreach ($f2 in $files) {
        $n2 = [IO.Path]::GetFileName($f2)
        if ($exclude -icontains $n2) { continue }
        if ($txt.Contains($n2)) {
            println "  `"$n`" -> `"$n2`";"
        }
    }
}

println "}"
$s.Close()

& "$scriptsDir\Load-ClrLibs.ps1"

$cfg = New-Object Mastersign.Bench.BenchConfiguration ($rootDir, $true, $true, $true)
$benchEnv = New-Object Mastersign.Bench.BenchEnvironment ($cfg)
$benchEnv.Load()

dot $targetFile "-o$imageFile" -Tsvg
