﻿$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\common.lib.ps1"

$config = @{}

function Set-ConfigValue($name, $value) {
    # Add-Member -InputObject $Script:cfg -MemberType NoteProperty -Name $name -Value $value
    if ($Script:debug) {
        Write-Host "[DEBUG] Config: $name = $value"
    }
    $Script:config[$name] = $value
}

function Get-ConfigValue($name, $def = $null) {
    if ($Script:config.ContainsKey($name)) {
        return $Script:config[$name]
    } else {
        return $def
    }
}

# Common
Set-ConfigValue DownloadDir "res\download"
Set-ConfigValue LibDir "lib"
Set-ConfigValue HomeDir "home"
Set-ConfigValue AppDataDir "$(Get-ConfigValue HomeDir)\AppData"
Set-ConfigValue LocalAppDataDir "$(Get-ConfigValue HomeDir)\LocalAppData"

# 7Zip
Set-ConfigValue SvZArchive "7za*.zip"
Set-ConfigValue SvZDir "7z"
Set-ConfigValue SvZPath "$(Get-ConfigValue SvZDir)"
Set-ConfigValue SvZExe "$(Get-ConfigValue SvZPath)\7za.exe"

# Less MSIerables
Set-ConfigValue LessMsiArchive "lessmsi-*.zip"
Set-ConfigValue LessMsiDir "lessmsi"
Set-ConfigValue LessMsiExe "$(Get-ConfigValue LessMsiDir)\lessmsi.exe"

# NodeJS
Set-ConfigValue NodeDir "NodeJS"
Set-ConfigValue NodePath "$(Get-ConfigValue NodeDir)"
Set-ConfigValue NodeExe "node.exe"
Set-ConfigValue NpmArchive "npm-*.zip"

# Git
Set-ConfigValue GitArchive "PortableGit-*-32-bit.7z.exe"
Set-ConfigValue GitDir "git"
Set-ConfigValue GitPath "$(Get-ConfigValue GitDir)\bin"
Set-ConfigValue GitExe "$(Get-ConfigValue GitPath)\git.exe"

# Pandoc
Set-ConfigValue PandocArchive "pandoc-*-windows.msi"
Set-ConfigValue PandocDir "pandoc"
Set-ConfigValue PandocPath "$(Get-ConfigValue PandocDir)"
Set-ConfigValue PandocExe "$(Get-ConfigValue PandocPath)\pandoc.exe"

# GraphViz
Set-ConfigValue GraphVizArchive "graphviz-*.zip"
Set-ConfigValue GraphVizDir "graphviz"
Set-ConfigValue GraphVizPath "$(Get-ConfigValue GraphVizDir)\release\bin"
Set-ConfigValue GraphVizExe "$(Get-ConfigValue GraphVizPath)\dot.exe"

# Inkscape
Set-ConfigValue InkscapeArchive "Inkscape-*-win32.7z"
Set-ConfigValue InkscapeDir "inkscape"
Set-ConfigValue InkscapePath "$(Get-ConfigValue InkscapeDir)"
Set-ConfigValue InkscapeExe "$(Get-ConfigValue InkscapePath)\inkscape.exe"

# MikTeX
Set-ConfigValue MikTeXArchive "miktex-portable-*.exe"
Set-ConfigValue MikTeXDir "miktex"
Set-ConfigValue MikTeXPath "$(Get-ConfigValue MikTeXDir)\bin"
