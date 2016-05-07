param (
    $Action = "setup",
    [switch]$WithInfo
)
$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\bench.lib.ps1"

$manager = New-Object Mastersign.Bench.DefaultBenchManager ($Script:cfg)
$manager.Verbose = $WithInfo

$success = $False
switch ($Action)
{
    "setup" { $success = $manager.AutoSetup() }
    "update-env" { $success = $manager.UpdateEnvironment() }
    "download" { $success = $manager.DownloadAppResources() }
    "clear-cache" { $success = $manager.DeleteAppResources() }
    "install" { $success = $manager.InstallApps() }
    "reinstall" { $success = $manager.ReinstallApps() }
    "renew" { $success = $manager.UpgradeApps() }
    default { Write-Error "Invalid action: '$action'" }
}

if (!$success)
{
    Write-Warning "Take a look into the last log file in $(Get-ConfigValue LogDir)."
    Write-Error "Action failed."
}
