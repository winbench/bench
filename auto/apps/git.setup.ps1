$git = App-Exe Git
if (!$git) { throw "Git not found" }

$pushDefault = & $git config --global push.default
$user = & $git config --global user.name
$email = & $git config --global user.email

if (!$pushDefault) {
    & $git config --global "push.default" "simple"
}

if (!$user -or !$email) {
    Write-Host "Configuring your GIT identity ..."
    if (!$user) {
        $user = Get-ConfigValue UserName
        if (!$user) {
            $user = Read-Host "User Name"
        } else {
            Write-Host "User Name: $user"
        }
        & $git config --global "user.name" $user
    }
    if (!$email) {
        $email = Get-ConfigValue UserEmail
        if (!$email) {
            $email = Read-Host "Email"
        } else {
            Write-Host "Email: $email"
        }
        & $git config --global "user.email" $email
    }
}

if (Get-ConfigValue UseProxy) {
    & $git config --global "http.proxy" $(Get-ConfigValue HttpProxy)
    & $git config --global "https.proxy" $(Get-ConfigValue HttpsProxy)
    & $git config --global "url.https://.insteadof" "git://"
} else {
    & $git config --global --unset "http.proxy"
    & $git config --global --unset "https.proxy"
    & $git config --global --unset "url.https://.insteadof"
}

if (!(Test-Path "$env:USERPROFILE\.ssh")) {
    $gitDir = App-Dir Git
    $keygen = "$gitDir\usr\bin\ssh-keygen.exe"
    $_ = mkdir "$env:USERPROFILE\.ssh"
    & $keygen -t rsa -b 4096 -f "$env:USERPROFILE\.ssh\id_rsa" -C "$user <$email>"
    notepad "$env:USERPROFILE\.ssh\id_rsa.pub"
}

if (!(Test-Path "$Script:rootDir\.git")) {
    $repo = Get-ConfigValue BenchRepository
    cd $Script:rootDir
    & $git init
    & $git remote add origin $repo
    & $git fetch
    & $git reset --mixed origin/master
    & $git branch --set-upstream-to=origin/master master
}