param ([switch]$debug)

$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$scriptsLib\bench.lib.ps1"

trap { Write-TrapError $_ }
Set-Debugging $debug

cd $Script:rootDir

$git = App-Exe Git
if ($git) {
    Debug "Found GIT executable: $git"
    if (Test-Path ".git") {
        Debug "Pulling from Bench repository"
        & $git reset --hard
        & $git pull
    } else {
        Debug "Bench root directory is not a GIT working copy"
    }
} else {
    Write-Warning "GIT executable not found"
}
