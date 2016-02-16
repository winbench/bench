if (Test-Path alias:curl) { Remove-Item alias:curl }
if (Test-Path alias:wget) { Remove-Item alias:wget }

$pd = (Get-Host).PrivateData
$pd.DebugForegroundColor = "DarkCyan"
$pd.VerboseForegroundColor = "Gray"
