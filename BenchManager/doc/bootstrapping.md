# Bootstrapping

## `bench-install.bat`

1. download `bench.zip` from GitHub (if not exist already)
2. download `BenchManager.zip` from GitHub  (if not exist already)
3. extract `bench.zip`
4. extract `BenchManager.zip`
5. run `actions\bench-ctl.cmd initialize`

## `bench-ctl.cmd initialize`

1. `auto\init.cmd`
2. `auto\runps.cmd Initialize-Bench`

## `Initialize-Bench.ps1`

1. load `BenchLib.dll` and dependencies
2. `BenchTasks.InitializeSiteConfiguration()`
	* Wizzard: User Identification, Proxy
	* create `bench-site.md`
3. `DefaultBenchManager.AutoSetup()`
	* download required apps
	* install required apps
4. `BenchTasks.InitializeCustomConfiguration()`
	* Wizzard: Existing Config / Default Config
	* create custom config dir
5. Check .NET version and start `BenchDashboard.exe -setup` or `bench-ctl.cmd`
