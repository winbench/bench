$tempDir = Get-ConfigValue TempDir
$downloadDir = Get-ConfigValue DownloadDir
$libDir = Get-ConfigValue LibDir
$homeDir = Get-ConfigValue HomeDir
$appDataDir = Get-ConfigValue AppDataDir
$localAppDataDir = Get-ConfigValue LocalAppDataDir

$Script:paths = $Script:cfg.Apps.EnvironmentPath
$Script:additionalEnvVars = $Script:cfg.Apps.Environment
$Script:benchEnv = New-Object Mastersign.Bench.BenchEnvironment($Script:cfg)

function Load-Environment() {
    $Script:benchEnv.Load()
}

function Load-AppEnvironment([string]$name) {
    $Script:benchEnv.Load()
}

function Update-EnvironmentPath() {
    $Script:benchEnv.Load()
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
	$env = New-Object Mastersign.Bench.BenchEnvironment($Script:cfg)
	$env.WriteEnvironmentFile()
}
