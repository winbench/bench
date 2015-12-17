$scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$Script:rootDir = Resolve-Path "$scriptsLib\..\.."

function Set-Debugging ($enabled) {
    if ($enabled) {
        $Script:DebugPreference = "Continue"
    } else {
        $Script:DebugPreference = "SilentlyContinue"
    }
}

function Set-StopOnError ($enabled) {
    $old = $Script:ErrorActionPreference -eq "Stop"
    if ($enabled) {
        $Script:ErrorActionPreference = "Stop"
    } else {
        $Script:ErrorActionPreference = "Continue"
    }
    return $old
}

function Debug($msg) {
    Write-Debug $msg
}

function Run-Script($name) {
    & "$scriptsLib\$name.ps1" @args
}

function Safe-Argument([string]$txt) {
    $arg = $txt.Trim("`"", "'")
    if ($arg -match "\s") {
        return "`"$arg`""
    } else {
        return $arg
    }
}

function Run-Detached($path) {
    $path = Safe-Argument $path
    $argText = [string]::Join(" ", ($args | % { Safe-Argument $_ }))
    CMD /C "START $path $argText"
}
