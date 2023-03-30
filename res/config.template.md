# User Configuration

Have a look at <https://winbench.org/ref/config/>
for a detailed explanation of the configuration properties.

## Environment

With the following properties, you can control the composition of the environment variables and the isolation level of the Bench environment from the Windows system.

* OverrideHome: `true`
* OverrideTemp: `true`
* IgnoreSystemPath: `true`
* UseRegistryIsolation: `true`
* RegisterInUserProfile: `false`
<!-- * LauncherDir: `$AppDataDir$\Microsoft\Windows\Start Menu\Bench` -->
<!-- * EnvironmentPath: `$HomeDir$\bin` -->
<!--
* Environment:
    + `MY_VAR`: `my custom value`
-->

## App Libraries

With the following dictionary property, you can add additional app libraries.

* AppLibs:
    + `core`: `github:winbench/apps-core`
    + `default`: `github:winbench/apps-default`

## Quick Access

With the following properties, you can control the appearance of the launchers for the three shells.

* QuickAccessCmd: `false`
* QuickAccessPowerShell: `true`
* QuickAccessPowerShellCore: `true`
* QuickAccessBash: `false`

## Dashboard

With the following properties, you can control certain behavior of the Bench Dashboard.

* AutoUpdateCheck: `true`
* DashboardSetupAppListColumns: `Order`, `Label`, `Version`, `Active`, `Deactivated`, `Status`, `Typ`, `Comment`
* DashboardSavePositions: `true`
* DashboardMainPosition: `default`
* DashboardSetupPosition: `default`
* DashboardMarkdownViewerPosition: `default`

## Project Archive

With the following properties, you can control the project archive.

* ProjectArchiveFormat: `zip`
* ProjectArchiveDir: `archive`

## Properties added by the Bench CLI

If the Bench CLI sets properties, which are not already listed in this file,
it appends them to the end of the file.

