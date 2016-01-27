param ($message = $null)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\common.lib.ps1"
. "$scriptsLib\config.lib.ps1"

if ($args) {
    powershell -NoLogo -NoProfile @args
    return
}

Clear-Host
Get-Content "$scriptsLib\banner.txt" -Encoding UTF8 `
    | % { $_.Replace("`$VERSION`$", [string]::Format("{0,9}", $(Get-ConfigValue Version "0.0.0"))) } `
    | Out-Default

if ($message) {
    $prefix = "====#### "
    $postfix = " ####===="
    $message = $message.Trim()
    $d = 60 - $prefix.Length - $postfix.Length - $message.Length
    if ($d -lt 0) {
        $text = $prefix + $message + $postfix
    } else {
        $l = "-" * [Math]::Floor($d / 2.0)
        $r = "-" * [Math]::Ceiling($d / 2.0)
        $text = $l + $prefix + $message + $postfix + $r
    }
    Write-Output $text
    Write-Output ""
}

$props = @()
if (test-Path ".svn" -PathType Container) {
    $props += "under SVN version control"
}
if (Test-Path ".git" -Pathtype Container) {
    $props += "under Git version control"
}
if (Test-Path "bower.json" -PathType Leaf) {
    if (Test-Path "bower_components" -PathType Container) {
        $props += "consuming Bower components"
    } else {
        $props += "consuming Bower components, run 'bower install' to load them"
    }
}
if (Test-Path "package.json" -PathType Leaf) {
    if (Test-Path "node_modules" -PathType Container) {
        $props += "a Node Package"
    } else {
        $props += "a Node Package, run 'npm install' to load dependencies"
    }
}
if (Test-Path "gulpfile.js" -PathType Leaf) {
    $props += "automated with Gulp"
}
if ((Test-Path "Gruntfile.js" -PathType Leaf) -or (Test-Path "Gruntfile.coffee" -PathType Leaf)) {
    $props += "automated with Grunt"
}
if ($props.Count -gt 0) {
    Write-Output "The Project is"
    foreach ($p in $props) {
        Write-Output "- $p"
    }
    Write-Output ""
}

powershell -NoLogo -NoProfile -NoExit
