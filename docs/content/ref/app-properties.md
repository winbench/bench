+++
date = "2016-06-22T13:43:28+02:00"
description = "The properties for the definition of an app"
title = "App Properties"
weight = 8
+++

| Property | App Types | Required |
|----------|-----------|----------|
| [ID](#ID) | all | `true` |
| [Label](#Label) | all | `false` |
| [Typ](#Typ) | all | `false` |
| [Dependencies](#Dependencies) | all | `false` |
| [Force](#Force) | all | `false` |
| [Dir](#Dir) | all | `false` |
| [Path](#Path) | `meta`, `default` | `false` |
| [Register](#Register) | `meta`, `default` | `false` |
| [Environment](#Environment) | all | `false` |
| [Exe](#Exe) | all | `false` |
| [AdornedExecutables](#AdornedExecutables) | all | `false` |
| [RegistryKeys](#RegistryKeys) | all | `false` |
| [Launcher](#Launcher) | all | `false` |
| [LauncherExecutable](#LauncherExecutable) | all | `false` |
| [LauncherArguments](#LauncherArguments) | all | `false` |
| [LauncherIcon](#LauncherIcon) | all | `false` |
| [Url](#Url) | `default` | `true` |
| [DownloadCookies](#DownloadCookies) | `default` | `false` |
| [ResourceName](#ResourceName) | `default` | `true`* |
| [ArchiveName](#ArchiveName) | `default` | `true`* |
| [ArchiveTyp](#ArchiveTyp) | `default` | `false` |
| [ArchivePath](#ArchivePath) | `default` | `false` |
| [SetupTestFile](#SetupTestFile) | `meta`, `default` | `false` |
| [PackageName](#PackageName) | `*-package` | `false` |
| [Version](#Version) | `*-package` | `false` |

## ID {#ID}

* Description: The ID of the app
* Data Type: string
* Possible Values: alphanumeric characters, no spaces
* Required: `true`
* App Types: all

## Label {#Label}

* Description: A user friendly name for the app
* Data Type: string
* Required: `false`
* Default: the value of the `ID` property
* App Types: all

## Typ {#Typ}

* Description: The application typ
* Data Type: string
* Possible Values:
    + `default`
    + `meta`
    + `node-package`
    + `python2-package`
    + `python3-package`
    + `ruby-package`
    + `nuget-package`
* Required: `false`
* Default: `default`
* App Types: all

The meaning of the different possible values is explained in [App Types](/ref/app-types).

## Dependencies {#Dependencies}

* Description: A list with the IDs of all apps in this app group
* Data Type: list of strings
* Possible Values: app IDs
* Required: `false`
* Default: empty
* App Types: all

## Force {#Force}

* Description: A boolean, indicating if the package should allways be installed, even if it is already installed
* Data Type: boolean
* Possible Values: `true`, `false`
* Required: `false`
* Default: `false`
* App Types: all

## Dir {#Dir}

* Description: The name of the target directory for the app
* Data Type: string
* Required: `false`
* Default: the value of the `ID` property in lowercase
* App Types: all

For package apps like `node-package` or `python3-package`,
the default value is the directory of the respective interpreter/compiler app.

## Path {#Path}

* Description: A list of relative paths inside the app directory to register in the environment `PATH`
* Data Type: list of strings
* Required: `false`
* Default: `.`
* App Types: `meta`, `default`

This property is only recognized, if `Register` is `true`.
For package apps like `node-package` or `python3-package`,
this property is ignored and implicitly set to the directory
of its interpreter/compiler app where the CLI wrapper scripts are stored.

## Register {#Register}

* Description: A boolean to indicate if the path(s) of the application should be added to the environment `PATH`
* Data Type: boolean
* Possible Values: `true`, `false`
* Required: `false`
* Default: `true`
* App Types: `meta`, `default`

## Environment {#Environment}

* Description: A list of key-value-pairs, describing additional environment variables
* Data Type: dictionary
* Required: `false`
* Default: empty
* Example: `MY_APP_HOME: $:Dir$`, `MY_APP_LOG: D:\logs\myapp.log`
* App Types: all

## Exe {#Exe}

* Description: The path of the main executable for this app
* Data Type: string
* Required: `false`
* Default: `<app ID>.exe`
* App Types: all

The path can be absolute or relative to the target directory of the app.
For package apps like `node-package` or `python3-package`,
the path can be just the name of CLI wrapper script,
given the package provides a CLI.

## AdornedExecutables {#AdornedExecutables}

* Description: A list of executables, which must be adorned with pre- and post-execution scripts
* Data Type: list of strings
* Required: `false`
* Default: empty
* App Types: all

Every listed path must be relative to the target directory of the app.

## RegistryKeys {#RegistryKeys}

* Description: A list of registry keys to back up and restore for isolation
  during the execution of this app
* Data Type: list of string
* Possible Values: relative key paths in the Windows registry hive `HKEY_CURRENT_USER`
* Required: `false`
* Default: empty
* Example: `Software\Company Foo\Program Bar`
* App Types: all

## Launcher {#Launcher}

* Description: A label for the app launcher
* Data Type: string
* Required: `false`
* Default: empty
* App Types: all

A launcher for the app is created only if this property is set to a non empty string.

## LauncherExecutable {#LauncherExecutable}

* Description: The path to the executable targeted by the app launcher
* Data Type: string
* Required: `false`
* Default: the value of the `Exe` property
* App Types: all

The path can be absolute, or relative to the target directory of the app.

## LauncherArguments {#LauncherArguments}

* Description: A list with arguments to the app executable
* Data Type: list of strings
* Required: `false`
* Default: is `%*`
* App Types: all

To allow arbitrary arguments to be passed from the launcher to the executable,
the last element in the list must be `%*`.
Passing arguments from the launcher to the executable allows drag-and-drop
for files from the Explorer onto the launcher.

## LauncherIcon {#LauncherIcon}

* Description: The path to the icon of the launcher
* Data Type: string
* Possible Values: path to files with the following extension `.ico` and `.exe`
* Required: `false`
* Default: the value of the `Exe` property
* App Types: all

The path can be absolute or relative to the target directory of the app.

## Url {#Url}

* Description: The URL to the file, containing the app resource
* Data Type: string
* Possible Values: an absolute URL with the protocol `http` or `https`
* Required: `true`
* App Types: `default`

## DownloadCookies {#DownloadCookies}

* Description: A dictionary with cookies, to send along with the download request
* Data Type: dictionary
* Required: `false`
* Default: empty
* Example: `cookie-name: cookie-value`
* App Types: `default`

## ResourceName {#ResourceName}

* Description: The name of the downloaded executable file
* Data Type: string
* Possible Values: the name of the executable refered to by `Url`,
  must have a file extension like `*.exe`, `*.bat`, or `*.cmd`
* Required: `true`*
* App Types: `default`

*) Only one of `ResourceName` or `ArchiveName` must be set.

## ArchiveName {#ArchiveName}

* Description: The name of the downloaded archive
* Data Type: string
* Possible Values: the name of the archive refered to by `Url`,
  must be a supported archive file like `*.zip`, `*.msi`, or a setup programs
* Required: `true`*
* App Types: `default`

*) Only one of `ResourceName` or `ArchiveName` must be set.

## ArchiveTyp {#ArchiveTyp}

* Description: The archive typ, which controls the extractor selection
* Required: `false`
* Data Type: string
* Possible Values
    + `auto` Try to determine the extractor by the filename extension or use the custom extractor script if it exists
    + `generic` Use 7-Zip to extract
    + `msi` Use LessMSI to extract
    + `inno` Use Inno Setup Unpacker to extract
    + `custom` Use the custom script `auto\apps\<app ID>.extract.ps1`
* Required: `false`
* Default: `auto`
* App Types: `default`

This property is only recognized, if the `ArchiveName` property is set.

## ArchivePath {#ArchivePath}

* Description: A sub folder in the archive to extract
* Data Type: string
* Possible Values: a relative path or empty
* Required: `false`
* Default: empty &rarr; the archive root
* App Types: `default`

This property is only recognized, if the property `ArchiveName` is set.

## SetupTestFile {#SetupTestFile}

* Description: The path to a file as part of the app installation
* Data Type: string
* Required: `false`
* Default: the value of the `Exe` property
* App Types: `meta`, `default`

The path is relative to the target directory of the app.
The existence of thdescribed file is used, to determine if the app is already installed.

## PackageName {#PackageName}

* Description: The name of the NPM package to install via NPM
* Data Type: string
* Required: `false`
* Default: the value of the `ID` property in lowercase
* App Types: `*-package`

## Version {#Version}

* Description: The package version or version range to install by the respective package mananer, if empty install latest
* Required: `false`
* Default: empty
* App Types: `*-package`

Version Patterns:

* Node.js: `2.5.0` or `>=1.2.0 <3.0.0`
* Python: `2.5.0` or `>=1.2.0,<3.0.0`
* Ruby: `2.5.0`

[app types]: /ref/app-types/
[meta apps]: /ref/app-types/#meta-apps
[default windows apps]: /ref/app-types/#default-windows-app
[Node.js packages]: /ref/app-types/#node-js-package
[Python packages]: /ref/app-types/#python-package
[Ruby packages]: /ref/app-types/#ruby-package
