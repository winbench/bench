$lein = App-Exe Leiningen
$leinResourceDir = "$(Get-ConfigValue AppResourceBaseDir)\leiningen"
$leinProfilesTemplate = "$leinResourceDir\profiles.clj"
$leinProfilesDir = "$(Get-ConfigValue HomeDir)\.lein"
$leinProfiles = [IO.Path]::Combine($leinProfilesDir, "profiles.clj")

Debug "LEIN_JAR: $env:LEIN_JAR"

if (!(Test-Path $env:LEIN_JAR))
{
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
