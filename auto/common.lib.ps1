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
