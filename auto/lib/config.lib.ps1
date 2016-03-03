﻿$Script:myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$Script:myDir\common.lib.ps1"

[string]$Script:autoDir = Resolve-Path ([IO.Path]::Combine($myDir, ".."))
[string]$Script:rootDir = Resolve-Path ([IO.Path]::Combine($autoDir, ".."))
$Script:pathBackup = $env:PATH

$_ = Set-StopOnError $True

$Script:config = @{}
$Script:definedApps = New-Object 'System.Collections.Generic.List`1[System.String]'
$Script:activatedApps = New-Object 'System.Collections.Generic.List`1[System.String]'
$Script:deactivatedApps = New-Object 'System.Collections.Generic.List`1[System.String]'
$Script:apps = New-Object 'System.Collections.Generic.List`1[System.String]'
$Script:valueExpandPattern = [regex]'\$(?<var>[^\$]+)\$'

function Expand-Placeholder([string]$placeholder) {
    $kvp = $placeholder.Split(":", 2)
    if ($kvp.Count -eq 1) {
        if ($Script:pathConfigValues -contains $placeholder) {
            return Get-ConfigPathValue $placeholder
        } else {
            return Get-ConfigValue $placeholder
        }
    } else {
        $app = $kvp[0].Trim()
        $var = $kvp[1].Trim()
        switch ($var) {
            "Dir" { return App-Dir $app }
            "Path" { return App-Path $app }
            "Exe" { return App-Exe $app }
            default { return Get-AppConfigValue $app $var }
        }
    }
}

function Expand-Value($value) {
    if ($value -is [string]) {
        if ($value -ieq "true") {
            return $true
        } elseif ($value -ieq "false") {
            return $false
        }
		$value = $Script:valueExpandPattern.Replace($value, [Text.RegularExpressions.MatchEvaluator]{
            param ($m)
            return Expand-Placeholder $m.Groups["var"].Value
        })
    }
    return $value
}

function Set-ConfigValue([string]$name, $value) {
    if ($Script:debug) {
        Debug "Config: $name = $value"
    }
    $Script:config[$name] = $value
}

function Get-ConfigValue([string]$name, $def = $null) {
    if ($Script:config.ContainsKey($name)) {
        $value = $Script:config[$name]
        if ($value -is [array]) {
            $value = [array]($value | % { Expand-Value $_ })
        } else {
            $value = Expand-Value $value
        }
        return $value
    } else {
        return $def
    }
}

function Get-ConfigListValue([string]$name, $def = $null) {
    $l = Get-ConfigValue $name $def
    if ($l -is [string]) {
        $l = @($l)
    }
    return [array]$l
}

function Get-ConfigPathValue([string]$name, [string]$def = $null) {
    $path = Get-ConfigValue $name $def
    if ($path -and ![IO.Path]::IsPathRooted($path)) {
        return [IO.Path]::Combine($Script:rootDir, $path)
    } else {
        return $path
    }
}

function Get-AppConfigPropertyName([string]$app, [string]$name) {
    return "App.$app.$name"
}

function Set-AppConfigValue([string]$app, [string]$name, $value) {
    $prop = Get-AppConfigPropertyName $app $name
    Set-ConfigValue $prop $value
}

function Get-AppConfigValue([string]$app, [string]$name, $def = $null) {
    $prop = Get-AppConfigPropertyName $app $name
    return Get-ConfigValue $prop $def
}

function Get-AppConfigListValue([string]$app, [string]$name, $def = $null) {
    $prop = Get-AppConfigPropertyName $app $name
    return Get-ConfigListValue $prop $def
}

function Get-AppConfigPathValue([string]$app, [string]$name, [string]$def = $null) {
    $prop = Get-AppConfigPropertyName $app $name
    $path = Get-ConfigValue $prop $def
    if ($path -and ![IO.Path]::IsPathRooted($path)) {
        $appDir = App-Dir $app
        if ($appDir) {
            return [IO.Path]::Combine($appDir, $path)
        } else {
            return [IO.Path]::Combine($Script:rootDir, $path)
        }
    } else {
        return $path
    }
}

function Add-ToSetList($list, $element) {
    if (!($list -contains $element)) {
        $list.Add($element)
    }
}

function Remove-FromSetList($list, $element) {
    if ($list -contains $element) {
        $list.Remove($element)
    }
}

. "$Script:myDir\appconfig.lib.ps1"

function Register-App([string]$app) {
    Add-ToSetList $Script:definedApps $app
}

function Activate-App([string]$app) {
    Add-ToSetList $Script:activatedApps $app
    Remove-FromSetList $Script:deactivatedApps $app
}

