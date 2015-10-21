param (
    $WithNode = $True,
    $WithNpm = $True,
    $WithGulp = $True,
    $WithPython = $True,
    $WithPandoc = $True,
    $WithGraphViz = $True,
    $WithInkscape = $True,
    $WithMikteX = $True,
    $WithVSCode = $True,
    $WithGit = $True,
    $clean = $True,
    $debug = $True
)

$ErrorActionPreference = "Stop"

$autoDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$autoDir\common.lib.ps1"
. "$autoDir\config.lib.ps1"
. "$autoDir\fs.lib.ps1"

$winShell = New-Object -ComObject Shell.Application
$pathCfgNames = @()
$rootDir = Resolve-Path "$autoDir\.."
$downloadDir = Resolve-Path "$rootDir\$(Get-ConfigValue DownloadDir)"
$libDir = "$rootDir\$(Get-ConfigValue LibDir)"
if ($clean) {
    $libDir = Empty-Dir $libDir
} else {
    $libDir = Safe-Dir $libDir
}
$homeDir = Safe-Dir "$rootDir\$(Get-ConfigValue HomeDir)"
$appDataDir = Safe-Dir "$rootDir\$(Get-ConfigValue AppDataDir)"
$localAppDataDir = Safe-Dir "$rootDir\$(Get-ConfigValue LocalAppDataDir)"

if (!(test-Path $downloadDir)) { return }
if (!(Test-Path $libDir)) { return }

function Register-Path($cfgName) {
    $Script:pathCfgNames += $cfgName
    Debug "Registered Path: $cfgName -> ${Script:libDir}\$(Get-ConfigValue $cfgName)"
}

