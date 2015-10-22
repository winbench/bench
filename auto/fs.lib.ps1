$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\common.lib.ps1"

function Reset-ACLs($dir) {
    Debug "Reseting ACLs for $dir"
    icacls "$dir" /T /C /RESET
}

function Purge-Dir ($dir) {
    if ([IO.Directory]::Exists($dir)) {
        Debug "Purge Directory $dir"
        # Reset-ACLs $dir
        [IO.Directory]::Delete($dir, $True)
    }
}

function Safe-Dir ($dir) {
    if (![IO.Path]::IsPathRooted($dir)) {
        $dir = [IO.Path]::Combine((Get-Location), $dir)
    }
    if ([IO.File]::Exists($dir)) {
        throw "A file exist where a directory supposed to be: $dir"
    }
    if (![IO.Directory]::Exists($dir)) {
        Debug "Creating Directory: $dir"
        $_ = [IO.Directory]::CreateDirectory($dir)
    }
    return $(Resolve-Path $dir).Path
}

function Empty-Dir ($dir) {
    Purge-Dir $dir
    return Safe-Dir $dir
}

function Find-Files($dir, $pattern) {
    if (![IO.Directory]::Exists($dir)) {
        return @()
    }
    return [IO.Directory]::GetFiles($dir, $pattern)
}

function Find-File($dir, $pattern) {
    $files = Find-Files $dir $pattern
    if ($files -is [string]) { $files = @($files) }
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
