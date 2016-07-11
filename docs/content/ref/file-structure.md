+++
date = "2016-06-22T13:43:12+02:00"
description = "The layout of the folders and files in the Bench environment"
draft = true
title = "File Structure"
weight = 4
+++

The file structure of Bench is devided in two groups, the core and the extended file structure.
If the location of files and directories is not changed via custom or site configuration,
all files and folders of Bench live in one directory: the _root directory of Bench_.
In the following sections, the file structure is described according to the default configuration.
If a directory or file can be configured to be elsewhere, the responsible
configuration property is mentioned.

## Core Structure {#core}

The core structure consists of directories and files, which are installed
during the Bench setup, and _can not_ be moved via custom or site configuration.

* [`actions`](#action-dir)
  ([ActionDir](/ref/config/#ActionDir))
    + [`bench-bash.cmd`](#action-bench-bash)
    + [`bench-cmd.cmd`](#action-bench-cmd)
    + [`bench-ctl.cmd`](#action-bench-ctl)
    + [`bench-ps.cmd`](#action-bench-ps)
    + `clone-git-project.cmd`
    + `new-project.cmd`
    + `open-editor.cmd`
    + `project-backup.cmd`
    + `project-editor.cmd`
    + `project-ps.cmd`
    + `project-watch.cmd`
* [`auto`](#auto-dir) Bench Automation
    + [`apps`](#auto-apps-dir) App Custom Scripts
        - [`<app-id>.extract.ps1`](#auto-apps-extract)
        - [`<app-id>.setup.ps1`](#auto-apps-setup)
        - [`<app-id>.env.ps1`](#auto-apps-env)
        - [`<app-id>.remove.ps1`](#auto-apps-remove)
        - [`<app-id>.pre-run.ps1`](#auto-apps-pre-run)
        - [`<app-id>.post-run.ps1`](#auto-apps-post-run)
    + [`bin`](#auto-bin-dir) Bench Binaries
        - [`BenchDashboard.exe`](/ref/dashboard/)
        - [`BenchLib.dll`](/ref/clr-api/)
    + [`lib`](#auto-lib-dir) Bench Scripts
        - `bench.lib.ps1`
        - ...
    + `archive.cmd`
    + `editor.cmd`
    + `init.cmd`
    + `runps.cmd`
* [`config`](#config) Custom Configuration
  ([CustomConfigDir](/ref/config/#CustomConfigDir))
    + [`apps.md`](#config-apps) App Library
      ([CustomAppIndexFile](/ref/config/#CustomAppIndexFile))
    + [`apps-activated.txt`](#config-apps-activated) App Activation List
      ([AppActivationFile](/ref/config/#AppActivationFile))
    + [`apps-deactivated.txt`](#config-apps-deactivated) App Deactivation List
      ([AppDeactivationFile](/ref/config/#AppDeactivationFile))
    + [`config.md`](#config-config) Custom Configuration File
      ([CustomConfigFile](/ref/config/#CustomConfigFile))
    + `ConEmu.xml`
      ([ConEmuConfigFile](/ref/config/#ConEmuConfigFile))
    + `env.ps1` Environment Setup Hook
    + `setup.ps1` Setup Hook
* `res` Bench Resources
    + `apps` App Resources
      ([AppResourceBaseDir](/ref/config/#AppResourceBaseDir))
    + `apps.template.md`
      ([CustomAppIndexTemplateFile](/ref/config/#CustomAppIndexTemplateFile))
    + `apps-activated.template.txt`
      ([AppActivationTemplateFile](/ref/config/#AppActivationTemplateFile))
    + `apps-deactivated.template.txt`
      ([AppDeactivationTemplateFile](/ref/config/#AppDeactivationTemplateFile))
    + `bench-install.bat` Bootstrap Batch File
    + `bench-site.template.md`
      ([SiteConfigTemplateFile](/ref/config/#SiteConfigTemplateFile))
    + `ConEmu.template.xml`
      ([ConEmuConfigTemplateFile](/ref/config/#ConEmuConfigTemplateFile))
    + `config.md` Default Configuration
    + `config.template.md`
      ([CustomConfigTemplateFile](/ref/config/#CustomConfigTemplateFile))
    + `version.txt` Version String
      ([VersionFile](/ref/config/#VersionFile))
* `lib` App Installations
   ([LibDir](/ref/config/#LibDir))
     + `_proxies`
       ([AppAdornmentBaseDir](/ref/config/#AppAdornmentBaseDir))
     + `_launcher`
       ([LauncherScriptDir](/ref/config/#LauncherScriptDir))
     + `<app-id>` App Target Dir
* `CHANGELOG.md`
* `env.cmd`
* `LICENSE.md`
* `README.md`

## Extended Structure {#extended}

The extended structure consists of directories and files, which are created
during the usage of Bench &ndash; including the installation of apps,
and _can_ be moved via custom or site configuration.

* `archive`
  ([ProjectArchiveDir](/ref/config/#ProjectArchiveDir))
* `cache`
  ([DownloadDir](/ref/config/#DownloadDir))
* `home`
  ([HomeDir](/ref/config/#HomeDir))
    + `AppData`
        - `Local`
          ([LocalAppDataDir](/ref/config/#LocalAppDataDir))
        - `Roaming`
          ([AppDataDir](/ref/config/#AppDataDir))
    + `Desktop`
    + `Documents`
    + `registry_isolation`
      ([AppRegistryBaseDir](/ref/config/#AppRegistryBaseDir))
    + ...
* `launcher`
  ([LauncherDir](/ref/config/#LauncherDir))
* `log`
  ([LogDir](/ref/config/#LogDir))
* `projects`
  ([ProjectRootDir](/ref/config/#ProjectRootDir))
* `tmp`
  ([TempDir](/ref/config/#TempDir))
* `bench-site.md`
  ([SiteConfigFileName](/ref/config/#SiteConfigFileName))

## Details {#details}

### Action Directory {#action-dir}

* Description: Script for task execution in the Bench environment.
* Path: `actions`
* Typ: directory
* Config Property: ([ActionDir](/ref/config/#ActionDir))

This directory contains `*.cmd` scripts to run a couple of useful tasks
in the Bench environment.
They do load the `env.cmd` by themselfs if necessary.
Therefore, they can be started directly from an arbitrary command prompt,
via a Windows shortcut, or from the explorer.

### Action `bench-bash` {#action-bench-bash}

* Description: Starts a [Git][] shell in the Bench environment.
* Path: `actions\bench-bash.cmd`
* Typ: file

This action will fail if [Git][] is not installed

### Action `bench-cmd` {#action-bench-cmd}

* Description: Starts a Windows CMD console in the Bench environment.
* Path: `actions\bench-cmd.cmd`
* Typ: file

### Action `bench-ctl` {#action-bench-ctl}

* Description: Starts the [command line interface][Bench CLI] of the Bench manager.
* Path: `actions\bench-ctl.cmd`
* Typ: file

### Action `bench-ps` {#action-bench-ps}

* Description: Starts a PowerShell console in the Bench environment.
* Path: `actions\bench-ps.cmd`
* Typ: file

### Bench Automation Directory {#auto-dir}

* Description: The base directory for the Bench scripts and binaries.
* Path: `auto`
* Typ: directory

### App Custom Script Directory {#auto-apps-dir}

* Description: The directory with the custom scripts of the apps included in Bench.
* Path: `auto\apps`
* Typ: directory

### App Custom Script `extract` {#auto-apps-extract}

* Description: Custom script for app resource extraction.
* Path: `auto\apps\<app-id>.extract.ps1`
* Typ: file

Custom scripts for app resource extraction must be named with the app ID
in lower case, and the name extension `.extract.ps1`.

The custom script for extraction is executed if the
[`ArchiveTyp`](/ref/app-properties/#ArchiveTyp) is set to `auto` or `custom`.
If the [`ArchiveTyp`](/ref/app-properties/#ArchiveTyp) of the app is set
to `auto` and a custom script for extraction for this app exists,
the custom script takes precedence over the other extraction methods.

Inside of the custom script is the [PowerShell API](/ref/ps-api/) iis available.
Custom extraction scripts are called with two command line arguments:

1. The absolute path of the downloaded app resource archive
2. The absolute path of the target directory to extract the resources

Example for extraction of a nested archive:

```PowerShell
param ($archivePath, $targetDir)

# create temporary directory
$tmpDir = Empty-Dir "$(Get-ConfigValue TempDir)\custom_extract"

# get path of 7-Zip
$7z = App-Exe SvZ

# call 7-Zip to extract outer archive
& $7z x "-o$tmpDir" "$archivePath"

# check if expected inner archive exists
if (!(Test-Path "$tmpDir\nested.zip"))
{
    throw "Did not find the expected content in the app resource."
}

# call 7-Zip to extract inner archive
& $7z x "-o$targetDir" "$tmpDir\nested.zip"

# Delete temporary directory
Purge-Dir $tmpDir
```

### App Custom Script `setup` {#auto-apps-setup}

* Description: Custom script for app setup.
* Path: `auto\apps\<app-id>.setup.md`
* Typ: file

Custom scripts for app resource extraction must be named with the app ID
in lower case, and the name extension `.setup.ps1`.

If a custom setup script for an app exists, it is executed after
the installation of the (extracted) app resources in the 
[apps target dir](#lib-app).
Inside of the custom script is the [PowerShell API](/ref/ps-api/) is available.

### App Custom Script `env` {#auto-apps-env}

* Description: Custom script for environment setup.
* Path: `auto\apps\<app-id>.env.ps1`
* Typ: file

Custom scripts for environment setup must be named with the app ID
in lower case, and the name extension `.env.ps1`.

If a custom environment setup script for an app exists, it is executed
after the setup to update configuration files depending
on the location of Bench or other [configuration properties](/ref/config).
It is also called if the _Upade Environment_ task for Bench is executed.
Inside of the custom script is the [PowerShell API](/ref/ps-api/) iis available.

### App Custom Script `remove` {#auto-apps-remove}

* Description: Custom script for app deinstallation.
* Path: `auto\apps\<app-id>.remove.ps1`
* Typ: files

Custom scripts for deinstallation must be named with the app ID
in lower case, and the name extension `.remove.ps1`.

If a custom deinstallation script for an app exists, it is executed
instead of the default uninstall method.
Inside of the custom script is the [PowerShell API](/ref/ps-api/) iis available.

### App Custom Script `pre-run` {#auto-apps-pre-run}

* Description: Pre-run hook for adorned executables of an app.
* Path: `auto\apps\<app-id>.pre-run.ps1`
* Typ: file

The custom pre-run script is executed immediatly before an app executable is run.
It is only executed if an app executable is run via its execution proxy.
This is usually the case because it is listed in 
[AdornedExecutables](/ref/app-properties/#AdornedExecutables).
The [main executable](/ref/app-properties/#Exe) of an app is automatically
included in the list of adorned executables
if the [registry isolation](/ref/app-properties/#RegistryKeys) is used.
Inside of the custom script is the [PowerShell API](/ref/ps-api/) iis available.

### App Custom Script `post-run` {#auto-apps-post-run}

* Description: Post-run hook for adorned executables of an app.
* Path: `auto\apps\<app-id>.post-run.ps1`
* Typ: file

The custom post-run script is executed immediatly after an app executable is run.
It is only executed if an app executable is run via its execution proxy.
This is usually the case because it is listed in 
[AdornedExecutables](/ref/app-properties/#AdornedExecutables).
The [main executable](/ref/app-properties/#Exe) of an app is automatically
included in the list of adorned executables
if the [registry isolation](/ref/app-properties/#RegistryKeys) is used.
Inside of the custom script is the [PowerShell API](/ref/ps-api/) iis available.

### Bench Binary Directory {#auto-bin-dir}

* Description: The directory with all binary executables and libraries of Bench.
* Path: `auto\bin`
* Typ: directory

### Bench Script Directory {#auto-lib-dir}

* Description: The directory with the PowerShell scripts of Bench.
* Path: `auto\lib`
* Typ: directory

### Configuration Directory {#config-dir}

* Description: The directory for the custom configuration.
* Path: `config`
* Config Property: [CustomConfigDir](/ref/config/#CustomConfigDir)
* Typ: directory

This directory is designed to be put under version control,
to manage and share Bench configurations.

### Custom App Library {#config-apps}

* Description: The custom app library.
* Path: `config\apps.md`
* Config Property: [CustomAppIndexFile](/ref/config/#CustomAppIndexFile)
* Typ: file

The custom app libary file is written in [Markdown list syntax](/ref/markup-syntax).

### App Activation List {#config-apps-activated}

* Description: The list of activated apps.
* Path: `config\apps-activated.txt`
* Config Property: [AppActivationFile](/ref/config/#AppActivationFile)
* Typ: file

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a # is interpreted
as an app ID.
Only non-space characters, up to the first space or the end of a line,
are considered.

### App Deactivation List {#config-apps-deactivated}

* Description: The list of deactivated apps.
* Path: `config\apps-deactivated.txt`
* Config Property: [AppDeactivationFile](/ref/config/#AppDeactivationFile)
* Typ: file

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a # is interpreted
as an app ID.
Only non-space characters, up to the first space or the end of a line,
are considered.

### Custom Configuration File {#config-config}

* Description: The custom configuration file.
* Path: `config\config.md`
* Config Property: [CustomConfigFile](/ref/config/#CustomConfigFile)
* Typ: file

The custom configuration file is written in [Markdown list syntax](/ref/markup-syntax).

### Environment Setup Hook {#config-env}

* Description: The hook script for environment setup.
* Path: `config\env.ps1`
* Typ: file

This script is executed, at the end of the Bench environment setup.
Inside of the custom script is the [PowerShell API](/ref/ps-api/) iis available.

[Bench CLI]: /ref/bench-ctl
[Git]: /ref/apps#Git
