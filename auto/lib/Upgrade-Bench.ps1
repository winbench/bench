param (
	[switch]$Silent,
	[string]$VersionUrl = "https://github.com/mastersign/bench/raw/master/res/version.txt",
	[string]$BootstrapUrl = "https://github.com/mastersign/bench/raw/master/res/bench-install.bat"
)

$Script:myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
[string]$Script:autoDir = Resolve-Path ([IO.Path]::Combine($myDir, ".."))
[string]$Script:rootDir = Resolve-Path ([IO.Path]::Combine($autoDir, ".."))

$versionFile = [IO.Path]::Combine($rootDir, "res\version.txt")
$bootstrapFile = [IO.Path]::Combine($rootDir, "bench-install.bat")

$wc = New-Object System.Net.WebClient

try 
{
	$installedVersion = (Get-Content $versionFile -TotalCount 1).Trim()
} 
catch
{
	Write-Warning "Checking the installed version failed:"
	Write-Warning $_.Exception.Message
	return
}
try 
{
	$latestVersion = $wc.DownloadString($VersionUrl).Trim()
}
catch
{
	Write-Warning "Checking the latest version failed:"
	Write-Warning $_.Exception.Message
	return
}
if ($installedVersion -eq $latestVersion)
{
	Write-Host "You already have the latest version of Bench installed."
	Write-Host ""
	Write-Host "Latest Bench version: $latestVersion"
	return
}

Write-Host "Downloading latest install script ..."
$wc.DownloadFile($BootstrapUrl, $bootstrapFile)

Write-Host "Upgrading Bench from $installedVersion to $latestVersion ..."
cmd /C $bootstrapFile
