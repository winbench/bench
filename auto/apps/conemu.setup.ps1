$conemuResourceDir = "$(Get-ConfigValue AppResourceBaseDir)\conemu"
$customConfigDir = Safe-Dir (Get-ConfigValue CustomConfigDir)

$conemuConfigTemplate = "$conemuResourceDir\ConEmu.xml"
$conemuCustomConfigFile = "$customConfigDir\ConEmu.xml"

if (!(Test-Path $conemuCustomConfigFile)) {
    cp $conemuConfigTemplate $conemuCustomConfigFile
}
