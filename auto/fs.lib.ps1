$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\common.lib.ps1"

function Safe-Dir ($dir) {
    if (![IO.Directory]::Exists($dir)) {
        Debug "Creating Directory: $dir"
        $_ = [IO.Directory]::CreateDirectory($dir)
    }
    return $(Resolve-Path $dir).Path
}

function Empty-Dir ($dir) {
    if ([IO.Directory]::Exists($dir)) {
        Debug "Purge Directory $dir"
        Remove-Item -Recurse -Force $dir
    }
    return Safe-Dir $dir
}

function Find-File($dir, $pattern) {
    $files = [IO.Directory]::GetFiles($dir, $pattern)
    if ($files.Count -gt 0) {
        $file = $files[0]
        if ($files.Count -gt 1) {
            Debug "Choose $file from $($files.Count) choices"
        } else {
            Debug "Choose $file"
        }
        return $file
    } else {
        Debug "Found no file for pattern '$pattern'"
        return $null
    }
}