function Deactivate-App([string]$app) {
    Remove-FromSetList $Script:activatedApps $apps
    Add-ToSetList $Script:deactivatedApps $app
}

function Process-AppRegistry($parseActivation = $false) {
    begin {
        $appActivationRequired = [regex]"^## Required"
        $appActivationDefault = [regex]"^## (Default|Groups)"
        $kvpP = [regex]'^\s*\*\s+(?<key>\S+)\s*:\s*(?<value>.*?)\s*$'
        $activation = $null
        $id = $null
        $requiredIds = @()

        function Is-CodeValue([string]$value) {
            return $value.StartsWith("``") -and $value.EndsWith("``")
        }
        function Is-UrlValue([string]$value) {
            return $value.StartsWith("<") -and $value.EndsWith(">")
        }
        function Clean-Value([string]$value) {
            if ((Is-CodeValue $value) -or (Is-UrlValue $value)) {
                $value = $value.Substring(1, $value.Length - 2)
            }
            return $value
        }
        function Parse-Value([string]$value) {
            [array]$elements = $value.Split(",") | % { $_.Trim() } | ? { Is-CodeValue $_ }
            if ($elements.Length -gt 1) {
                return [array]($elements | % { Clean-Value $_ })
            } else {
                return Clean-Value $value
            }
        }
    }
    process {
        if ($parseActivation) {
            if ($_ -match $appActivationRequired) {
                $activation = "required"
            } elseif ($_ -match $appActivationDefault) {
                $activation = "default"
            }
        } else {
            $activation = "default"
        }
        if ($activation) {
            $m = $kvpP.Match($_)
            if ($m.Success) {
                $k = $m.Groups["key"].Value
                $v = $m.Groups["value"].Value
                $v = Parse-Value $v
                if ($k -eq "ID") {
                    $id = $v
                    Register-App $id
                    if ($activation -eq "required") {
                        $requiredIds += $id
                    }
                } elseif ($id) {
                    Set-AppConfigValue $id $k $v
                }
            }
        }
    }
    end {
        foreach ($app in $requiredIds) { Activate-App $app }
    }
}

function Add-Dependency([string]$name, [string]$dep) {
    [array]$deps = App-Dependencies $name
    if ($deps) {
        if (!($deps -contains $dep)) {
            $deps += $dep
            Set-AppConfigValue $name Dependencies $deps
        }
    } else {
        Set-AppConfigValue $name Dependencies @($dep)
    }
}

function Initialize-AutoDependencies() {
    foreach ($name in $Script:definedApps) {
        $appTyp = App-Typ $name
        switch ($appTyp) {
            "node-package" {
                Add-Dependency $name Npm
            }
            "python2-package" {
                Add-Dependency $name Python2
            }
            "python3-package" {
                Add-Dependency $name Python3
            }
        }
    }
}

function Initialize-AdornmentForRegistryIsolation() {
    foreach ($name in $Script:definedApps) {
        $regKeys = App-RegistryKeys $name
        if ($regKeys.Count -gt 0) {
            $appExe = App-Exe $name
            Debug "Automatically adding to adorned executables of ${name}: $appExe"
            [array]$adornedExecutables = App-AdornedExecutables $name
            if ($adornedExecutables) {
                Set-AppConfigValue $name AdornedExecutables $adornedExecutables
            } else {
                Set-AppConfigValue $name AdornedExecutables @($appExe)
            }
        }
    }
}

function Initialize-AdornmentPaths() {
    foreach ($name in $Script:definedApps) {
        [array]$adornedExecutables = Get-AppConfigListValue $name AdornedExecutables
        if ($adornedExecutables) {
            $appPaths = Get-AppConfigListValue $name Path
            $proxyPath = [IO.Path]::Combine(
                (Get-ConfigPathValue AppAdornmentbaseDir),
                $name.ToLowerInvariant())
            if ($appPaths -is [string]) {
                $appPaths = @($appPaths, $proxyPath)
            } elseif ($appPaths -is [array]) {
                $appPaths += $proxyPath
            } else {
                $appPaths = @('.', $proxyPath)
            }
            Set-AppConfigValue $name Path $appPaths
        }
    }
}

