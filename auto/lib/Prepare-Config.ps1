param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

$Script:setupStore = New-Object Mastersign.Bench.SetupStore

$_ = [Mastersign.Bench.BenchTasks]::PrepareConfiguration(
	$Script:rootDir,
	$Script:setupStore,
	(New-Object Mastersign.Bench.ConsoleUserInterface)
)

Write-Host "Finished preparing custom configuration"
