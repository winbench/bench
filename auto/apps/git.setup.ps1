$gitDir = App-Dir Git
$git = App-Exe Git
if (!$git) { throw "Git not found" }

if (Test-Path "$gitDir\post-install.bat")
{
    Write-Host "Renaming Git post-install script to prevent deletion"
    mv "$gitDir\post-install.bat" "$gitDir\git-post-install.bat"
}

pushd $gitDir
Write-Host "Running post-install script for Git ..."
cmd /C "git-post-install.bat" | Out-Null
popd

$autocrlf = git config --global core.autocrlf
$pushDefault = git config --global push.default
$user = git config --global user.name
$email = git config --global user.email

if (!$autocrlf)
{
    git config --global "core.autocrlf" "true"
}

if (!$pushDefault)
{
    git config --global "push.default" "simple"
}

if (!$user -or !$email)
{
    Write-Host "Configuring your GIT identity ..."
    if (!$user)
    {
        $user = Get-ConfigValue UserName
        Write-Host "User Name: $user"
        git config --global "user.name" $user
    }
    if (!$email)
    {
        $email = Get-ConfigValue UserEmail
        Write-Host "Email: $email"
        git config --global "user.email" $email
    }
}
