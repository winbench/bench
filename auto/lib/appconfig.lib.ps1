function App-Typ([string]$name) { 
    return Get-AppConfigValue $name Typ "default"
}

function App-Version([string]$name) {
    return Get-AppConfigValue $name Version
}

function App-Archive([string]$name) {
    return Get-AppConfigValue $name Archive
}

function App-ArchiveSubDir([string]$name) {
    return Get-AppConfigValue $name ArchiveSubDir
}

function App-Download([string]$name) {
    return Get-AppConfigValue $name Download
}

function App-Force([string]$name) {
    return Get-AppConfigValue $name ForceInstall $false
}

function App-NpmPackage([string]$name) {
    return Get-AppConfigValue $name NpmPackage $name.ToLowerInvariant()
}

function App-Dir([string]$name) {
    switch (App-Typ $name) {
        "node-package" {
            return App-Dir NpmBootstrap
        }
        default {
            return [IO.Path]::Combine(
                (Get-ConfigPathValue LibDir),
                (Get-AppConfigValue $name Dir $name.ToLowerInvariant())) 
        }
    }
}

function App-Path([string]$name) {
    switch (App-Typ $name) {
        "node-package" {
            return App-Path NpmBootstrap
        }
        default {
            $appDir = App-Dir $name
            $cfgPath = Get-AppConfigValue $name Path ""
            if ($cfgPath -is [string]) {
                return [IO.Path]::Combine($appDir, $cfgPath)
            } elseif ($cfgPath -is [array]) {
                return [IO.Path]::Combine($appDir, $cfgPath[0])
            }
        }
    }
}

function App-Paths([string]$name) {
    switch (App-Typ $name) {
        "node-package" {
            return App-Paths NpmBootstrap
        }
        default {
            $paths = @()
            $appDir = App-Dir $name
            $cfgPath = Get-AppConfigValue $name Path ""
            if ($cfgPath -is [string]) {
                $paths += [IO.Path]::Combine($appDir, $cfgPath)
            } elseif ($cfgPath -is [array]) {
                foreach ($p in $cfgPath) {
                    $paths += [IO.Path]::Combine($appDir, $p)
                }
            }
            return $paths
        }
    }
}

function App-Exe([string]$name, [bool]$checkExist = $true) {
    $path = [IO.Path]::Combine(
        (App-Path $name),
        (Get-AppConfigValue $name Exe "${name}.exe"))
    if ($checkExist -and ![IO.File]::Exists($path)) {
        Debug "Executable for $name not found: $path"
        return $null
    } else {
        return $path
    }
}

function App-Register([string]$name) {
    return Get-AppConfigValue $name Register $true
}

function Check-DefaultApp([string]$name) {
    Debug "Checking app ${name}"
    $exe = App-Exe $name
    return $exe -ne $null
}

function Check-NpmPackage([string]$name) {
    # $npm = App-Exe Npm
    # if (!$npm) { throw "Node Package Manager not found" }
    # $p = [regex]"^\S+ ([^@\s]*)@[^@\s]+`$"
    # $list = & $npm list --global --depth 0 | ? { $p.IsMatch($_) } | % { $p.Replace($_, "`$1") }
    # $packageName = App-NpmPackage $name
    # return $packageName -in $list
    $packageDir = [IO.Path]::Combine((App-Dir Npm), "node_modules", (App-NpmPackage $name))
    Debug "Checking NPM package ${name}: $packageDir"
    return Test-Path $packageDir -PathType Container
}

function Check-App([string]$name) {
    switch (Get-AppConfigValue $name Typ "default") {
        "node-package" {
            return (Check-DefaultApp $name) -or (Check-NpmPackage $name)
        }
        default {
            return Check-DefaultApp $name
        }
    }
}