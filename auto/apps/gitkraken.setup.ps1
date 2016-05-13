$configDir = [IO.Path]::Combine((Get-ConfigValue AppDataDir), ".gitkraken")
$configFile = [IO.Path]::Combine($configDir, "config")
$templateFile = [IO.Path]::Combine((Get-ConfigValue AppResourceBaseDir), "gitkraken\config")

if (!(Test-Path $configDir))
{
    $_ = mkdir $configDir
}
if (!(Test-Path $configFile))
{
    copy $templateFile $configFile
}
