function App-Typ([string]$name) { 
    return Get-AppConfigValue $name Typ "default"
}

function App-Version([string]$name) {
    return Get-AppConfigValue $name Version
}

function App-Dependencies([string]$name) {
    return Get-AppConfigListValue $name Dependencies
}

function App-Url([string]$name) {
    return Get-AppConfigValue $name Url
}

function App-DownloadHeaders([string]$name) {
    $list = Get-AppConfigListValue $name DownloadHeaders
    $dict = @{}
    foreach ($e in $list) {
        $kvp = $e.Split(":", 2)
        $dict[$kvp[0].Trim()] = $kvp[1].Trim()
    }
    return $dict
}

function App-DownloadCookies([string]$name) {
    $cookies = Get-AppConfigListValue $name DownloadCookies
    $result = @()
    foreach ($v in $cookies) {
        $c = New-Object System.Net.Cookie
        $v = $v.Split(":", 2)
        $domain = $v[0].Trim()
        $kvp = $v[1].Trim()
        $c.Domain = $domain
        $v = $kvp.Split("=", 2)
        $c.Name = $v[0].Trim()
        $c.Value = $v[1].Trim()
        $result += $c
    }
    return $result
}

function App-ResourceFile([string]$name) {
    return Get-AppConfigValue $name AppFile
}

function App-ResourceArchive([string]$name) {
    return Get-AppConfigValue $name AppArchive
}

function App-ResourceArchiveTyp([string]$name) {
    return Get-AppConfigValue $name AppArchiveTyp "auto"
}

function App-ResourceArchiveSubDir([string]$name) {
    return Get-AppConfigValue $name AppArchiveSubDir
}

function App-Force([string]$name) {
    return [bool](Get-AppConfigValue $name ForceInstall $false)
}

function App-NpmPackage([string]$name) {
    return Get-AppConfigValue $name NpmPackage $name.ToLowerInvariant()
}

function App-PyPiPackage([string]$name) {
    return Get-AppConfigValue $name PyPiPackage $name.ToLowerInvariant()
}

function App-PythonVersions([string]$name) {
    return Get-AppConfigListValue $name PythonVersions @("2", "3")
}

function App-Dir([string]$name) {
    switch (App-Typ $name) {
        "node-package" {
            return App-Dir NpmBootstrap
        }
        "python-package" {
            # return null because of the ambiguity between Python versions
            return $null
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
        "python-package" {
            return $null
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
            return @(App-Path NpmBootstrap)
        }
        "python-package" {
            return $null
        }
        default {
            $paths = @()
            $appDir = App-Dir $name
            $cfgPaths = Get-AppConfigListValue $name Path
            if ($cfgPaths -and $cfgPaths.Count -gt 0) {
                foreach ($p in $cfgPaths) {
                    $paths += [IO.Path]::Combine($appDir, $p)
                }
            } else {
                $paths += $appDir
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

function App-Environment([string]$name) {
    $l = Get-AppConfigListValue $name Environment
    $dict = @{}
    foreach ($e in $l) {
        $kvp = $e.Split("=", 2)
        $name = $kvp[0].Trim()
        $value = $kvp[1].Trim()
        $dict[$name] = $value
    }
    return $dict
}

function App-Launcher([string]$name) {
    return Get-AppConfigValue $name Launcher $null
}

function App-LauncherExecutable([string]$name) {
    return Get-AppConfigValue $name LauncherExecutable (App-Exe $name)
}

function App-LauncherArguments([string]$name) {
    return Get-AppConfigListValue $name LauncherArguments @('%*')
}

function App-LauncherIcon([string]$name) {
    return Get-AppConfigValue $name LauncherIcon (App-LauncherExecutable $name)
}

function Check-DefaultApp([string]$name) {
    Debug "Checking app ${name}"
    $exe = App-Exe $name
    return $exe -ne $null
}

function Check-NpmPackage([string]$name) {
    $packageDir = [IO.Path]::Combine((App-Dir Npm), "node_modules", (App-NpmPackage $name))
    Debug "Checking NPM package ${name}: $packageDir"
    return Test-Path $packageDir -PathType Container
}

function Check-PyPiPackageForPythonVersion ([string]$name, [string]$pythonVersion) {
    $python = "Python" + $pythonVersion
    $packageDir = [IO.Path]::Combine((App-Dir $python), "lib", "site-packages", (App-PyPiPackage $name))
    Debug "Checking PyPI package $name for Python ${pythonVersion}: $packageDir"
    return Test-Path $packageDir -PathType Container
}

function Check-PyPiPackage([string]$name, [string]$pythonVersion = $null) {
    if ($pythonVersion) {
        return Check-PyPiPackageForPythonVersion $name $pythonVersion
    } else {
        foreach ($version in (App-PythonVersions $name)) {
            if (Check-PyPiPackageForPythonVersion $name $version) {
                return $true
            }
        }
        return $false
    }
}

function Check-App([string]$name) {
    switch (Get-AppConfigValue $name Typ "default") {
        "node-package" {
            return (Check-DefaultApp $name) -or (Check-NpmPackage $name)
        }
        "python-package" {
            return (Check-DefaultApp $name) -or (Check-PyPiPackage $name)
        }
        default {
            return Check-DefaultApp $name
        }
    }
}
