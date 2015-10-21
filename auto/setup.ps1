param (
    $WithNode = $False,
    $WithNpm = $False,
    $WithPandoc = $True,
    $WithGit = $False,
    $debug = $True
)

$autoDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$autoDir\common.lib.ps1"
. "$autoDir\config.lib.ps1"
. "$autoDir\fs.lib.ps1"

$winShell = New-Object -ComObject Shell.Application
$pathCfgNames = @()
$rootDir = Resolve-Path "$autoDir\.."
$downloadDir = Resolve-Path "$rootDir\$(Get-ConfigValue DownloadDir)"
$libDir = Empty-Dir "$rootDir\$(Get-ConfigValue LibDir)"
$homeDir = Safe-Dir "$rootDir\$(Get-ConfigValue HomeDir)"
$appDataDir = Safe-Dir "$rootDir\$(Get-ConfigValue AppDataDir)"
$localAppDataDir = Safe-Dir "$rootDir\$(Get-ConfigValue LocalAppDataDir)"

function Register-Path($cfgName) {
    $Script:pathCfgNames += $cfgName
    Debug "Registered Path: $cfgName -> $($Script:libDir)\$(Get-ConfigValue $cfgName)"
}

function Write-EnvironmentFile() {
    $envFile = "$autoDir\env.cmd"
    $txt = "REM --- MD Bench Environment Setup ---`n`n"
    [string]$h = $Script:homeDir
    $homeDrive = $h.Substring(0, $h.IndexOf("\"))
    $homePath = $h.Substring($h.IndexOf("\"))
    $txt += "SET USERPROFILE=$h`n"
    $txt += "SET HOMEDRIVE=$homeDrive`n"
    $txt += "SET HOMEPATH=$homePath`n"
    $txt += "SET APPDATA=$($Script:appDataDir)`n"
    $txt += "SET LOCALAPPDATA=$($Script:localAppDataDir)`n"
    $txt += "SET PATH=%SystemRoot%;%SystemRoot%\System32"
    foreach ($cfgValue in $Script:pathCfgNames) {
        $txt += ";$($Script:libDir)\$(Get-ConfigValue $cfgValue)"
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
    $7z = "$Script:libDir\$(Get-ConfigValue SvZExe)"
    & $7z "x" "-y" "-o$targetDir" "$archive" | Out-Null
    if (!$?) {
        throw "Extracting $archive failed"
    }
}

function Extract-Msi($archive, $targetDir) {
    Debug "Extracting MSI $archive to $targetDir"
    $targetDir = Safe-Dir $targetDir
    $lessmsi = "$Script:libDir\$(Get-ConfigValue LessMsiExe)"
    & $lessmsi x "$archive" "$targetDir\"
}

#
# Setup Routines
#

function Setup-7Zip() {
    $archive = Find-Download SvZArchive
    $dir = Safe-LibDir SvZDir
    ShellUnzip-Archive $archive $dir
    Register-Path SvZPath
}

function Setup-LessMsi() {
    $archive = Find-Download LessMsiArchive
    $dir = Safe-LibDir LessMsiDir
    Unzip-Archive $archive $dir
}

function Setup-NodeJS() {
    $nodeExe = Find-Download NodeExe
    $dir = Safe-LibDir NodeDir
    Copy-Item $nodeExe $dir
    Register-Path NodePath
}

function Setup-Npm() {
    $archive = Find-Download NpmArchive
    $dir = Safe-LibDir NodeDir
    Unzip-Archive $archive $dir
}

function Setup-Git() {
    $archive = Find-Download GitArchive
    $dir = Safe-LibDir GitDir
    Unzip-Archive $archive $dir
    Push-Location $dir
        Write-Output "Running post-install for Git..."
        cmd /C post-install.bat
        Write-Output "Finished post-install for Git"
    Pop-Location
    Register-Path GitPath
}

function Setup-Pandoc() {
    $archive = Find-Download PandocArchive
    $dir = Safe-LibDir PandocDir
    Extract-Msi $archive $dir
    Move-Item $dir\SourceDir\Pandoc\* $dir\
    Remove-Item -Force -Recurse $dir\SourceDir
    Register-Path PandocPath
}

Setup-7Zip
Setup-LessMsi
if ($WithNode) { Setup-NodeJS }
if ($WithNpm) { Setup-Npm }
if ($WithPandoc) { Setup-Pandoc }
if ($WithGit) { Setup-Git }
Write-EnvironmentFile
