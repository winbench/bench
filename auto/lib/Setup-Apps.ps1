param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

Set-Debugging $debug

$apps = Get-ConfigValue Apps

$winShell = New-Object -ComObject Shell.Application

$paths = @()
$tempDir = Empty-Dir (Get-ConfigDir TempDir)
$downloadDir = Safe-Dir (Get-ConfigDir DownloadDir)
$libDir = Safe-Dir (Get-ConfigDir LibDir)
$homeDir = Safe-Dir (Get-ConfigDir HomeDir)
$appDataDir = Safe-Dir (Get-ConfigDir AppDataDir)
$localAppDataDir = Safe-Dir (Get-ConfigDir LocalAppDataDir)
$desktopDir = Safe-Dir "$homeDir\Desktop"
$documentsDir = Safe-Dir "$homeDir\Documents"

if (!(Test-Path $downloadDir)) { return }
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
    if (Get-ConfigValue UseProxy) {
        $env:HTTP_PROXY = Get-ConfigValue HttpProxy
        $env:HTTPS_PROXY = Get-ConfigValue HttpsProxy
    }
    $env:USERPROFILE = $h
    $env:HOMEDRIVE = $homeDrive
    $env:HOMEPATH = $homePath
    $env:APPDATA = $Script:appDataDir
    $env:LOCALAPPDATA = $Script:localAppDataDir
}

function Update-EnvironmentPath() {
    $env:PATH = "$env:SystemRoot;$env:SystemRoot\System32;$env:SystemRoot\System32\WindowsPowerShell\v1.0"
    foreach ($path in $Script:paths) {
        $env:PATH = "$path;$env:PATH"
    }
}

