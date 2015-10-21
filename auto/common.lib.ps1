function Debug($msg) {
    if ($Script:debug) {
        Write-Host "[DEBUG] $msg"
    }
}
