$paths = @()
$additionalEnvVars = @{}
$tempDir = Safe-Dir (Get-ConfigPathValue TempDir)
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
    Debug "Registering Paths for $name ..."
    if (App-Register $name) {
        $paths = App-Paths $name
        foreach ($p in $paths) {
            Register-Path $p
        }
    }
}

function Register-AppEnvironment([string]$name) {
    Debug "Registering Environment Variables for $name ..."
    $dict = App-Environment $name
    foreach ($k in $dict.Keys) {
        Debug "Registered Environment Variable: $k = $($dict[$k])"
        $Script:additionalEnvVars[$k] = $dict[$k]
    }
}

function Load-Environment() {
    if (Get-ConfigValue UseProxy) {
        $env:HTTP_PROXY = Get-ConfigValue HttpProxy
        $env:HTTPS_PROXY = Get-ConfigValue HttpsProxy
    }
    if (Get-ConfigValue OverrideHome) {
        [string]$h = $Script:homeDir
        $homeDrive = $h.Substring(0, $h.IndexOf("\"))
        $homePath = $h.Substring($h.IndexOf("\"))
        $env:USERPROFILE = $h
        $env:HOME = $h
        $env:HOMEDRIVE = $homeDrive
        $env:HOMEPATH = $homePath
        $env:APPDATA = $Script:appDataDir
        $env:LOCALAPPDATA = $Script:localAppDataDir
    }
    if (Get-ConfigValue OverrideTemp) {
        $env:TEMP = $Script:tempDir
        $env:TMP = $Script:tempDir
    }
    foreach ($k in $Script:additionalEnvVars.Keys) {
        Set-Item "env:$k" $Script:additionalEnvVars[$k]
    }
}

function Load-AppEnvironment([string]$name) {
    $dict = App-Environment $name
    foreach ($k in $dict.Keys) {
        Debug "Load Environment Variable: $k = $($dict[$k])"
        Set-Item "env:$k" $dict[$k]
    }
}

function Update-EnvironmentPath() {
    if (Get-ConfigValue IgnoreSystemPath) {
        $env:PATH = "$env:SystemRoot;$env:SystemRoot\System32;$env:SystemRoot\System32\WindowsPowerShell\v1.0"
    } else {
        $env:PATH = $Script:pathBackup
    }
    $benchPath = ""
    foreach ($path in $Script:paths) {
        $benchPath = "$path;$benchPath"
    }
    $env:PATH = "$benchPath;$env:PATH"
}

function Execute-AppEnvironmentSetup([string]$name) {
    $scriptFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).env.ps1"
    Debug "Searching for environment script apps\$($name.ToLowerInvariant()).env.ps1"
    if (Test-Path $scriptFile) {
        Debug "Running custom environment script for $name"
        . $scriptFile
    }
}

function Write-EnvironmentFile() {

    function pivotDir([string]$path) {
        if (!$path.EndsWith('\')) {
            $path += '\'
        }
        return $path
    }
    function pivotDrive([string]$path) {
        return pivotDir $path.Substring(0, $path.IndexOf('\'))
    }
    function isRelativeTo([string]$path, [string]$base) {
        return $path.StartsWith($base, [StringComparison]::InvariantCultureIgnoreCase)
    }
    function relPath([string]$path, [switch]$noHome)
    {
        $bases = @()
        $bases += @{
            Dir = $Script:libDir
            PathVar = "L"
        }
        if (!$noHome) {
            $bases += @{
                Dir = $Script:homeDir
                PathVar = "HOME"
                DriveVar = "HOMEDRIVE"
            }
        }
        $bases += @{
            Dir = $Script:rootDir
            PathVar = "BENCH_HOME"
            DriveVar = "BENCH_DRIVE"
        }
        foreach ($b in $bases) {
            $d = pivotDir $b.Dir
            if (isRelativeTo $path $d) {
                $relPath = $path.Substring($d.Length).Trim('\')
                return "%$($b.PathVar)%\$relPath"
            }
        }
        foreach ($b in $bases) {
            if (!$b.DriveVar) { continue }
            $d = pivotDrive $b.Dir
            if (isRelativeTo $path $d) {
                $relPath = $path.Substring($d.Length).Trim('\')
                return "%$($b.DriveVar)%\$relPath"
            }
        }
        return $path
    }

    $envFile = "$Script:rootDir\auto\env.cmd"
    $nl = [Environment]::NewLine
    $txt = "@ECHO OFF$nl"
    $txt += "REM **** MD Bench Environment Setup ****$nl$nl"
    if (Get-ConfigValue UseProxy) {
        $txt += "SET HTTP_PROXY=$(Get-ConfigValue HttpProxy)$nl"
        $txt += "SET HTTPS_PROXY=$(Get-ConfigValue HttpsProxy)$nl"
    }
    if (Get-ConfigValue OverrideHome) {
        if (Get-ConfigValue UserName) {
            $txt += "SET USERNAME=$(Get-ConfigValue UserName)$nl"
        }
        if (Get-ConfigValue UserEmail) {
            $txt += "SET USEREMAIL=$(Get-ConfigValue UserEmail)$nl"
        }
    }
    $txt += "SET BENCH_AUTO=%~dp0$nl"
    $txt += "CALL :SET_BENCH_HOME `"%BENCH_AUTO%..`"$nl"
    $txt += "SET /P BENCH_VERSION=<`"%BENCH_HOME%\res\version.txt`"$nl"
    $txt += "CALL :SET_BENCH_DRIVE `"%BENCH_AUTO%`"$nl"
    $txt += "SET BENCH_APPS=%BENCH_HOME%\$(Get-ConfigValue LibDir)$nl"
    $txt += "SET L=%BENCH_APPS%$nl"
    if (Get-ConfigValue OverrideHome) {
        $txt += "SET HOME=$(relPath $Script:homeDir -NoHome)$nl"
        $txt += "CALL :SET_HOME_PATH `"%HOME%`"$nl"
        $txt += "CALL :SET_HOME_DRIVE `"%HOME%`"$nl"
        $txt += "SET USERPROFILE=%HOME%$nl"
        $txt += "SET APPDATA=$(relPath $Script:appDataDir)$nl"
        $txt += "SET LOCALAPPDATA=$(relPath $Script:localAppDataDir)$nl"
    }
    if (Get-ConfigValue OverrideTemp) {
        [string]$tmp = $Script:tempDir
        $txt += "SET TEMP=$(relPath $Script:tempDir)$nl"
        $txt += "SET TMP=%TEMP%$nl"
    }
    $benchPath = ""
    foreach ($path in $Script:paths) {
        $benchPath = "$(relPath $path);$benchPath"
    }
    $benchPath = $benchPath.TrimEnd(';')
    $txt += "SET BENCH_PATH=%BENCH_AUTO%;$benchPath$nl"
    if (Get-ConfigValue IgnoreSystemPath) {
        $txt += "SET PATH=%BENCH_PATH%;%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0$nl"
    } else {
        $txt += "SET PATH=%BENCH_PATH%;%PATH%$nl"
    }

    foreach ($k in $Script:additionalEnvVars.Keys) {
        $txt += "SET $k=$(relPath $Script:additionalEnvVars[$k])$nl"
    }

    $txt += "GOTO:EOF$nl$nl"
    $txt += ":SET_BENCH_HOME${nl}SET BENCH_HOME=%~dpfn1${nl}GOTO:EOF$nl$nl"
    $txt += ":SET_BENCH_DRIVE${nl}SET BENCH_DRIVE=%~d1${nl}GOTO:EOF$nl$nl"
    $txt += ":SET_HOME_PATH${nl}SET HOMEPATH=%pfn1${nl}GOTO:EOF$nl$nl"
    $txt += ":SET_HOME_DRIVE${nl}SET HOMEDRIVE=%d1${nl}GOTO:EOF"

    $txt | Out-File -Encoding oem -FilePath $envFile
    Debug "Written environment file to $envFile"
}
