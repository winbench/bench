+++
date = "2017-05-16T09:30:00+02:00"
description = "The properties for the definition of an app"
title = "App Properties"
weight = 8
+++

[app types]: /ref/app-types/
[meta apps]: /ref/app-types/#meta-apps
[default windows apps]: /ref/app-types/#default-windows-app
[Node.js packages]: /ref/app-types/#node-js-package
[Python packages]: /ref/app-types/#python-package
[Ruby packages]: /ref/app-types/#ruby-package

## System Architecture Property Postfix {#sapp}

Bench supports 32 Bit and 64 Bit app binaries.
Because some app properties differ, depending on the targeted system architecture,
the name of these properties can be extended by the postfix `32Bit` and `64Bit` respectively.
The basic property then automatically defaults to the appropriate extended property.
This concept is called _system architecture property postfixes_ (SAPP).

This means, Bench always reads the basic property, and if this is set,
no adaption to the system architecture takes place.
But if the extended property versions are set and the basic property is not
set explicitly, it automatically defaults to the value of the 32 Bit
or the 64 Bit property version, respectively.

The preconditions for using the 64 Bit version of the property are:

* Does the current operating system supports executing 64 Bit code,
  or in other words: is it a 64 Bit Windows?
* Is the config property [`Allow64Bit`](/ref/config/#Allow64Bit) set to `true`?
  Which is not the case by default.

The decision is made every time Bench starts or the configuration is reloaded,
respectively. The decision is available at runtime in the config property
[`Use64Bit`](/ref/config/#Use64Bit).

In combination with variable expansion, SAPP allows defining apps for both
architectures with a minimal number of explicitly set properties.

**Example:**

Consider the app property [`Url`](#Url), which supports the system architecture postfix.
The basic property is `Url`
the extended version for 32 Bit code or the X86 architecture is `Url32Bit`,
and the extended version for 64 Bit code or the AMD64 architecture is `Url64Bit`.

If an app is only released with a 32 Bit binary, then the `Url` property should be set directly.
But if the app is released for both architectures, then the `Url` property must not be set,
instead both properties `Url32Bit` and `Url64Bit` must be set.
Bench always reads the basic property `Url` but if it is not set,
it defaults to either `Url32Bit` or `Url64Bit`.
If the preconditions mentioned above are met, `Url` defaults to `Url64Bit`;
otherwise it defaults to `Url32Bit`.

If an app is only released with a 64 Bit binary, then the `Url64Bit` property should be set,
and the [`Only64Bit`](#Only64Bit) flag should be set to `true`.

## Overview

<!--
#data-table /*/*/Description
#column Property: name(..)
#column App Types: value(../App Types)
#column Required: value(../Required)
#column SAPP: value(../SAPP)
-->

## ID {#ID}

* Description: The ID of the app
* Data Type: string
* Possible Values: alphanumeric characters, no spaces
* Required: `true`
* App Types: all
* SAPP: `false`

## Label {#Label}

* Description: A user friendly name for the app
* Data Type: string
* Required: `false`
* Default: the value of the `ID` property
* App Types: all
* SAPP: `false`

## Typ {#Typ}

* Description: The application typ
* Data Type: string
* Possible Values:
    + `default`
    + `meta`
    + `node-package`
    + `python-package`
    + `python2-package`
    + `python3-package`
    + `ruby-package`
    + `nuget-package`
* Required: `false`
* Default: `default`
* App Types: all
* SAPP: `false`

The meaning of the different possible values is explained in [App Types](/ref/app-types).

## Dependencies {#Dependencies}

* Description: A list with the IDs of all apps in this app group
* Data Type: list of strings
* Possible Values: app IDs
* Required: `false`
* Default: empty
* App Types: all
* SAPP: `false`

## Website {#Website}

* Description: A URL to the main website of this program
* Data Type: URL
* Required: `false`
* Default: empty
* App Types: all
* SAPP: `false`

This URL is used to create an entry in the documentation menu in the
main window of the Bench Dashboard.

## License {#License}

* Description: A SPDX license identifier or `unknown` or `commercial`.
* Data Type: string
* Required: `false`
* Default: `unknown`
* App Types: all
* SAPP: `false`

If this value is set to a SPDX identifier listed in the config property
[`KnownLicenses`](/ref/config/#KnownLicenses),
the [`LicenseUrl`](/ref/app-properties/#LicenseUrl) defaults to the associated URL.

## LicenseUrl {#LicenseUrl}

* Description: A URL or a relative path to a document with the license text.
* Data Type: URL
* Required: `false`
* Default: empty or SPDX license URL
* App Types: all
* SAPP: `false`

## Docs {#Docs}

* Description: A dictionary with documentation URLs for this program
* Data Type: dictionary of labels and URLs
* Required: `false`
* Default: empty
* App Types: all
* SAPP: `false`

This dictionary lists a number of documentation links for this program.
Each entry is a key-value-pair of a label and a URL.
This dictionary is used to create entries in the documentation menu in the
main window of the Bench Dashbaord.

## Force {#Force}

* Description: A boolean, indicating if the package should allways be installed, even if it is already installed
* Data Type: boolean
* Possible Values: `true`, `false`
* Required: `false`
* Default: `false`
* App Types: all
* SAPP: `false`

## Dir {#Dir}

* Description: The name of the target directory for the app
* Data Type: string
* Required: `false`
* Default: the value of the `ID` property in lowercase
* App Types: all
* SAPP: `true`

For package apps like `node-package` or `python3-package`,
the default value is the directory of the respective interpreter/compiler app.

## Path {#Path}

* Description: A list of relative paths inside the app directory to register in the environment `PATH`
* Data Type: list of strings
* Required: `false`
* Default: `.`
* App Types: `meta`, `default`
* SAPP: `true`

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
* SAPP: `false`

## Environment {#Environment}

* Description: A list of key-value-pairs, describing additional environment variables
* Data Type: dictionary
* Required: `false`
* Default: empty
* Example: `MY_APP_HOME: $:Dir$`, `MY_APP_LOG: D:\logs\myapp.log`
* App Types: all
* SAPP: `true`

## Exe {#Exe}

* Description: The path of the main executable for this app
* Data Type: string
* Required: `false`
* Default: `<app ID>.exe`
* App Types: all
* SAPP: `true`

The path can be absolute or relative to the target directory of the app.
For package apps like `node-package` or `python*-package`,
the path can be just the name of CLI wrapper script,
given the package provides a CLI.

## Only64Bit {#Only64Bit}

* Description: A flag to declare that an app is only supported on 64Bit systems.
* Data Type: boolean
* Required: `false`
* Default: `false`
* App Types: all
* SAPP: `false`

## ExeTestArguments {#ExeTestArguments}

* Description: A string to pass as command line arguments when the executable is tested.
* Data Type: string
* Default: empty
* App Types: all
* SAPP: `true`

To test if an app was installed successfully,
the main executable is run with these arguments.
If the process exit code is `0` the test was successful.

## ExeTest {#ExeTest}

* Description: A flag to prevent the test of the main executable.
* Data Type: boolean
* Default: `true`
* App Types: all
* SAPP: `false`

If the main executable of an app can not be tested by executing it with the
[`ExeTestArguments`](#ExeTestArguments), this property must be set to `false`.

## AdornedExecutables {#AdornedExecutables}

* Description: A list of executables, which must be adorned with pre- and post-execution scripts
* Data Type: list of strings
* Required: `false`
* Default: empty
* App Types: all
* SAPP: `true`

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
* SAPP: `true`

## Launcher {#Launcher}

* Description: A label for the app launcher
* Data Type: string
* Required: `false`
* Default: empty
* App Types: all
* SAPP: `false`

A launcher for the app is created only if this property is set to a non empty string.

## LauncherExecutable {#LauncherExecutable}

* Description: The path to the executable targeted by the app launcher
* Data Type: string
* Required: `false`
* Default: the value of the `Exe` property
* App Types: all
* SAPP: `true`

The path can be absolute, or relative to the target directory of the app.

## LauncherArguments {#LauncherArguments}

* Description: A list with arguments to the app executable
* Data Type: list of strings
* Required: `false`
* Default: is `%*`
* App Types: all
* SAPP: `true`

To allow arbitrary arguments to be passed from the launcher to the executable,
the last element in the list must be `%*`.
Passing arguments from the launcher to the executable allows drag-and-drop
for files from the Explorer onto the launcher.

## LauncherWorkingDir {#LauncherWorkingDir}

* Description: A path to the working directory of the launcher shortcut.
* Data Type: string
* Required: `false`
* Default: The value of the config property [`HomeDir`](/ref/config/#HomeDir)
* App Types: all
* SAPP: `false`

## LauncherIcon {#LauncherIcon}

* Description: The path to the icon of the launcher
* Data Type: string
* Possible Values: path to files with the following extension `.ico` and `.exe`
* Required: `false`
* Default: the value of the `Exe` property
* App Types: all
* SAPP: `true`

The path can be absolute or relative to the target directory of the app.

## Url {#Url}

* Description: The URL to the file, containing the app resource
* Data Type: string
* Possible Values: an absolute URL with the protocol `http` or `https`
* Required: `true`
* App Types: `default`
* SAPP: `true`

## DownloadCookies {#DownloadCookies}

* Description: A dictionary with cookies, to send along with the download request
* Data Type: dictionary
* Required: `false`
* Default: empty
* Example: `cookie-name: cookie-value`
* App Types: `default`
* SAPP: `true`

## ResourceName {#ResourceName}

* Description: The name of the downloaded executable file
* Data Type: string
* Possible Values: the name of the executable refered to by `Url` &ndash;
  must have a file extension like `*.exe`, `*.bat`, or `*.cmd`
* Required: `true`\*
* App Types: `default`
* SAPP: `true`

\*) Only one of `ResourceName` or `ArchiveName` must be set.

## ArchiveName {#ArchiveName}

* Description: The name of the downloaded archive
* Data Type: string
* Possible Values: the name of the archive refered to by `Url` &ndash;
  must be a supported archive file like `*.zip`, `*.msi`, or a setup programs
* Required: `true`\*
* App Types: `default`
* SAPP: `true`

\*) Only one of `ResourceName` or `ArchiveName` must be set.

## ArchiveTyp {#ArchiveTyp}

* Description: The archive typ, which controls the extractor selection
* Data Type: string
* Possible Values
    + `auto` Try to determine the extractor by the filename extension or use the custom extractor script if it exists
    + `generic` Use 7-Zip to extract
    + `msi` Use LessMSI to extract
    + `inno` Use Inno Setup Unpacker to extract
    + `custom` Use the custom script `auto\apps\<app ID>.extract.ps1`
* Required: `false`
* Default: `auto``
* App Types: `default`
* SAPP: `true`

This property is only recognized, if [`ArchiveName`](#ArchiveName) is set.

## ArchivePath {#ArchivePath}

* Description: A sub folder in the archive to extract
* Data Type: string
* Possible Values: a relative path or empty
* Required: `false`
* Default: empty &rarr; the archive root
* App Types: `default`
* SAPP: `true`

This property is only recognized, if [`ArchiveName`](#ArchiveName) is set.

## SetupTestFile {#SetupTestFile}

* Description: The path to a file as part of the app installation
* Data Type: string
* Required: `false`
* Default: the value of the `Exe` property
* App Types: `meta`, `default`
* SAPP: `true`

The path is relative to the target directory of the app.
The existence of the described file is used, to determine if the app is already installed.

## PackageName {#PackageName}

* Description: The name of the NPM package to install via NPM
* Data Type: string
* Required: `false`
* Default: the value of the `ID` property in lowercase
* App Types: `*-package`
* SAPP: `false`

## Version {#Version}

* Description: The package version or version range to install by the respective package mananer, if empty install latest
* Required: `false`
* Default: empty
* App Types: `*-package`
* SAPP: `true`

Version Patterns:

* Node.js: `2.5.0` or `>=1.2.0 <3.0.0`
* Python: `2.5.0` or `>=1.2.0,<3.0.0`
* Ruby: `2.5.0`
