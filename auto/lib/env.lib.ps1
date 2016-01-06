$paths = @()
$additionalEnvVars = @{}
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

function Register-AppEnvironment([string]$name) {
    $dict = App-Environment $name
    foreach ($k in $dict.Keys) {
        Debug "Registered Environment Variable: $k = $($dict[$k])"
        $Script:additionalEnvVars[$k] = $dict[$k]
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
    foreach ($k in $Script:additionalEnvVars.Keys) {
        Set-Item "env:$k" $Script:additionalEnvVars[$k]
    }
}

function Update-EnvironmentPath() {
    $env:PATH = "$env:SystemRoot;$env:SystemRoot\System32;$env:SystemRoot\System32\WindowsPowerShell\v1.0"
    $benchPath = ""
    foreach ($path in $Script:paths) {
        $benchPath = "$path;$benchPath"
    }
    $env:Path = "$benchPath;$env:Path"
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
    $benchPath = ""
    foreach ($path in $Script:paths) {
        $benchPath = "%L%$($path.Substring(${Script:libDir}.Length));$benchPath"
    }
    $benchPath = $benchPath.TrimEnd(';')
    $txt += "SET BENCH_PATH=%BENCH_AUTO%;$benchPath$nl"
    $txt += "SET PATH=%BENCH_PATH%;%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0$nl"
    
    foreach ($k in $Script:additionalEnvVars.Keys) {
        $v = $Script:additionalEnvVars[$k]
        if ($v.StartsWith($Script:libDir, [StringComparison]::InvariantCultureIgnoreCase)) {
            $v = "%BENCH_APPS%" + $v.Substring($Script:libDir.Length)
        }
        if ($v.StartsWith($Script:homeDir, [StringComparison]::InvariantCultureIgnoreCase)) {
            $v = "%USERPROFILE%" + $v.Substring($Script:homeDir.Length)
        }
        if ($v.StartsWith($Script:rootDir, [StringComparison]::InvariantCultureIgnoreCase)) {
            $v = "%BENCH_HOME%" + $v.Substring($Script:rootDir.Length)
        }
        $txt += "SET $k=$v$nl"
    }
    $txt | Out-File -Encoding oem -FilePath $envFile
    Debug "Written environment file to $envFile"
}
