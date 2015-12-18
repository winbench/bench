$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
. "$myDir\common.lib.ps1"

$Script:rootDir = Resolve-Path ([IO.Path]::Combine($myDir, "..", ".."))

$_ = Set-StopOnError $True

$Script:config = @{}

function Set-ConfigValue([string]$name, $value) {
    if ($Script:debug) {
        Debug "Config: $name = $value"
    }
    $Script:config[$name] = $value
}

function Get-ConfigValue([string]$name, $def = $null) {
    if ($Script:config.ContainsKey($name)) {
        return $Script:config[$name]
    } else {
        return $def
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

function Get-ConfigDir([string]$name) {
    $path = Get-ConfigValue $name
    if ([IO.Path]::IsPathRooted($path)) {
        return $path
    } else {
        return [IO.Path]::Combine($Script:rootDir, $path)
    }
}

# Common
Set-ConfigValue Version "0.1.0"
Set-ConfigValue DownloadDir "res\download"
Set-ConfigValue ResFile "res\resources.md"
Set-ConfigValue TempDir "tmp"
Set-ConfigValue LibDir "lib"
Set-ConfigValue HomeDir "home"
Set-ConfigValue AppDataDir "$(Get-ConfigValue HomeDir)\AppData"
Set-ConfigValue LocalAppDataDir "$(Get-ConfigValue HomeDir)\LocalAppData"
Set-ConfigValue ProjectRootDir "projects"
Set-ConfigValue ProjectArchiveDir "archive"
Set-ConfigValue ProjectArchiveFormat "zip"
Set-ConfigValue UseProxy $false
Set-ConfigValue HttpProxy $null
Set-ConfigValue HttpsProxy $null
Set-ConfigValue DownloadAttempts 3

Set-ConfigValue Apps @(
    "SvZ",
    "LessMsi",
    "Git",
    "Node",
    "NpmBootstrap",
    "Npm",
    "Gulp",
    "Yeoman",
    "MdProcGen",
    "JSHint",
    "VSCode",
    "Pandoc",
    "GraphViz",
    "Inkscape",
    "MikTeX"
)

# Template for command line tool
# Set-AppConfigValue XYZ Typ "default" # (optional)
# Set-AppConfigValue XYZ Url "http://xyz.org/latest"
# Set-AppConfigValue XYZ Download "xyz.exe" # only for executable downloads
# Set-AppConfigValue XYZ Archive "xyz-*.zip" # for archive downloads (zip, msi, ...)
# Set-AppConfigValue XYZ ArchiveSubDir "abc" # (optional)
# Set-AppConfigValue XYZ Dir "abc" # (optional)
# Set-AppConfigValue XYZ Exe "xyzabc.cmd" # (optional)
# Set-AppConfigValue XYZ Register $true # (optional)
# Set-AppConfigValue XYZ Path "bin" # (optional)

# Template for npm package
# Set-AppConfigValue XYZ Typ "npm"
# Set-AppConfigValue XYZ NpmPackage "js-abc"
# Set-AppConfigValue XYZ NpmExe "abc.cmd" # (optional)
# Set-AppConfigValue XYZ NpmForceInstall $false # (optional)

# 7Zip
Set-AppConfigValue SvZ Url "http://7-zip.org/a/7za920.zip"
Set-AppConfigValue SvZ Archive "7za*.zip"
Set-AppConfigValue SvZ Dir "7z"
Set-AppConfigValue SvZ Exe "7za.exe"

# Less MSIerables
Set-AppConfigValue Less MsiUrl "https://github.com/activescott/lessmsi/releases/download/v1.3/lessmsi-v1.3.zip"
Set-AppConfigValue Less MsiArchive "lessmsi-*.zip"
Set-AppConfigValue Less MsiExe "lessmsi.exe"
Set-AppConfigValue Less MsiRegister $false

# Git
Set-AppConfigValue Git Url "https://github.com/git-for-windows/git/releases/download/v2.6.4.windows.1/PortableGit-2.6.4-32-bit.7z.exe"
Set-AppConfigValue Git Archive "PortableGit-*-32-bit.7z.exe"
Set-AppConfigValue Git Path "bin"
Set-AppConfigValue Git Exe "git.exe"

# NodeJS
Set-AppConfigValue Node Url "https://nodejs.org/dist/v4.2.3/win-x86/node.exe"
Set-AppConfigValue Node Download "node.exe"
Set-AppConfigValue Node Dir "node"
Set-AppConfigValue Node Exe "node.exe"

# Npm Bootstrap
Set-AppConfigValue NpmBootstrap Url "https://nodejs.org/dist/npm/npm-1.4.12.zip"
Set-AppConfigValue NpmBootstrap Archive "npm-*.zip"
Set-AppConfigValue NpmBootstrap Dir "$(Get-ConfigValue NodeDir)"
Set-AppConfigValue NpmBootstrap Exe "npm.cmd"

# Npm Update
Set-AppConfigValue Npm Typ "npm"
Set-AppConfigValue Npm Exe "npm.cmd"
Set-AppConfigValue Npm NpmForceInstall $true

# Gulp
Set-AppConfigValue Gulp Typ "npm"
Set-AppConfigValue Gulp Exe "gulp.cmd"

# Yeoman
Set-AppConfigValue Yeoman Typ "npm"
Set-AppConfigValue Yeoman NpmPackage "yo"
Set-AppConfigValue Yeoman Exe "yo"

# MdProc Yeoman Generator
Set-AppConfigValue MdProcGen Typ "npm"
Set-AppConfigValue MdProcGen NpmPackage "generator-mdproc"

# JsHint
Set-AppConfigValue JSHint Typ "npm"
Set-AppConfigValue JSHint Exe "jshint"

# Python
Set-AppConfigValue Python Url "https://www.python.org/ftp/python/3.4.3/python-3.4.3.msi"
Set-AppConfigValue Python Archive "python-3.*.msi"
Set-AppConfigValue Python ArchiveSubDir "SourceDir"
Set-AppConfigValue Python Exe "python.exe"

# Pandoc
Set-AppConfigValue Pandoc Url "https://github.com/jgm/pandoc/releases/download/1.15.1.1/pandoc-1.15.1.1-windows.msi"
Set-AppConfigValue Pandoc Archive "pandoc-*-windows.msi"
Set-AppConfigValue Pandoc ArchiveSubDir "SourceDir\Pandoc"
Set-AppConfigValue Pandoc Exe "pandoc.exe"

# GraphViz
Set-AppConfigValue GraphViz Url "http://www.graphviz.org/pub/graphviz/stable/windows/graphviz-2.38.zip"
Set-AppConfigValue GraphViz Archive "graphviz-*.zip"
Set-AppConfigValue GraphViz Path "release\bin"
Set-AppConfigValue GraphViz Exe "dot.exe"

# Inkscape
Set-AppConfigValue Inkscape Url "https://inkscape.org/en/gallery/item/3932/download/"
Set-AppConfigValue Inkscape Archive "Inkscape-*-win32.7z"
Set-AppConfigValue Inkscape ArchiveSubDir "inkscape"
Set-AppConfigValue Inkscape Exe "inkscape.exe"

# MikTeX
Set-AppConfigValue MikTeX Url "http://mirrors.ctan.org/systems/win32/miktex/setup/miktex-portable-2.9.5719.exe"
Set-AppConfigValue MikTeX Archive "miktex-portable-2.*.exe"
Set-AppConfigValue MikTeX Path "miktex\bin"
Set-AppConfigValue MikTeX Exe "latex.exe"

# Visual Studio Code
Set-AppConfigValue VSCode Url "http://go.microsoft.com/fwlink/?LinkID=623231"
Set-AppConfigValue VSCode Archive "VSCode-win32.zip"
Set-AppConfigValue VSCode Dir "code"
Set-AppConfigValue VSCode Exe "code.exe"

# SublimeText 3
Set-AppConfigValue SublimeText Url "http://c758482.r82.cf2.rackcdn.com/Sublime%20Text%20Build%203083.zip"
Set-AppConfigValue SublimeText Archive "Sublime*Text*Build*.zip"
Set-AppConfigValue SublimeText Exe "sublime_text.exe"

#
# Load custom configuration
#

$customConfigFile = "$rootDir\config.ps1"
if (Test-Path $customConfigFile) {
    . $customConfigFile
}
