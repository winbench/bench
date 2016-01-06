﻿$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\common.lib.ps1"

$Script:rootDir = Resolve-Path ([IO.Path]::Combine($myDir, "..", ".."))

$_ = Set-StopOnError $True

$Script:config = @{}
$Script:apps = New-Object 'System.Collections.Generic.List`1[System.String]'
$Script:definedApps = New-Object 'System.Collections.Generic.List`1[System.String]'
$Script:valueExpandPattern = [regex]'\$(?<var>[^\$]+)\$'

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

function Expand-Placeholder([string]$placeholder) {
    $kvp = $placeholder.Split(":", 2)
    if ($kvp.Count -eq 1) {
        return Get-ConfigValue $placeholder
    } else {
        $app = $kvp[0].Trim()
        $var = $kvp[1].Trim()
        switch ($var) {
            "Dir" {
                return App-Dir $app
            }
            "Path" {
                return App-Path $app
            }
            "Exe" {
                return App-Exe $app
            }
            default {
                return Get-AppConfigValue $app $var
            }
        }
    }
}

function Expand-Value($value) {
    if ($value -is [string]) {
        $value = $valueExpandPattern.Replace($value, [Text.RegularExpressions.MatchEvaluator]{
            param ($m)
            return Expand-Placeholder $m.Groups["var"].Value
        })
        if ($value -ieq "true") {
            $value = $true
        }
        if ($value -ieq "false") {
            $value = $false
        }
    }
    return $value
}

function Get-ConfigListValue([string]$name, $def = @()) {
    $l = Get-ConfigValue $name $def
    if ($l -is [string]) {
        $l = @($l)
    }
    return [array]$l
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

function Get-AppConfigListValue([string]$app, [string]$name, $def = @()) {
    $prop = Get-AppConfigPropertyName $app $name
    return Get-ConfigListValue $prop $def
}

function Register-App([string]$app) {
    if (!($app -in $Script:definedApps)) {
        $Script:definedApps.Add($app)
    }
}

function Activate-App([string]$app) {
    if (!($app -in $Script:apps)) {
        $Script:apps.Add($app)
    }
}

function Deactivate-App([string]$app) {
    if ($app -in $Script:apps) {
        $_ = $Script:apps.Remove($app)
    }
}

function Get-ConfigPathValue([string]$name) {
    $path = Get-ConfigValue $name
    if ([IO.Path]::IsPathRooted($path)) {
        return $path
    } else {
        return [IO.Path]::Combine($Script:rootDir, $path)
    }
}

function Process-AppRegistry($parseGroups = $false) {
    begin {
        $appGroupRequired = "## Required"
        $appGroupDefault = "## Default"
        $appGroupOptional = "## Optional"
        $kvpP = [regex]'^\s*\*\s+(?<key>\S+)\s*:\s*(?<value>.*?)\s*$'
        $group = $null
        $id = $null
        $requiredIds = @()
        $defaultIds = @()
        
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
        if ($parseGroups) {
            if ($_ -eq $appGroupRequired) {
                $group = "required"
            } elseif ($_ -eq $appGroupDefault) {
                $group = "default"
            } elseif ($_ -eq $appGroupOptional) {
                $group = "optional"
            }
        } else {
            $group = "optional"
        }
        if ($group) {
            $m = $kvpP.Match($_)
            if ($m.Success) {
                $k = $m.Groups["key"].Value
                $v = $m.Groups["value"].Value
                $v = Parse-Value $v
                if ($k -eq "ID") {
                    $id = $v
                    Register-App $id
                    if ($group -eq "required") {
                        $requiredIds += $id
                    }
                    if ($group -eq "default") {
                        $defaultIds += $id
                    }
                } elseif ($id) {
                    Set-AppConfigValue $id $k $v
                }
            }
        }
    }
    end {
        foreach ($app in $requiredIds) { Activate-App $app }
        foreach ($app in $defaultIds) { Activate-App $app }
    }
}

function Initialize() {

    $Script:config.Clear()
    $Script:apps.Clear()
    $Script:definedApps.Clear()
    
    # Common
    Set-ConfigValue Version "0.2.0"
    Set-ConfigValue UserName $null
    Set-ConfigValue UserEmail $null
    Set-ConfigValue CustomConfigFile "config.ps1"
    Set-ConfigValue CustomConfigTemplate "res\config.template.ps1"
    Set-ConfigValue AppIndex "res\apps.md"
    Set-ConfigValue CustomAppIndex "apps.md"
    Set-ConfigValue CustomAppIndexTemplate "res\apps.template.md"
    Set-ConfigValue DownloadDir "res\download"
    Set-ConfigValue AppResourceBaseDir "res\apps"
    Set-ConfigValue TempDir "tmp"
    Set-ConfigValue LibDir "lib"
    Set-ConfigValue HomeDir "home"
    Set-ConfigValue AppDataDir "$(Get-ConfigValue HomeDir)\AppData\Roaming"
    Set-ConfigValue LocalAppDataDir "$(Get-ConfigValue HomeDir)\AppData\Local"
    Set-ConfigValue ProjectRootDir "projects"
    Set-ConfigValue ProjectArchiveDir "archive"
    Set-ConfigValue ProjectArchiveFormat "zip"
    Set-ConfigValue UseProxy $false
    Set-ConfigValue HttpProxy $null
    Set-ConfigValue HttpsProxy $null
    Set-ConfigValue DownloadAttempts 3
    Set-ConfigValue BenchRepository "https://github.com/mastersign/bench.git"
    Set-ConfigValue EditorApp "VSCode"

    $appIndex = Get-ConfigPathValue AppIndex
    Get-Content $appIndex | Process-AppRegistry -parseGroups $true

    #
    # Load custom configuration
    #

    $customAppIndex = Get-ConfigPathValue CustomAppIndex
    if (Test-Path $customAppIndex) {
        Get-Content $customAppIndex | Process-AppRegistry -parseGroups $false
    }

    $customConfigFile = Get-ConfigPathValue CustomConfigFile
    if (Test-Path $customConfigFile) {
        . $customConfigFile
    }

    Set-ConfigValue BenchRoot $Script:rootDir
}

Initialize