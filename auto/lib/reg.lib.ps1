function Get-AppRegistryFileName([string]$name, [string]$typ, [int]$no = 0) {
    $regBaseDir = Get-ConfigValue AppRegistryBaseDir
    $resDir = Safe-Dir ([IO.Path]::Combine($regBaseDir, $name.ToLowerInvariant()))
    if ($no -gt 0) {
        $noPart = "_" + $no.ToString("00")
    } else {
        $noPart = ""
    }
    return [IO.Path]::Combine($resDir, $typ + $noPart + ".reg")
}

function Get-IsolationLogFile () {
    return [IO.Path]::Combine((Get-ConfigValue TempDir), "registry_isolation.log")
}

function Remove-IsolationLogFile () {
    $log = Get-IsolationLogFile
    if (Test-Path $log) {
        Remove-Item $log
    }
}

function Export-RegistryKey([string]$key, [string]$targetFile) {
    Debug "Export HKCU\$key to '$targetFile' ..."
    $log = Get-IsolationLogFile
    try {
        REG EXPORT "HKCU\$key" "$targetFile" /y /reg:32 > $log
    } catch { }
    $exitCode = $LASTEXITCODE
    if ($exitCode -ne 0) {
        Get-Content $log | Out-Default
        throw "Error during registry export. Exit Code = $exitCode."
    }
    Remove-IsolationLogFile
}

function Delete-RegistryKey([string]$key) {
    Debug "Delete HKCU\$key ..."
    $log = Get-IsolationLogFile
    try {
        REG DELETE "HKCU\$key" /f /reg:32 > $log
    } catch { }
    $exitCode = $LASTEXITCODE
    if ($exitCode -ne 0) {
        Get-Content $log | Out-Default
        throw "Error deleting registry key. Exit Code = $exitCode."
    }
    Remove-IsolationLogFile
}

function Import-RegistryKey([string]$key, [string]$sourceFile) {
    Debug "Import HKCU\$key from '$sourceFile' ..."
    $log = Get-IsolationLogFile
    try {
        # REG writes allways to stderr with IMPORT
        REG IMPORT "$sourceFile" /reg:32 > $log 2>&1
    } catch { }
    $exitCode = $LASTEXITCODE
    if ($exitCode -ne 0) {
        Get-Content $log | Out-Default
        throw "Error during registry import. Exit Code = $exitCode."
    }
    Remove-IsolationLogFile
}

function Suspend-RegistryKey([string]$name, [string]$key, [int]$no) {
    Debug "Suspending registry key '$key' for $name ($no)"
    $backupRegFile = Get-AppRegistryFileName $name "system" $no
    $benchRegFile = Get-AppRegistryFileName $name "bench" $no
    if (Test-Path $backupRegFile) {
        throw "The registry backup file allready exists: $backupRegFile"
    }
    if (Test-Path "HKCU:\$key") {
        Export-RegistryKey $key $backupRegFile
        Delete-RegistryKey $key
    }
    if (Test-Path $benchRegFile) {
        Import-RegistryKey $key $benchRegFile
    }
}

function Suspend-RegistryKeys([string]$name) {
    [array]$keys = App-RegistryKeys $name
    if ($keys) {
        for ($i = 0; $i -lt $keys.Length; $i++) {
            Suspend-RegistryKey $name $keys[$i] $i
        }
    }
}

function Restore-RegistryKey([string]$name, [string]$key, [int]$no) {
    Debug "Restoring registry key '$key' for $name ($no)"
    $benchRegFile = Get-AppRegistryFileName $name "bench" $no
    $backupRegFile = Get-AppRegistryFileName $name "system" $no
    if (Test-Path "HKCU:\$key") {
        Export-RegistryKey $key $benchRegFile
        Delete-RegistryKey $key
    }
    if (Test-Path $backupRegFile) {
        Import-RegistryKey $key $backupRegFile
        Remove-Item $backupRegFile
    }
}

function Restore-RegistryKeys([string]$name) {
    [array]$keys = App-RegistryKeys $name
    if ($keys) {
        for ($i = 0; $i -lt $keys.Length; $i++) {
            Restore-RegistryKey $name $keys[$i] $i
        }
    }
}