function Initialize() {

    $Script:config.Clear()
    $Script:apps.Clear()
    $Script:definedApps.Clear()

    # Common
    Set-ConfigValue VersionFile "res\version.txt"
    Set-ConfigValue UserName $null
    Set-ConfigValue UserEmail $null
    Set-ConfigValue CustomConfigDir "config"
    Set-ConfigValue CustomConfigFile '$CustomConfigDir$\config.ps1'
    Set-ConfigValue CustomConfigTemplate "res\config.template.ps1"
    Set-ConfigValue AppIndex "res\apps.md"
    Set-ConfigValue CustomAppIndex '$CustomConfigDir$\apps.md'
    Set-ConfigValue CustomAppIndexTemplate "res\apps.template.md"
    Set-ConfigValue DownloadDir "res\download"
    Set-ConfigValue AppResourceBaseDir "res\apps"
    Set-ConfigValue AppAdornmentBaseDir "auto\proxies"
    Set-ConfigValue AppRegistryBaseDir '$HomeDir$\registry_isolation'
    Set-ConfigValue ActionDir "actions"
    Set-ConfigValue TempDir "tmp"
    Set-ConfigValue LibDir "lib"
    Set-ConfigValue HomeDir "home"
    Set-ConfigValue AppDataDir '$HomeDir$\AppData\Roaming'
    Set-ConfigValue LocalAppDataDir '$HomeDir$\AppData\Local'
    Set-ConfigValue DownloadProgress $true
    Set-ConfigValue OverrideHome $true
    Set-ConfigValue OverrideTemp $true
    Set-ConfigValue IgnoreSystemPath $true
    Set-ConfigValue ProjectRootDir "projects"
    Set-ConfigValue ProjectArchiveDir "archive"
    Set-ConfigValue ProjectArchiveFormat "zip"
    Set-ConfigValue LauncherDir "launcher"
    Set-ConfigValue UseProxy $false
    Set-ConfigValue HttpProxy $null
    Set-ConfigValue HttpsProxy $null
    Set-ConfigValue DownloadAttempts 3
    Set-ConfigValue BenchRepository "https://github.com/mastersign/bench.git"
    Set-ConfigValue EditorApp "VSCode"

    $Script:pathConfigValues = @(
        "CustomConfigDir",
        "CustomConfigFile",
        "CustomConfigTemplate",
        "AppIndex",
        "CustomAppIndex",
        "CustomAppIndexTemplate"
        "DownloadDir",
        "AppResourceBaseDir",
        "AppAdornmentBaseDir",
        "AppRegistryBaseDir"
        "ActionDir",
        "TempDir",
        "LibDir",
        "HomeDir",
        "AppDataDir",
        "LocalAppDataDir",
        "ProjectRootDir",
        "ProjectArchiveDir"
    )

    $appIndex = Get-ConfigPathValue AppIndex
    Get-Content $appIndex | Process-AppRegistry -parseActivation $true

    #
    # Load Custom Configuration
    #

    $customAppIndex = Get-ConfigPathValue CustomAppIndex
    if (Test-Path $customAppIndex) {
        Get-Content $customAppIndex | Process-AppRegistry -parseActivation $false
    }

    $customConfigFile = Get-ConfigPathValue CustomConfigFile
    if (Test-Path $customConfigFile) {
        . $customConfigFile
    }

    #
    # Auto Configurations
    #

    Initialize-AdornmentForRegistryIsolation
    Initialize-AdornmentPaths
    Initialize-AutoDependencies

    #
    # Resolve Dependencies
    #

    $toActivate = $Script:activatedApps.ToArray()
    function Select-App($app) {
        if ($Script:deactivatedApps -contains $app) {
            return
        }
        Activate-App $app
        $dependencies = Get-AppConfigListValue $app Dependencies
        foreach ($dep in $dependencies) {
            if ($dep -and !($Script:activatedApps -contains $dep)) {
                Select-App $dep
            }
        }
    }
    foreach ($app in $toActivate) {
        Select-App $app
    }
    foreach ($app in $Script:definedApps) {
        if ($Script:activatedApps -contains $app) {
            Add-ToSetList $Script:apps $app
        }
    }

    Debug "Defined Apps: $([string]::Join(", ", $Script:definedApps))"
    Debug "Activated Apps: $([string]::Join(", ", $Script:activatedApps))"
    Debug "Deactivated Apps: $([string]::Join(", ", $Script:deactivatedApps))"
    Debug "Resolved Apps: $([string]::Join(", ", $Script:apps))"

    #
    # Uncostumizable Configurations
    #

    Set-ConfigValue BenchDrive ([IO.Path]::GetPathRoot($Script:rootDir).Substring(0, 2))
    Set-ConfigValue BenchRoot $Script:rootDir
    Set-ConfigValue BenchAuto $Script:autoDir
    Set-ConfigValue BenchScripts $Script:myDir
    Set-ConfigValue Version (Get-Content (Get-ConfigPathValue VersionFile) -Encoding ASCII)
}

Initialize
