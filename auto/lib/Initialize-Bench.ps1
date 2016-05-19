$Script:myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
& "$Script:myDir\Load-ClrLibs.ps1"

[string]$Script:autoDir = Resolve-Path ([IO.Path]::Combine($myDir, ".."))
[string]$Script:rootDir = Resolve-Path ([IO.Path]::Combine($autoDir, ".."))

$cfg = [Mastersign.Bench.BenchTasks]::InitializeSiteConfiguration($rootDir)
if (!$cfg)
{
	Write-Host "Initialization canceled."
	exit 1
}

$wizzardStartAutoSetup = $cfg.GetBooleanValue("WizzardStartAutoSetup", $True)
$mgr = New-Object Mastersign.Bench.DefaultBenchManager ($cfg)
$mgr.Verbose = $True
$cfg = $null

$success = $mgr.SetupRequiredApps()

if (!$success)
{
	Write-Host "Initial app setup failed."
	exit 1
}

$cfg = [Mastersign.Bench.BenchTasks]::InitializeCustomConfiguration($mgr)
if (!$cfg)
{
	Write-Host "Initialization canceled."
	exit 1
}

if ([Mastersign.Bench.BenchTasks]::IsDashboardSupported)
{
	if ($wizzardStartAutoSetup) 
	{
		Start-Process "$autoDir\bin\BenchDashboard.exe" ("-setup")
	}
	else
	{
		Start-Process "$autoDir\bin\BenchDashboard.exe"
	}
}
else
{
	if ($wizzardStartAutoSetup)
	{
		Start-Process "$rootDir\actions\bench-ctl.cmd" ("setup")
	}
	else
	{
		Start-Process "$rootDir\actions\bench-ctl.cmd"
	}
}
