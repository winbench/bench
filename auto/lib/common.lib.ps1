[string]$Script:scriptsLib = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
[string]$Script:rootDir = Resolve-Path "$scriptsLib\..\.."

function Set-Debugging ($enabled) {
    if ($enabled) {
        $global:DebugPreference = "Continue"
    } else {
        $global:DebugPreference = "SilentlyContinue"
    }
}

function Set-StopOnError ($enabled) {
    $old = $global:ErrorActionPreference -eq "Stop"
    if ($enabled) {
        $global:ErrorActionPreference = "Stop"
    } else {
        $global:ErrorActionPreference = "Continue"
    }
    return $old
}

function Write-TrapError($err) {
    Write-Warning "An error occured during execution: $($err.Exception.Message)"
    $exception = "Exception: [$($err.Exception.GetType())]`n"
    $exception += "    " + $err.ScriptStackTrace.ToString().Replace("`n", "`n    ")
    Write-Debug $exception
}

function Debug($msg) {
    Write-Debug $msg
}

function Pause($msg = "Press any key to exit ...") {
    [Console]::WriteLine()
    [Console]::Write($msg)
    [Console]::ReadKey($true) | Out-Null
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
    if ($args) {
        $argText = [string]::Join(" ", ($args | % { Safe-Argument $_ }))
        Debug "Running $path $argText"
        CMD /C "START $path $argText"
    } else {
        Debug "Running $path"
        CMD /C "START $path"
    }
}

function Exit-OnError($exitCode = $LastExitCode) {
    if ($exitCode -ne 0) {
        Pause
        exit $exitCode
    }
}
