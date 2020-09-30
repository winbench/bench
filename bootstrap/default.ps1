#
# Run this script with
#
# powershell -NoLogo -ExecutionPolicy ByPass -Command "iex (iwr https://raw.githubusercontent.com/winbench/bench/master/bootstrap/default.ps1).Content"
#

$benchRootName = "Bench"
$benchRoot = "${env:SystemDrive}\$benchRootName"
$benchVersionUrl = "https://raw.githubusercontent.com/winbench/bench/master/res/version.txt"
$benchVersion = [string](Invoke-WebRequest -Uri $benchVersionUrl).Content
$benchArchiveUrl = "https://github.com/winbench/bench/releases/download/v${benchVersion}/Bench.zip"
$benchArchive = "Bench.zip"

$Allow64Bit = [Environment]::Is64BitOperatingSystem
$ActivatedApps = @(
    "Bench.Git"
    "Bench.Python3"
    "Bench.Node"
)
$Mode = "default"
$GitUserName = "User"
$GitUserEmail = "user@localhost"

switch ($Mode) {
    "integrated" {
        $OverrideHome = $false
        $OverrideTemp = $false
        $IgnoreSystemPath = $false
        $UseRegistryIsolation = $false
        $RegisterInUserProfile = $true
        $AutoUpdateCheck = $true
    }
    "isolated" {
        $OverrideHome = $true
        $OverrideTemp = $true
        $IgnoreSystemPath = $true
        $UseRegistryIsolation = $true
        $RegisterInUserProfile = $false
        $AutoUpdateCheck = $true
    }
    Default {
        $OverrideHome = $true
        $OverrideTemp = $true
        $IgnoreSystemPath = $false
        $UseRegistryIsolation = $false
        $RegisterInUserProfile = $false
        $AutoUpdateCheck = $true
    }
}

$ErrorActionPreference = "Stop"

function writeUtf8File($path) {
    begin {
        if (![IO.Path]::IsPathRooted($path)) {
            $path = [IO.Path]::Combine((Get-Location), $path)
        }
        $Utf8WithoutBom = New-Object System.Text.UTF8Encoding $false
    }
    process {
        [IO.File]::WriteAllText($path, $_, $Utf8WithoutBom)
    }
}

if (Test-Path $benchRoot) {
    Write-Warning "The Bench directory already exists. Cancelling setup."
    Write-Host "Press any key to close this script..."
    $Host.UI.RawUI.ReadKey() | Out-Null
    return
} else {
    mkdir $benchRoot | Out-Null
}

Set-Location $benchRoot

##############################
# Download and Extract Bench #
##############################

if (!(Test-Path "auto")) {
    if (!(Test-Path $benchArchive)) {
        Invoke-WebRequest -Uri $benchArchiveUrl -OutFile $benchArchive
    }
    Expand-Archive -Path $benchArchive -DestinationPath "."
    Remove-Item $benchArchive
}

############################
# Initialize Configuration #
############################

function bool($v) {
    if ($v) { "``true``" } else { "``false``" }
}

if (!(Test-Path "config")) {

    mkdir config | Out-Null

    # Write site sonfiguration file

@"
# Site Configuration

## Developer Identity

* UserName: ``${GitUserName}``
* UserEmail: ``${GitUserEmail}``

## Machine Architecture

* Allow64Bit: $(bool $Allow64Bit)
"@ | writeUtf8File "bench-site.md"

    # Write main configuration file

@"
# User Configuration

## Environment

With the following properties you can control the composition of the environment variables
and the isolation level of the Bench environment from the Windows system.

* OverrideHome: $(bool $OverrideHome)
* OverrideTemp: $(bool $OverrideTemp)
* IgnoreSystemPath: $(bool $IgnoreSystemPath)
* UseRegistryIsolation: $(bool $UseRegistryIsolation)
* RegisterInUserProfile: $(bool $RegisterInUserProfile)
* EnvironmentPath: ```$UserConfigDir`$\bin``
<!--
* LauncherDir: ```$HomeDir`$\Desktop\Bench``
* Environment:
    + ``MY_VAR``: ``my custom value``
-->

## App Libraries

With the following dictionary property, you can add additional app libraries.

* AppLibs:
    + ``core``: ``github:winbench/apps-core``
    + ``default``: ``github:winbench/apps-default``
    + ``mastersign``: ``github:mastersign/bench-apps-mastersign``

## Quick Access

With the following properties you can control the appearance of the launchers for the three shells.

* QuickAccessCmd: ``true``
* QuickAccessPowerShell: ``true``
* QuickAccessBash: ``false``

## Dashboard

With the following properties you can control certain behavior of the Bench Dashboard.

* AutoUpdateCheck: $(bool $AutoUpdateCheck)
* DashboardSetupAppListColumns: ``Order``, ``Category``, ``Label``, ``Version``, ``Active``, ``Deactivated``, ``Status``, ``Typ``, ``License``, ``Comment``

## Properties added by the Bench CLI

If the Bench CLI sets properties, which are not already listed in this file,
it appends them to the end of the file.
"@ | writeUtf8File "config\config.md"

    # Write template for user app library

@"
# User App Library

This document is the registry for user defined applications in _Bench_.

[markup syntax]: https://winbench.org/ref/markup-syntax/
[app properties]: https://winbench.org/ref/app-properties/
[app types]: https://winbench.org/ref/app-types/

For the syntax of app definitions, take a look at the
documentation for the [markup syntax][] and the supported [app properties][].
Learn about the different [app types][] you can define.

Activate your apps by adding them to the list of activated apps in ``apps-activated.txt``,
or activate them via the _Bench Dashboard_ setup dialog,
or activate them by calling ``bench app activate <App ID>``.

## User

Place your own app definitions here.
"@ | writeUtf8File "config\apps.md"

    # Write list of initially activated apps

@"
# Activated Apps
# --------------

# This file contains the list with all activated apps.
# The first word in a non-empty line, not commented out with #, is treated as an app ID.

"@ + [String]::Join([Environment]::NewLine, $ActivatedApps) `
| writeUtf8File "config\apps-activated.txt"

    # Write list of initially deactivated apps

@"
# Deactivated Apps
# ----------------

# This file contains the list with all deactivated apps.
# The first word in a non-empty line, not commented out with #, is treated as an app ID.

# This file superseeds the listed apps from apps-activated.txt and their dependencies.
"@ | writeUtf8File "config\apps-deactivated.txt"

}

###########################
# Setup Bench Environment #
###########################

if (!(Test-Path "lib\applibs")) {
    .\auto\bin\bench.exe --verbose manage load-app-libs
}
if (!(Test-Path "lib\apps")) {
    .\auto\bin\bench.exe --verbose manage setup
}

################################
# Copy Shortcut on the Desktop #
################################

Copy-Item "launcher\Bench Dashboard.lnk" "$env:UserProfile\Desktop\Bench.lnk"
