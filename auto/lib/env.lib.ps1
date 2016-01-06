$paths = @()
$tempDir = Empty-Dir (Get-ConfigPathValue TempDir)
$downloadDir = Safe-Dir (Get-ConfigPathValue DownloadDir)
$libDir = Safe-Dir (Get-ConfigPathValue LibDir)
$homeDir = Safe-Dir (Get-ConfigPathValue HomeDir)
$appDataDir = Safe-Dir (Get-ConfigPathValue AppDataDir)
$localAppDataDir = Safe-Dir (Get-ConfigPathValue LocalAppDataDir)
$desktopDir = Safe-Dir "$homeDir\Desktop"
$documentsDir = Safe-Dir "$homeDir\Documents"

function Register-Path([string]$path) {
    if (!($Script:paths -contains $path)) {
        $Script:paths += $path
        Debug "Registered Path: $path"
    }
}

function Register-AppPaths([string]$name) {
    if (App-Register $name) {
        $paths = App-Paths $name
        foreach ($p in $paths) {
            Register-Path $p
        }
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
    if (Get-ConfigValue UserName) {
        $txt += "SET USERNAME=$(Get-ConfigValue UserName)$nl"
    }
    if (Get-ConfigValue UserEmail) {
        $txt += "SET USEREMAIL=$(Get-ConfigValue UserEmail)$nl"
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
