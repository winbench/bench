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
$desktopDir = Safe-Dir "$homeDir\Desktop"
$documentsDir = Safe-Dir "$homeDir\Documents"

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

function Write-EnvironmentFile() {
    $envFile = "$autoDir\env.cmd"
    $txt = "REM **** MD Bench Environment Setup ****`r`n`r`n"
    if (Get-ConfigValue UseProxy) {
        $txt += "SET HTTP_PROXY=$(Get-ConfigValue HttpProxy)`r`n"
        $txt += "SET HTTPS_PROXY=$(Get-ConfigValue HttpsProxy)`r`n"
    }
    [string]$h = $Script:homeDir
    $homeDrive = $h.Substring(0, $h.IndexOf("\"))
    $homePath = $h.Substring($h.IndexOf("\"))
    $txt += "SET USERPROFILE=$h`r`n"
    $txt += "SET HOMEDRIVE=$homeDrive`r`n"
    $txt += "SET HOMEPATH=$homePath`r`n"
    $txt += "SET APPDATA=${Script:appDataDir}`r`n"
    $txt += "SET LOCALAPPDATA=${Script:localAppDataDir}`r`n"
    $txt += "SET BENCH_HOME=${Script:rootDir}`r`n"
    $txt += "SET L=${Script:libDir}`r`n"
    $txt += "SET BENCH_PATH=${Script:autoDir}"
    foreach ($path in $Script:paths) {
        $txt += ";%L%$($path.Substring(${Script:libDir}.Length))"
    }
    $txt += "`r`n"
    $txt += "SET PATH=%BENCH_PATH%;%PATH%"
    $txt | Out-File -Encoding oem -FilePath $envFile
    Debug "Written environment file to $envFile"
}

function App-Typ($name) { return Get-ConfigValue "${name}Typ" "default" }
function App-Archive($name) { return Get-ConfigValue "${name}Archive" }
function App-ArchiveSubDir($name) { return Get-ConfigValue "${name}ArchiveSubDir" }
function App-Download($name) { return Get-ConfigValue "${name}Download" }
function App-NpmPackage($name) { return Get-ConfigValue "${name}NpmPackage" $name.ToLowerInvariant() }
function App-Dir($name) {
    switch (App-Typ $name) {
        "npm" {
            return App-Dir Npm
        }
        default {
            return [IO.Path]::Combine(
                $Script:libDir,
                (Get-ConfigValue "${name}Dir" $name.ToLowerInvariant())) 
        }
    }
}
function App-Path($name) {
    switch (App-Typ $name) {
        "npm" {
            return App-Path NpmBootstrap
        }
        default {
            return [IO.Path]::Combine(
                (App-Dir $name),
                (Get-ConfigValue "${name}Path" ""))
        }
    }
}
function App-Exe($name, $checkExist = $true) {
    $typ = App-Typ $name

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

function Execute-Custom-Setup($name) {
    $customSetupFile = "$autoDir\apps\${name}.setup.ps1"
    if (Test-Path $customSetupFile) {
        Debug "Running custom setup for $name ..."
        $old = Set-StopOnError $false
        . $customSetupFile
        $_ = Set-StopOnError $old
        Debug "Finished custom setup for $name"
    }
}

function Default-Setup([string]$name, [bool]$registerPath = $true) {
    Write-Host "Setting up $name ..."
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
    if (![IO.File]::Exists((App-Exe $name))) {
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
    } else {
        Write-Host "Skipping allready installed $name in $dir"
    }
    Execute-Custom-Setup $name
}

function Setup-NpmPackage($name) {
    $packageName = App-NpmPackage $name
    Write-Host "Setting up npm package $packageName ..."
    $npm = App-Exe Npm
    if (!$npm) { throw "Node Package Manager not found" }
    & $npm install $packageName --global
    Execute-Custom-Setup $name
}

Load-Environment
foreach ($name in $apps) {
    $typ = App-Typ $name
    switch ($typ) {
        "npm" { Setup-NpmPackage $name }
        default { Default-Setup $name }
    }
}
Write-EnvironmentFile

Purge-Dir $tmpDir
