$lein = App-Exe Leiningen
$leinResourceDir = "$(Get-ConfigPathValue AppResourceBaseDir)\leiningen"
$leinProfilesTemplate = "$leinResourceDir\profiles.clj"
$leinProfiles = "$(Get-ConfigPathValue HomeDir)\.lein\profiles.clj"

Debug "LEIN_JAR: $env:LEIN_JAR"

if (!(Test-Path $env:LEIN_JAR)) {
    & $lein self-install
}

if (!(Test-Path $leinProfiles)) {
    copy $leinProfilesTemplate $leinProfiles
}
