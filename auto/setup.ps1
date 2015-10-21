param ([switch]$debug)

$autoDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$autoDir\common.lib.ps1"
. "$autoDir\config.lib.ps1"
. "$autoDir\fs.lib.ps1"

Set-Debugging $debug
$_ = Set-StopOnError $true

$apps = Get-ConfigValue Apps

$winShell = New-Object -ComObject Shell.Application

$paths = @()
$rootDir = Resolve-Path "$autoDir\.."
$tmpDir = Empty-Dir "$rootDir\$(Get-ConfigValue TempDir)"
$downloadDir = Resolve-Path "$rootDir\$(Get-ConfigValue DownloadDir)"
$libDir = Safe-Dir "$rootDir\$(Get-ConfigValue LibDir)"
$homeDir = Safe-Dir "$rootDir\$(Get-ConfigValue HomeDir)"
$appDataDir = Safe-Dir "$rootDir\$(Get-ConfigValue AppDataDir)"
$localAppDataDir = Safe-Dir "$rootDir\$(Get-ConfigValue LocalAppDataDir)"

if (!(test-Path $downloadDir)) { return }
if (!(Test-Path $libDir)) { return }

function Register-Path($path) {
    if (!($Script:paths -contains $path)) {
        $Script:paths += $path
        Debug "Registered Path: $path"
    }
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
    $txt += "SET BENCH_PATH=${Script:autoDir}"
    foreach ($path in $Script:paths) {
        $txt += ";$path"
    }
    $txt += "`r`n"
    $txt += "SET PATH=%BENCH_PATH%;%PATH%`r`n"
    $txt | Out-File -Encoding oem -FilePath $envFile
    Debug "Written environment file to $envFile"
}

function App-Archive($name) { return Get-ConfigValue "${name}Archive" }
function App-ArchiveSubDir($name) { return Get-ConfigValue "${name}ArchiveSubDir" }
function App-Download($name) { return Get-ConfigValue "${name}Download" }
function App-Dir($name) {
    return [IO.Path]::Combine(
        $Script:libDir,
        (Get-ConfigValue "${name}Dir" $name.ToLowerInvariant())) 
}
function App-Path($name) { 
    return [IO.Path]::Combine(
        (App-Dir $name),
        (Get-ConfigValue "${name}Path" ""))
}
function App-Exe($name, $checkExist = $true) {
    $path = [IO.Path]::Combine(
        (App-Path $name),
        (Get-ConfigValue "${name}Exe" "${name}.exe"))
    if ($checkExist -and ![IO.file]::Exists($path)) {
        return $null
    } else {
        return $path
    }
}
function App-Register($name) { return Get-ConfigValue "${name}Register" $true }

function Find-Download($pattern) {
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
    $zip = ${Script:winShell}.NameSpace($zipFile)
    if (!$zip) {
        throw "Invalid ZIP file: $zipFile"
    }
    $trg = ${Script:winShell}.NameSpace($targetDir)
    if (!$trg) {
        throw "Invalid target directory: $targetDir"
    }
    foreach($item in $zip.items())
    {
        $trg.copyhere($item)
    }
}

function Extract-Archive($archive, $targetDir) {
    $7z = App-Exe SvZ
    $targetDir = Safe-Dir $targetDir
    if ($7z) {
        Debug "Extracting $archive to $targetDir"
        & $7z "x" "-y" "-o$targetDir" "$archive" | Out-Null
        if (!$?) {
            throw "Extracting $archive failed"
        }
    } else {
        ShellUnzip-Archive $archive $targetDir
    }
}

function Extract-Msi($archive, $targetDir) {
    Debug "Extracting MSI $archive to $targetDir"
    $targetDir = Safe-Dir $targetDir
    $lessmsi = App-Exe LessMsi
    if ($lessmsi) {
        & $lessmsi x "$archive" "$targetDir\"
        if (!$?) {
            throw "Extracting $archive failed"
        }
    } else {
        throw "Missing LessMsi for MSI extraction"
    }
}

function Default-Setup([string]$name, [bool]$registerPath = $true) {
    if (App-Download $name) {
        $src = Find-Download (App-Download $name)
        $mode = "copy"
        $subDir = $null
    } else {
        [string]$src = Find-Download (App-Archive $name)
        if ($src.EndsWith(".msi", [StringComparison]::InvariantCultureIgnoreCase)) {
            $mode = "msi"
        } else {
            $mode = "arch"
        }
        $subDir = App-ArchiveSubDir $name
    }

    if (App-Register $name) {
        Register-Path (App-Path $name)
    }

    $dir = App-Dir $name
    if ([IO.File]::Exists((App-Exe $name))) {
        Write-Host "Skipping allready installed $name in $dir"
        return
    }
    $dir = Safe-Dir $dir
    if ($subDir) {
        $target = Safe-Dir "$Script:tmpDir\$name"
    } else {
        $target = $dir
    }
    switch ($mode) {
        "copy" { Copy-item $src $target }
        "arch" { Extract-Archive $src $target }
        "msi" { Extract-Msi $src $target }
    }
    if ($subDir) {
        Move-Item "$target\$subDir\*" "$dir\"
        Purge-Dir $target
    }
}

#
# Setup Routines
#

function Setup-7Zip() {
    Default-Setup SvZ
}

function Setup-LessMsi() {
    Default-Setup LessMsi
}

function Setup-NodeJS() {
    Default-Setup Node
}

function Setup-Npm() {
    Default-Setup Npm

    $npm = App-Exe Npm
    if (!$npm) { throw "Node Package Manager not found" }
    & $npm config set registry "http://registry.npmjs.org/"
    if (Get-ConfigValue UseProxy) {
        & $npm config set "proxy" "http://$(Get-ConfigValue HttpProxy)/"
        & $npm config set "https-proxy" "http://$(Get-ConfigValue HttpsProxy)/"
    } else {
        & $npm config delete "proxy"
        & $npm config delete "https-proxy"
    }
}

function Setup-Gulp() {
    $npm = App-Exe Npm
    if (!$npm) { throw "Node Package Manager not found" }
    & $npm install gulp --global
}

function Setup-Python() {
    Default-Setup Python
}

function Setup-Git() {
    Default-Setup Git

    #$dir = App-Dir Git
    #Push-Location $dir
    #    Write-Output "Running post-install for Git..."
    #    $old = Set-StopOnError $false
    #    cmd /C post-install.bat
    #    Set-StopOnError $old
    #    Write-Output "Finished post-install for Git"
    #Pop-Location
}

function Setup-Pandoc() {
    Default-Setup pandoc
}

function Setup-GraphViz() {
    Default-Setup GraphViz
}

function Setup-Inkscape() {
    Default-Setup Inkscape
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
if ($apps -contains "Node") { Setup-NodeJS }
if ($apps -contains "Npm") { Setup-Npm }
if ($apps -contains "Gulp") { Setup-Gulp }
if ($apps -contains "Python") { Setup-Python }
if ($apps -contains "Pandoc") { Setup-Pandoc }
if ($apps -contains "GraphViz") { Setup-GraphViz }
if ($apps -contains "Inkscape") { Setup-Inkscape }
if ($apps -contains "MikteX") { Setup-MikTeX }
if ($apps -contains "VSCode") { Setup-VSCode }
if ($apps -contains "Git") { Setup-Git }
Write-EnvironmentFile

Purge-Dir $tmpDir