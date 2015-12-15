$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$rootDir = Resolve-Path "$myDir\.."
. "$myDir\common.lib.ps1"

$Script:config = @{}

function Set-ConfigValue($name, $value) {
    if ($Script:debug) {
        Debug "Config: $name = $value"
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
Set-ConfigValue ResFile "res\resources.md"
Set-ConfigValue TempDir "tmp"
Set-ConfigValue LibDir "lib"
Set-ConfigValue HomeDir "home"
Set-ConfigValue AppDataDir "$(Get-ConfigValue HomeDir)\AppData"
Set-ConfigValue LocalAppDataDir "$(Get-ConfigValue HomeDir)\LocalAppData"
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
    "Python",
    "VSCode",
    "Pandoc",
    "GraphViz",
    "Inkscape",
    "MikTeX"
)

# Template for command line tool
# Set-ConfigValue XYZTyp "default"
# Set-ConfigValue XYZUrl "http://xyz.org/latest"
# Set-ConfigValue XYZArchive "xyz-*.zip"
# Set-ConfigValue XYZArchiveSubDir "abc"
# Set-ConfigValue XYZDir "abc"
# Set-ConfigValue XYZPath "bin"
# Set-ConfigValue XYZExe "xyzabc.cmd"
# Set-ConfigValue XYZRegister $true

# Template for npm package
# Set-ConfigValue XYZTyp "npm"
# Set-ConfigValue XYZNpmPackage "js-abc"
# Set-ConfigValue XYZNpmExe "abc.cmd"
# Set-ConfigValue XYZNpmForceInstall $false

# 7Zip
Set-ConfigValue SvZUrl "http://7-zip.org/a/7za920.zip"
Set-ConfigValue SvZArchive "7za*.zip"
Set-ConfigValue SvZDir "7z"
Set-ConfigValue SvZExe "7za.exe"

# Less MSIerables
Set-ConfigValue LessMsiUrl "https://github.com/activescott/lessmsi/releases/download/v1.3/lessmsi-v1.3.zip"
Set-ConfigValue LessMsiArchive "lessmsi-*.zip"
Set-ConfigValue LessMsiExe "lessmsi.exe"
Set-ConfigValue LessMsiRegister $false

# Git
Set-ConfigValue GitUrl "https://github.com/git-for-windows/git/releases/download/v2.6.4.windows.1/PortableGit-2.6.4-32-bit.7z.exe"
Set-ConfigValue GitArchive "PortableGit-*-32-bit.7z.exe"
Set-ConfigValue GitPath "bin"
Set-ConfigValue GitExe "git.exe"

# NodeJS
Set-ConfigValue NodeUrl "https://nodejs.org/dist/v4.2.3/win-x86/node.exe"
Set-ConfigValue NodeDownload "node.exe"
Set-ConfigValue NodeDir "node"
Set-ConfigValue NodeExe "node.exe"

# Npm Bootstrap
Set-ConfigValue NpmBootstrapUrl "https://nodejs.org/dist/npm/npm-1.4.12.zip"
Set-ConfigValue NpmBootstrapArchive "npm-*.zip"
Set-ConfigValue NpmBootstrapDir "$(Get-ConfigValue NodeDir)"
Set-ConfigValue NpmBootstrapExe "npm.cmd"

# Npm Update
Set-ConfigValue NpmTyp "npm"
Set-ConfigValue NpmExe "npm.cmd"
Set-ConfigValue NpmNpmForceInstall $true

# Gulp
Set-ConfigValue GulpTyp "npm"
Set-ConfigValue GulpExe "gulp.cmd"

# Yeoman
Set-ConfigValue YeomanTyp "npm"
Set-ConfigValue YeomanNpmPackage "yo"
Set-ConfigValue YeomanExe "yo"

# MdProc Yeoman Generator
Set-ConfigValue MdProcGenTyp "npm"
Set-ConfigValue MdProcGenNpmPackage "generator-mdproc"

# JsHint
Set-ConfigValue JSHintTyp "npm"
Set-ConfigValue JSHintExe "jshint"

# Python
Set-ConfigValue PythonUrl "https://www.python.org/ftp/python/3.4.3/python-3.4.3.msi"
Set-ConfigValue PythonArchive "python-3.*.msi"
Set-ConfigValue PythonArchiveSubDir "SourceDir"
Set-ConfigValue PythonExe "python.exe"

# Pandoc
Set-ConfigValue PandocUrl "https://github.com/jgm/pandoc/releases/download/1.15.1.1/pandoc-1.15.1.1-windows.msi"
Set-ConfigValue PandocArchive "pandoc-*-windows.msi"
Set-ConfigValue PandocArchiveSubDir "SourceDir\Pandoc"
Set-ConfigValue PandocExe "pandoc.exe"

# GraphViz
Set-ConfigValue GraphVizUrl "http://www.graphviz.org/pub/graphviz/stable/windows/graphviz-2.38.zip"
Set-ConfigValue GraphVizArchive "graphviz-*.zip"
Set-ConfigValue GraphVizPath "release\bin"
Set-ConfigValue GraphVizExe "dot.exe"

# Inkscape
Set-ConfigValue InkscapeUrl "https://inkscape.org/en/gallery/item/3932/download/"
Set-ConfigValue InkscapeArchive "Inkscape-*-win32.7z"
Set-ConfigValue InkscapeArchiveSubDir "inkscape"
Set-ConfigValue InkscapeExe "inkscape.exe"

# MikTeX
Set-ConfigValue MikTeXUrl "http://mirrors.ctan.org/systems/win32/miktex/setup/miktex-portable-2.9.5719.exe"
Set-ConfigValue MikTeXArchive "miktex-portable-2.*.exe"
Set-ConfigValue MikTeXPath "miktex\bin"
Set-ConfigValue MikTeXExe "latex.exe"

# Visual Studio Code
Set-ConfigValue VSCodeUrl "http://go.microsoft.com/fwlink/?LinkID=623231"
Set-ConfigValue VSCodeArchive "VSCode-win32.zip"
Set-ConfigValue VSCodeDir "code"
Set-ConfigValue VSCodeExe "code.exe"


#
# Load custom configuration
#

$customConfigFile = "$rootDir\config.ps1"
if (Test-Path $customConfigFile) {
    . $customConfigFile
}
