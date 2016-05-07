$gitDir = App-Dir Git
$git = App-Exe Git
if (!$git) { throw "Git not found" }

if (Test-Path "$gitDir\post-install.bat")
{
    Write-Host "Renaming Git post-install script to prevent deletion"
    mv "$gitDir\post-install.bat" "$gitDir\post-install.bat.bak"
}

pushd $gitDir
Write-Host "Running post-install script for Git ..."
copy "post-install.bat.bak" "post-install.bat"
cmd /C "post-install.bat" | Out-Null
popd

$autocrlf = & $git config --global core.autocrlf
$pushDefault = & $git config --global push.default
$user = & $git config --global user.name
$email = & $git config --global user.email

if (!$autocrlf)
{
    & $git config --global "core.autocrlf" "true"
}

if (!$pushDefault)
{
    & $git config --global "push.default" "simple"
}

if (Get-ConfigBooleanValue UseProxy)
{
    & $git config --global "http.proxy" $(Get-ConfigValue HttpProxy)
    & $git config --global "https.proxy" $(Get-ConfigValue HttpsProxy)
    & $git config --global "url.https://.insteadof" "git://"
}
else
{
    & $git config --global --unset "http.proxy"
    & $git config --global --unset "https.proxy"
    & $git config --global --unset "url.https://.insteadof"
}

if (!$user -or !$email)
{
    Write-Host "Configuring your GIT identity ..."
    if (!$user)
    {
        $user = Get-ConfigValue UserName
        Write-Host "User Name: $user"
        & $git config --global "user.name" $user
    }
    if (!$email)
    {
        $email = Get-ConfigValue UserEmail
        Write-Host "Email: $email"
        & $git config --global "user.email" $email
    }
}

#if (!(Test-Path "$env:USERPROFILE\.ssh"))
#{
#    $keygen = "$gitDir\usr\bin\ssh-keygen.exe"
#    $_ = mkdir "$env:USERPROFILE\.ssh"
#    @("", "") | & $keygen -t rsa -b 4096 -f "$env:USERPROFILE\.ssh\id_rsa" -C "$user <$email>"
#    $keygenExitCode = $LastExitCode
#    if ($keygenExitCode -ne 0)
#    {
#        del "$env:USERPROFILE\.ssh" -Recurse -Force
#        Write-Host "Failed to create a default SSH key pair with exit code $keygenExitCode"
#        exit 1
#    }
#}

function checkExitCode($exitCode, $action)
{
    if ($exitCode -ne 0)
    {
        if (Test-Path "$Script:rootDir\.git")
        {
            del "$Script:rootDir\.git" -Recurse -Force
        }
        Write-Error "'$action' failed with exit code $exitCode"
        exit 1
    }
}

if (!(Test-Path "$Script:rootDir\.git"))
{
    Write-Host "Converting Bench root into a working copy ..."
    $repo = Get-ConfigValue BenchRepository

    cd $Script:rootDir
    Write-Host "  Initializing Bench root as Git versioned"
    & $git init | Out-Default
    checkExitCode $LastExitCode "git init"

    Write-Host "  Adding remote repository"
    & $git remote add origin $repo | Out-Default
    checkExitCode $LastExitCode "git remote add origin ..."

    Write-Host "  Fetching remote branches"
    & $git fetch | Out-Default
    checkExitCode $LastExitCode "git fetch"

    Write-Host "  Binding local branch to remote branch"
    & $git reset --mixed origin/master | Out-Default
    checkExitCode $LastExitCode "git reset ..."

    Write-Host "  Setup branch tracking"
    & $git branch --set-upstream-to=origin/master master | Out-Default
    checkExitCode $LastExitCode "git branch --set-upstream-to=..."

    Write-Host "Finished converting Bench root into a working copy."
}