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
    $txt += "SET BENCH_APPS=%BENCH_HOME%\$(Get-ConfigValue LibDir)$nl"
    if (Get-ConfigValue OverrideHome) {
        [string]$h = $Script:homeDir
        $homeDrive = $h.Substring(0, $h.IndexOf("\"))
        $homePath = $h.Substring($h.IndexOf("\") + 1)
        if ($h.StartsWith($Script:rootDir, [StringComparison]::InvariantCultureIgnoreCase)) {
            $relPath = $h.Substring($Script:rootDir.Length + 1).Trim('\')
            $txt += "SET HOMEDRIVE=%~d0$nl"
            $txt += "SET HOMEPATH=%BENCH_HOME%\$relPath$nl"
            $txt += "SET USERPROFILE=%BENCH_HOME%\$relPath$nl"
            $txt += "SET HOME=%BENCH_HOME%\$relPath$nl"
        } else {
            $txt += "SET HOMEDRIVE=$homeDrive$nl"
            $txt += "SET HOMEPATH=$homePath$nl"
            $txt += "SET USERPROFILE=$h$nl"
            $txt += "SET HOME=$h$nl"
        }
        if ($Script:appDataDir.StartsWith($h, [StringComparison]::InvariantCultureIgnoreCase)) {
            $relPath = $Script:appDataDir.Substring($h.Length + 1).Trim('\')
            $txt += "SET APPDATA=%USERPROFILE%\$relPath$nl"
        } else {
            $txt += "SET APPDATA=${Script:appDataDir}$nl"
        }
        if ($Script:localAppDataDir.StartsWith($h, [StringComparison]::InvariantCultureIgnoreCase)) {
            $relPath = $Script:localAppDataDir.Substring($h.Length + 1).Trim('\')
            $txt += "SET LOCALAPPDATA=%USERPROFILE%\$relPath$nl"
        } else {
            $txt += "SET LOCALAPPDATA=${Script:localAppDataDir}$nl"
        }
    }
    if (Get-ConfigValue OverrideTemp) {
        [string]$tmp = $Script:tempDir
        if ($tmp.StartsWith($Script:rootDir, [System.StringComparison]::InvariantCultureIgnoreCase)) {
            $relPath = $tmp.Substring($Script:rootDir.Length + 1).Trim('\')
            $txt += "SET TEMP=%BENCH_HOME%\$relPath$nl"
            $txt += "SET TMP=%BENCH_HOME%\$relPath$nl"
        } else {
            $txt += "SET TEMP=${Script:tempDir}$nl"
            $txt += "SET TMP=${Script:tmpDir}$nl"
        }
    }
    $txt += "SET L=%BENCH_HOME%\$(Get-ConfigValue LibDir)$nl"
    $benchPath = ""
    foreach ($path in $Script:paths) {
        $benchPath = "%L%$($path.Substring(${Script:libDir}.Length));$benchPath"
    }
    $benchPath = $benchPath.TrimEnd(';')
    $txt += "SET BENCH_PATH=%BENCH_AUTO%;$benchPath$nl"
    if (Get-ConfigValue IgnoreSystemPath) {
        $txt += "SET PATH=%BENCH_PATH%;%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0$nl"
    } else {
        $txt += "SET PATH=%BENCH_PATH%;%PATH%$nl"
    }
    
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
    $txt += "GOTO:EOF$nl$nl"
    $txt += ":SET_BENCH_HOME${nl}SET BENCH_HOME=%~dpfn1${nl}GOTO:EOF$nl"

    $txt | Out-File -Encoding oem -FilePath $envFile
    Debug "Written environment file to $envFile"
}
