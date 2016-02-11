param (
    $name,
    $executable,
    [switch]$debug
)
$oldTitle = $Host.UI.RawUI.WindowTitle
$Host.UI.RawUI.WindowTitle = "Bench - $([IO.Path]::GetFileNameWithoutExtension($executable))"

$Script:scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:scriptsLib\bench.lib.ps1"

trap {
    Write-TrapError $_
    Pause
    exit 1
}
Set-Debugging $debug

#
# Pre-Execution Phase
#

$customPreFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).pre-run.ps1"
if (Test-Path $customPreFile) {
    Debug "Executing custom pre-run script ..."
    . $customPreFile
}

#
# Execution Phase
#

try {

    if ($args -and $args.Length -gt 0) {
        Start-Process -FilePath $executable -Wait -ArgumentList $args
    } else {
        Start-Process -FilePath $executable -Wait
    }

} catch {
    Write-Warning ("An error occured during execution of adorned executable: " `
      + $_.Exception.Message)
    Write-Warning "Excutable = $executable"
    Write-Warning "Arguments = $([string]::Join(' ', $args))"
    Write-Warning "Exit Code = $LASTEXITCODE"
}

#
# Post-Execution Phase
#

$customPostFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).post-run.ps1"
if (Test-Path $customPostFile) {
    Debug "Executing custom post-run script ..."
    . $customPostFile
}

$Host.UI.RawUI.WindowTitle = $oldTitle