function Write-EnvironmentFile() {
    $envFile = "$Script:rootDir\auto\env.cmd"
    $nl = [Environment]::NewLine
    $txt = "@ECHO OFF$nl"
    $txt += "REM **** MD Bench Environment Setup ****$nl$nl"
    if (Get-ConfigValue UseProxy) {
        $txt += "SET HTTP_PROXY=$(Get-ConfigValue HttpProxy)$nl"
        $txt += "SET HTTPS_PROXY=$(Get-ConfigValue HttpsProxy)$nl"
    }
    [string]$h = $Script:homeDir
    $homeDrive = $h.Substring(0, $h.IndexOf("\"))
    $homePath = $h.Substring($h.IndexOf("\"))
    $txt += "SET USERPROFILE=$h$nl"
    $txt += "SET HOMEDRIVE=$homeDrive$nl"
    $txt += "SET HOMEPATH=$homePath$nl"
    $txt += "SET APPDATA=${Script:appDataDir}$nl"
    $txt += "SET LOCALAPPDATA=${Script:localAppDataDir}$nl"
    $txt += "SET BENCH_HOME=${Script:rootDir}$nl"
    $txt += "SET L=${Script:libDir}$nl"
    $txt += "SET BENCH_PATH=${Script:rootDir}\auto"
    foreach ($path in $Script:paths) {
        $txt += ";%L%$($path.Substring(${Script:libDir}.Length))"
    }
    $txt += $nl
    $txt += "SET PATH=%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0$nl"
    $txt += "SET PATH=%BENCH_PATH%;%PATH%"
    $txt | Out-File -Encoding oem -FilePath $envFile
    Debug "Written environment file to $envFile"
}

function App-Typ([string]$name) { return Get-AppConfigValue $name Typ "default" }
function App-Archive([string]$name) { return Get-AppConfigValue $name Archive }
function App-ArchiveSubDir([string]$name) { return Get-AppConfigValue $name ArchiveSubDir }
function App-Download([string]$name) { return Get-AppConfigValue $name Download }
function App-NpmPackage([string]$name) { return Get-AppConfigValue $name NpmPackage $name.ToLowerInvariant() }
function App-NpmForce([string]$name) { return Get-AppConfigValue $name NpmForceInstall $false }
function App-Dir([string]$name) {
    switch (App-Typ $name) {
        "npm" {
            return App-Dir Npm
        }
        default {
            return [IO.Path]::Combine(
                $Script:libDir,
                (Get-AppConfigValue $name Dir $name.ToLowerInvariant())) 
        }
    }
}
function App-Path([string]$name) {
    switch (App-Typ $name) {
        "npm" {
            return App-Path NpmBootstrap
        }
        default {
            $appDir = App-Dir $name
            $cfgPath = Get-AppConfigValue $name Path ""
            if ($cfgPath -is [string]) {
                return [IO.Path]::Combine($appDir, $cfgPath)
            } elseif ($cfgPath -is [array]) {
                return [IO.Path]::Combine($appDir, $cfgPath[0])
            }
        }
    }
}
function App-Paths([string]$name) {
    switch (App-Typ $name) {
        "npm" {
            return App-Paths NpmBootstrap
        }
        default {
            $paths = @()
            $appDir = App-Dir $name
            $cfgPath = Get-AppConfigValue $name Path ""
            if ($cfgPath -is [string]) {
                $paths += [IO.Path]::Combine($appDir, $cfgPath)
            } elseif ($cfgPath -is [array]) {
                foreach ($p in $cfgPath) {
                    $paths += [IO.Path]::Combine($appDir, $p)
                }
            }
            return $paths
        }
    }
}
function App-Exe([string]$name, [bool]$checkExist = $true) {
    $typ = App-Typ $name

    $path = [IO.Path]::Combine(
        (App-Path $name),
        (Get-AppConfigValue $name Exe "${name}.exe"))
    if ($checkExist -and ![IO.file]::Exists($path)) {
        return $null
    } else {
        return $path
    }
}
function App-Register([string]$name) { return Get-AppConfigValue $name Register $true }

function Find-Download([string]$pattern) {
    $path = Find-File $Script:downloadDir $pattern
    if (!$path) {
        throw "Download not found: $pattern"
    } else {
        Debug "Found download: $path"
    }
    return $path
}

function ShellUnzip-Archive([string]$zipFile, [string]$targetDir)
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

function Extract-Archive([string]$archive, [string]$targetDir) {
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

function Extract-Msi([string]$archive, [string]$targetDir) {
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

function Execute-Custom-Setup([string]$name) {
    $customSetupFile = "$scriptsLib\..\apps\${name}.setup.ps1"
    if (Test-Path $customSetupFile) {
        Debug "Running custom setup for $name ..."
        $old = Set-StopOnError $false
        . $customSetupFile
        $_ = Set-StopOnError $old
        Debug "Finished custom setup for $name"
    }
}

function Default-Setup([string]$name, [bool]$registerPath = $true) {
    $dir = App-Dir $name
    if (![IO.File]::Exists((App-Exe $name))) {

        Write-Host "Setting up $name ..."

        $download = App-Download $name
        if ($download) {
            [string]$src = Find-Download $download
            $mode = "copy"
            $subDir = $null
        } else {
            $archive = App-Archive $name
            [string]$src = Find-Download $archive
            if ($src.EndsWith(".msi", [StringComparison]::InvariantCultureIgnoreCase)) {
                $mode = "msi"
            } else {
                $mode = "arch"
            }
            $subDir = App-ArchiveSubDir $name
        }

        $dir = Safe-Dir $dir
        if ($subDir) {
            $target = Safe-Dir "$Script:tempDir\$name"
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

    } else {
        Write-Host "Skipping allready installed $name in $dir"
    }

    if (App-Register $name) {
        $paths = App-Paths $name
        foreach ($p in $paths) {
            Register-Path $p
        }
    }

    Execute-Custom-Setup $name
}

function Check-NpmPackage([string]$name) {
    $npm = App-Exe Npm
    if (!$npm) { throw "Node Package Manager not found" }
    $p = [regex]"^\S+ ([^@\s]*)@[^@\s]+`$"
    $list = & $npm list --global --depth 0 | ? { $p.IsMatch($_) } | % { $p.Replace($_, "`$1") }
    $packageName = App-NpmPackage $name
    return $packageName -in $list
}

function Setup-NpmPackage([string]$name) {
    $packageName = App-NpmPackage $name
    $npm = App-Exe Npm
    if (!$npm) { throw "Node Package Manager not found" }
    if ((App-NpmForce $name) -or !(Check-NpmPackage $name)) {

        Write-Host "Setting up npm package $packageName ..."

        & $npm install $packageName --global

    } else {
        Write-Host "Skipping allready installed NPM package $packageName"
    }
    Execute-Custom-Setup $name
}

Load-Environment
Update-EnvironmentPath
foreach ($name in $apps) {
    $typ = App-Typ $name
    switch ($typ) {
        "npm" { Setup-NpmPackage $name }
        default { Default-Setup $name }
    }
    Update-EnvironmentPath
}
Write-EnvironmentFile

Purge-Dir $tempDir