function Load-Environment() {
    [string]$h = $Script:homeDir
    $homeDrive = $h.Substring(0, $h.IndexOf("\"))
    $homePath = $h.Substring($h.IndexOf("\"))
    $env:USERPROFILE = $h
    $env:HOMEDRIVE = $homeDrive
    $env:HOMEPATH = $homePath
    $env:APPDATA = $Script:appDataDir
    $env:LOCALAPPDATA = $Script:localAppDataDir
}

function Write-EnvironmentFile() {
    $envFile = "$autoDir\env.cmd"
    $txt = "REM **** MD Bench Environment Setup ****`r`n`r`n"
    [string]$h = $Script:homeDir
    $homeDrive = $h.Substring(0, $h.IndexOf("\"))
    $homePath = $h.Substring($h.IndexOf("\"))
    $txt += "SET USERPROFILE=$h`r`n"
    $txt += "SET HOMEDRIVE=$homeDrive`r`n"
    $txt += "SET HOMEPATH=$homePath`r`n"
    $txt += "SET APPDATA=${Script:appDataDir}`r`n"
    $txt += "SET LOCALAPPDATA=${Script:localAppDataDir}`r`n"
    $txt += "SET PATH=%SystemRoot%;%SystemRoot%\System32"
    foreach ($cfgValue in $Script:pathCfgNames) {
        $txt += ";${Script:libDir}\$(Get-ConfigValue $cfgValue)"
    }
    $txt | Out-File -Encoding oem -FilePath $envFile
    Debug "Written environment file to $envFile"
}

function Safe-LibDir($cfgName) {
    return Safe-Dir "$Script:libDir\$(Get-ConfigValue $cfgName)"
}

function Find-Download($cfgName) {
    $pattern = Get-ConfigValue $cfgName
    $path = Find-File $Script:downloadDir $pattern
    if (!$path) {
        throw "Download not found: $pattern"
    } else {
        Debug "Found download: $path"
    }
    return $path
}

function Get-Exe($name) {
    return "${Script:libDir}\$(Get-ConfigValue "${name}Exe")"
}

function ShellUnzip-Archive($zipFile, $targetDir)
{
    Debug "Extracting (Shell) $zipFile to $targetDir"
    $zip = $Script:winShell.NameSpace($zipFile)
    foreach($item in $zip.items())
    {
        $Script:winShell.Namespace($targetDir).copyhere($item)
    }
}

function Unzip-Archive($archive, $targetDir) {
    Debug "Extracting $archive to $targetDir"
    $7z = Get-Exe SvZ
    & $7z "x" "-y" "-o$targetDir" "$archive" | Out-Null
    if (!$?) {
        throw "Extracting $archive failed"
    }
}

function Extract-Msi($archive, $targetDir) {
    Debug "Extracting MSI $archive to $targetDir"
    $targetDir = Safe-Dir $targetDir
    $lessmsi = Get-Exe LessMsi
    & $lessmsi x "$archive" "$targetDir\"
}

function Default-Setup($name, $extractMode = "zip",  $registerPath = $True) {
    if ($extractMode -eq "copy") {
        $src = Find-Download "${name}Download"
    } else {
        $src = Find-Download "${name}Archive"
    }
    $dir = Safe-LibDir "${name}Dir"
    switch ($extractMode) {
        "copy" { Copy-item $src $dir }
        "zip" { Unzip-Archive $src $dir }
        "shellzip" { ShellUnzip-Archive $src $dir }
        "msi" { Extract-Msi $src $dir }
        default { throw "Invalid extraction mode: $extractMode" }
    }
    if ($registerPath) {
        Register-Path "${name}Path"
    }
}

#
# Setup Routines
#

function Setup-7Zip() {
    Default-Setup SvZ -extractMode shellzip
}

function Setup-LessMsi() {
    Default-Setup LessMsi -registerPath $False
}

function Setup-NodeJS() {
    Default-Setup Node -extractMode copy
}

function Setup-Npm() {
    Default-Setup Npm -registerPath $False

    $npm = Get-Exe Npm
    & $npm config set registry "http://registry.npmjs.org/"
    if (Get-ConfigValue UseProxy) {
        & $npm config set "proxy" "http://$(Get-ConfigValue HttpProxy)/"
        & $npm config set "https-proxy" "http://$(Get-ConfigValue HttpsProxy)/"
    }
}

function Setup-Gulp() {
    $npm = Get-Exe Npm
    & $npm install gulp --global
}

function Setup-Python() {
    Default-Setup Python
}

function Setup-Git() {
    Default-Setup Git

    $dir = Safe-LibDir GitDir
    Push-Location $dir
        Write-Output "Running post-install for Git..."
        cmd /C post-install.bat
        Write-Output "Finished post-install for Git"
    Pop-Location
}

function Setup-Pandoc() {
    Default-Setup pandoc -extractMode "msi"

    $dir = Safe-LibDir PandocDir
    Move-Item $dir\SourceDir\Pandoc\* $dir\
    Remove-Item -Force -Recurse $dir\SourceDir
}

function Setup-GraphViz() {
    Default-Setup GraphViz
}

function Setup-Inkscape() {
    Default-Setup Inkscape

    $dir = Safe-LibDir InkscapeDir
    Move-Item $dir\inkscape\* $dir\
    Remove-Item -Force $dir\inkscape
}

function Setup-MikTeX() {
    Default-Setup MikTeX
}

function Setup-VSCode() {
    Default-Setup VSCode
}

Load-Environment
Setup-7Zip
Setup-LessMsi
if ($WithNode) { Setup-NodeJS }
if ($WithNpm) { Setup-Npm }
if ($WithGulp) { Setup-Gulp }
if ($WithPython) { Setup-Python }
if ($WithPandoc) { Setup-Pandoc }
if ($WithGraphViz) { Setup-GraphViz }
if ($WithInkscape) { Setup-Inkscape }
if ($WithMikteX) { Setup-MikTeX }
if ($WithVSCode) { Setup-VSCode }
if ($WithGit) { Setup-Git }
Write-EnvironmentFile
