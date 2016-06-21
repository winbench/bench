$lein = App-Exe Leiningen
$leinResourceDir = "$(Get-ConfigValue AppResourceBaseDir)\leiningen"
$leinProfilesTemplate = "$leinResourceDir\profiles.clj"
$leinProfilesDir = "$(Get-ConfigValue HomeDir)\.lein"
$leinProfiles = [IO.Path]::Combine($leinProfilesDir, "profiles.clj")

$leinEnv = Get-AppConfigValue Leiningen Environment
$leinJar = $leinEnv["LEIN_JAR"]

if (!(Test-Path $leinJar))
{
    $env:LEIN_JAR = $leinJar
    Write-Host "Installing Leiningen to: $leinJar"
    Pause
    & $lein self-install
}

if (!(Test-Path $leinProfilesDir))
{
    $_ = mkdir $leinProfilesDir
}
if (!(Test-Path $leinProfiles))
{
    copy $leinProfilesTemplate $leinProfiles
}
