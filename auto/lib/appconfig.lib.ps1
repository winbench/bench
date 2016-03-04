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
        if (!$e) { continue }
        $kvp = $e.Split(":", 2)
        $dict[$kvp[0].Trim()] = $kvp[1].Trim()
    }
    return $dict
}

function App-DownloadCookies([string]$name) {
    $cookies = Get-AppConfigListValue $name DownloadCookies
    $result = @()
    foreach ($v in $cookies) {
        if (!$v) { continue }
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

function App-Dir([string]$name) {
    switch (App-Typ $name) {
        "node-package" {
            return App-Dir Npm
        }
        "python2-package" {
            return App-Dir Python2
        }
        "python3-package" {
            return App-Dir Python3
        }
        "meta" {
            # no default app directory for meta apps
            $path = Get-AppConfigValue $name Dir
            if ($path -and ![IO.Path]::IsPathRooted($path)) {
                return [IO.Path]::Combine((Get-ConfigPathValue LibDir), $path)
            } else {
                return $path
            }
        }
        default {
            $path = Get-AppConfigValue $name Dir $name.ToLowerInvariant()
            if ($path -and ![IO.Path]::IsPathRooted($path)) {
                return [IO.Path]::Combine((Get-ConfigPathValue LibDir), $path)
            } else {
                return $path
            }
        }
    }
}

function App-Path([string]$name) {
    switch (App-Typ $name) {
        "node-package" {
            return App-Path Npm
        }
        "python2-package" {
            return [IO.Path]::Combine((App-Dir Python2), "Scripts")
        }
        "python3-package" {
            return [IO.Path]::Combine((App-Dir Python3), "Scripts")
        }
        # treat meta apps like default apps, but (App-Dir $name) can be $null
        default {
            $appDir = App-Dir $name
            $path = Get-AppConfigValue $name Path $appDir
            if ($path -is [array]) { $path = $path[0] }
            if ($path -and ![IO.Path]::IsPathRooted($path)) {
                return [IO.Path]::Combine($appDir, $path)
            } else {
                return $path
            }
        }
    }
}

function App-Paths([string]$name) {
    switch (App-Typ $name) {
        "node-package" {
            return @(App-Path Npm)
        }
        "python2-package" {
            return @([IO.Path]::Combine((App-Dir Python2), "Scripts"))
        }
        "python3-package" {
            return @([IO.Path]::Combine((App-Dir Python3), "Scripts"))
        }
        # treat meta apps like default apps, but (App-Dir $name) can be $null
        default {
            $paths = @()
            $appDir = App-Dir $name
            [array]$cfgPaths = Get-AppConfigListValue $name Path
            if ($cfgPaths -and $cfgPaths.Count -gt 0) {
                foreach ($p in $cfgPaths) {
                    if (!$p) { continue }
                    if (![IO.Path]::IsPathRooted($p)) {
                        $paths += [IO.Path]::Combine($appDir, $p)
                    } else {
                        $paths += $p
                    }
                }
            } else {
                $paths += $appDir
            }
            return $paths
        }
    }
}

function App-Exe([string]$name, [bool]$checkExist = $true) {
    $path = Get-AppConfigPathValue $name Exe "${name}.exe"
    if (Test-Path $path) {
        return $path
    } else {
        return $null
    }
}

function App-Register([string]$name) {
    return Get-AppConfigValue $name Register $true
}

function App-Environment([string]$name) {
    $l = Get-AppConfigListValue $name Environment
    $dict = @{}
    foreach ($e in $l) {
        if (!$e) { continue }
        $kvp = $e.Split("=", 2)
        $name = $kvp[0].Trim()
        $value = $kvp[1].Trim()
        $dict[$name] = $value
    }
    return $dict
}

function App-AdornedExecutables([string]$name) {
    $appDir = App-Dir $name
    [array]$exePaths = Get-AppConfigListValue $name AdornedExecutables
    if ($exePaths) {
        return [array]($exePaths | % {
            if (![IO.Path]::IsPathRooted($_)) {
                if ($appDir) {
                    [IO.Path]::Combine($appDir, $_)
                }
            } else {
                $_
            }
        })
    }
}

function App-RegistryKeys([string]$name) {
    return Get-AppConfigListValue $name RegistryKeys
}

function App-Launcher([string]$name) {
    return Get-AppConfigValue $name Launcher $null
}

function App-LauncherExecutable([string]$name) {
    return Get-AppConfigPathValue $name LauncherExecutable (App-Exe $name)
}

function App-LauncherArguments([string]$name) {
    return Get-AppConfigListValue $name LauncherArguments @('%*')
}

function App-LauncherIcon([string]$name) {
    return Get-AppConfigValue $name LauncherIcon (App-LauncherExecutable $name)
}

function App-SetupTestFile([string]$name) {
    return Get-AppConfigPathValue $name SetupTestFile (App-Exe $name)
}

function Check-DefaultApp([string]$name) {
    Debug "Checking app ${name}"
    $file = App-SetupTestFile $name
    return $file -and (Test-Path $file)
}

function Check-NpmPackage([string]$name) {
    $packageDir = [IO.Path]::Combine((App-Dir Npm), "node_modules\$(App-NpmPackage $name)")
    Debug "Checking NPM package ${name}: $packageDir"
    return Test-Path $packageDir -PathType Container
}

function Check-PyPiPackage ([string]$pythonVersion, [string]$name) {
    $python = "Python" + $pythonVersion
    $packageDir = [IO.Path]::Combine((App-Dir $python), "lib\site-packages\$(App-PyPiPackage $name)")
    Debug "Checking PyPI package $name for Python ${pythonVersion}: $packageDir"
    return Test-Path $packageDir -PathType Container
}

function Check-App([string]$name) {
    switch (Get-AppConfigValue $name Typ "default") {
        "node-package" {
            return (Check-DefaultApp $name) -or (Check-NpmPackage $name)
        }
        "python2-package" {
            return (Check-DefaultApp $name) -or (Check-PyPiPackage 2 $name)
        }
        "python3-package" {
            return (Check-DefaultApp $name) -or (Check-PyPiPackage 3 $name)
        }
        default {
            return Check-DefaultApp $name
        }
    }
}
