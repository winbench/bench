param (
    $name,
    $executable
)
$oldTitle = $Host.UI.RawUI.WindowTitle
$Host.UI.RawUI.WindowTitle = "Bench - $([IO.Path]::GetFileNameWithoutExtension($executable))"

$Script:scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:scriptsLib\bench.lib.ps1"
. "$Script:scriptsLib\reg.lib.ps1"

trap {
    Write-TrapError $_
    Pause
    exit 1
}

#
# Pre-Execution Phase
#

Suspend-RegistryKeys $name

$customPreFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).pre-run.ps1"
if (Test-Path $customPreFile) {
    Write-Host "Executing custom pre-run script ..."
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

Restore-RegistryKeys $name

$customPostFile = "$Script:scriptsLib\..\apps\$($name.ToLowerInvariant()).post-run.ps1"
if (Test-Path $customPostFile) {
    Write-Host "Executing custom post-run script ..."
    . $customPostFile
}

$Host.UI.RawUI.WindowTitle = $oldTitle
