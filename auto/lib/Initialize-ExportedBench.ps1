$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$sourceDir = Resolve-Path "$myDir\..\.."

[void][System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
$browser = New-Object System.Windows.Forms.FolderBrowserDialog
$browser.SelectedPath = $env:SystemDrive
$browser.ShowNewFolderButton = $true
$browser.Description = "Select the target directory for the cloned Bench environment."
$result = $browser.ShowDialog()
if ($result -ne "OK") { return }

$targetDir = $browser.SelectedPath
if (gci $targetDir)
{
	$answer = [System.Windows.Forms.MessageBox]::Show(
		"The selected directory is not empty.`n`nAre you sure you want to setup Bench in this directory?",
		"Bench Transfer Package",
		"YesNo",
		"Warning"
	)
	if ($answer -ne "Yes") { return }
}
if (!(Test-Path $targetDir)) { $_ = mkdir $targetDir }

$folders = @("auto", "res", "config")
foreach ($f in $folders)
{
	copy "$sourceDir\$f" "$targetDir\$f" -Recurse -Force
}

Start-Process -FilePath "$targetDir\auto\bin\bench.exe" -ArgumentList @("--verbose", "manage", "initialize")
