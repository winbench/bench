# User Configuration

## Environment

With the following properties, you can control the composition of the environment variables and the isolation level of the Bench environment from the Windows system.

* OverrideHome: `true`
* OverrideTemp: `true`
* IgnoreSystemPath: `true`
* UseRegistryIsolation: `true`
* RegisterInUserProfile: `false`
<!-- * EnvironmentPath: `$HomeDir$\bin` -->
<!--
* Environment:
    + `MY_VAR`: `my custom value`
-->

## App Libraries

With the following dictionary property, you can add additional app libraries.

* AppLibs:
    + `core`: `github:mastersign/bench-apps-core`
    + `default`: `github:mastersign/bench-apps-default`

## Quick Access

With the following properties, you can control the appearance of the launchers for the three shells.

* QuickAccessCmd: `false`
* QuickAccessPowerShell: `true`
* QuickAccessBash: `false`

## Dashboard

With the following properties, you can control certain behavior of the Bench Dashboard.

* AutoUpdateCheck: `true`
* DashboardSetupAppListColumns: `Order`, `Label`, `Version`, `Active`, `Deactivated`, `Status`, `Typ`, `Comment`

## Project Archive

With the following properties, you can control the project archive.

* ~~ProjectArchiveFormat: `7z`~~
* ProjectArchiveDir: `archive`

## Properties added by the Bench CLI

If the Bench CLI sets properties, which are not already listed in this file,
it appends them to the end of the file.

