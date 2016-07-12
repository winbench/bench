+++
date = "2016-06-22T13:43:12+02:00"
description = "The layout of the folders and files in the Bench environment"
title = "File Structure"
weight = 4
+++

The file structure of Bench is divided in two groups, the core and the extended file structure.
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
    + [`ConEmu.xml`](#config-conemu)
      ([ConEmuConfigFile](/ref/config/#ConEmuConfigFile))
    + [`env.ps1`](#config-env) Environment Setup Hook
    + [`setup.ps1`](#config-setup) Setup Hook
* [`res`](#res-dir) Bench Resources
    + [`apps`](#res-apps-dir) App Custom Resources
      ([AppResourceBaseDir](/ref/config/#AppResourceBaseDir))
    + [`apps.template.md`](#res-apps-template)
      ([CustomAppIndexTemplateFile](/ref/config/#CustomAppIndexTemplateFile))
    + [`apps-activated.template.txt`](#res-app-activation-template)
      ([AppActivationTemplateFile](/ref/config/#AppActivationTemplateFile))
    + [`apps-deactivated.template.txt`](#res-app-deactivation-template)
      ([AppDeactivationTemplateFile](/ref/config/#AppDeactivationTemplateFile))
    + [`bench-install.bat`](#res-bench.install) Bootstrap Batch File
    + [`bench-site.template.md`](#res-site-config-template)
      ([SiteConfigTemplateFile](/ref/config/#SiteConfigTemplateFile))
    + [`ConEmu.template.xml`](#res-conemu-template)
      ([ConEmuConfigTemplateFile](/ref/config/#ConEmuConfigTemplateFile))
    + [`config.md`](#res-config) Default Configuration
    + [`config.template.md`](#res-config-template)
      ([CustomConfigTemplateFile](/ref/config/#CustomConfigTemplateFile))
    + [`version.txt`](#res-version) Version String
      ([VersionFile](/ref/config/#VersionFile))
* [`lib`](#lib-dir) App Installations
   ([LibDir](/ref/config/#LibDir))
     + [`_proxies`](#lib-proxies-dir)
       ([AppAdornmentBaseDir](/ref/config/#AppAdornmentBaseDir))
     + [`_launcher`](#lib-launcher-dir)
       ([LauncherScriptDir](/ref/config/#LauncherScriptDir))
     + ...
* [`CHANGELOG.md`](#changelog)
* [`env.cmd`](#env)
* [`LICENSE.md`](#license)
* [`README.md`](#readme)

## Extended Structure {#extended}

The extended structure consists of directories and files, which are created
during the usage of Bench &ndash; including the installation of apps,
and _can_ be moved via custom or site configuration.

* [`archive`](#archive-dir) Project Archive
  ([ProjectArchiveDir](/ref/config/#ProjectArchiveDir))
* [`cache`](#cache-dir) Downloaded App Resources
  ([DownloadDir](/ref/config/#DownloadDir))
* [`home`](#home-dir) Isolated User Profile
  ([HomeDir](/ref/config/#HomeDir))
    + `AppData`
        - [`Local`](#home-app-data-local-dir)
          ([LocalAppDataDir](/ref/config/#LocalAppDataDir))
        - [`Roaming`](#home-app-data-roaming-dir)
          ([AppDataDir](/ref/config/#AppDataDir))
    + `Desktop`
    + `Documents`
    + [`registry_isolation`](#home-registry-isolation-dir)
      ([AppRegistryBaseDir](/ref/config/#AppRegistryBaseDir))
    + ...
* [`launcher`](#launcher-dir) Launcher Shortcuts
  ([LauncherDir](/ref/config/#LauncherDir))
* [`log`](#log-dir) Log Files
  ([LogDir](/ref/config/#LogDir))
* [`projects`](#projects-dir) Projects Root Directory
  ([ProjectRootDir](/ref/config/#ProjectRootDir))
* [`tmp`](#tmp-dir) Temporary Files
  ([TempDir](/ref/config/#TempDir))
* [`bench-site.md`](#bench-site) Site Configuration
  ([SiteConfigFileName](/ref/config/#SiteConfigFileName))

## Details {#details}

### Action Directory {#action-dir}

* Description: Script for task execution in the Bench environment.
* Path: `actions`
* Typ: directory
* Config Property: ([ActionDir](/ref/config/#ActionDir))

This directory contains `*.cmd` scripts to run a couple of useful tasks
in the Bench environment.
They do load the `env.cmd` by them selfs if necessary.
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

The _custom script for extraction_ is executed if the
[`ArchiveTyp`](/ref/app-properties/#ArchiveTyp) is set to `auto` or `custom`.
If the [`ArchiveTyp`](/ref/app-properties/#ArchiveTyp) of the app is set
to `auto` and a _custom script for extraction_ for this app exists,
the custom script takes precedence over the other extraction methods.

Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.
Custom extraction scripts are called with two command line arguments:

1. The absolute path of the downloaded app resource archive
2. The absolute path of the target directory to extract the resources

Example for the extraction of a nested archive:

```PowerShell
param ($archivePath, $targetDir)

$nestedArchive = "nested.zip"

# create temporary directory
$tmpDir = Empty-Dir "$(Get-ConfigValue TempDir)\custom_extract"

# get path of 7-Zip
$7z = App-Exe SvZ

# call 7-Zip to extract outer archive
& $7z x "-o$tmpDir" "$archivePath"

# check if expected inner archive exists
if (!(Test-Path "$tmpDir\$nestedArchive"))
{
    throw "Did not find the expected content in the app resource."
}

# call 7-Zip to extract inner archive
& $7z x "-o$targetDir" "$tmpDir\$nestedArchive"

# Delete temporary directory
Purge-Dir $tmpDir
```

### App Custom Script `setup` {#auto-apps-setup}

* Description: Custom script for app setup.
* Path: `auto\apps\<app-id>.setup.md`
* Typ: file

Custom scripts for app resource extraction must be named with the app ID
in lower case, and the name extension `.setup.ps1`.

If a _custom setup script_ for an app exists, it is executed after
the installation of the (extracted) app resources in the
[apps target dir](#lib-app).
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) is available.

### App Custom Script `env` {#auto-apps-env}

* Description: Custom script for environment setup.
* Path: `auto\apps\<app-id>.env.ps1`
* Typ: file

Custom scripts for environment setup must be named with the app ID
in lower case, and the name extension `.env.ps1`.

If a _custom environment setup script_ for an app exists, it is executed
after the setup to update configuration files depending
on the location of Bench or other [configuration properties](/ref/config).
It is also called if the _Upade Environment_ task for Bench is executed.
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.

### App Custom Script `remove` {#auto-apps-remove}

* Description: Custom script for app deinstallation.
* Path: `auto\apps\<app-id>.remove.ps1`
* Typ: files

Custom scripts for deinstallation must be named with the app ID
in lower case, and the name extension `.remove.ps1`.

If a _custom deinstallation script_ for an app exists, it is executed
instead of the default uninstall method.
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.

### App Custom Script `pre-run` {#auto-apps-pre-run}

* Description: Pre-run hook for adorned executables of an app.
* Path: `auto\apps\<app-id>.pre-run.ps1`
* Typ: file

The _custom pre-run script_ is executed immediatly before an app executable is run.
It is only executed if an app executable is run via its execution proxy.
This is usually the case because it is listed in
[AdornedExecutables](/ref/app-properties/#AdornedExecutables).
The [main executable](/ref/app-properties/#Exe) of an app is automatically
included in the list of adorned executables
if the [registry isolation](/ref/app-properties/#RegistryKeys) is used.
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.

### App Custom Script `post-run` {#auto-apps-post-run}

* Description: Post-run hook for adorned executables of an app.
* Path: `auto\apps\<app-id>.post-run.ps1`
* Typ: file

The _custom post-run script_ is executed immediatly after an app executable is run.
It is only executed if an app executable is run via its execution proxy.
This is usually the case because it is listed in
[AdornedExecutables](/ref/app-properties/#AdornedExecutables).
The [main executable](/ref/app-properties/#Exe) of an app is automatically
included in the list of adorned executables
if the [registry isolation](/ref/app-properties/#RegistryKeys) is used.
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.

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
Every non empty line, which is not commented with a `#` is interpreted
as an app ID.
Only non-space characters, up to the first space or the end of a line,
are considered.

### App Deactivation List {#config-apps-deactivated}

* Description: The list of deactivated apps.
* Path: `config\apps-deactivated.txt`
* Config Property: [AppDeactivationFile](/ref/config/#AppDeactivationFile)
* Typ: file

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a `#` is interpreted
as an app ID.
Only non-space characters, up to the first space or the end of a line,
are considered.

### Custom Configuration File {#config-config}

* Description: The custom configuration file.
* Path: `config\config.md`
* Config Property: [CustomConfigFile](/ref/config/#CustomConfigFile)
* Typ: file

The _custom configuration file_ is written in [Markdown list syntax](/ref/markup-syntax).

### ConEmu Configuration {#config-conemu}

* Description: The configuration for the embedded ConEmu console in the [Dasboard](/ref/dashboard/).
* Path: `config\ConEmu.xml`
* Config Property: [ConEmuConfigFile](/ref/config/#ConEmuConfigFile)
* Typ: file

### Environment Setup Hook {#config-env}

* Description: The hook script for environment setup.
* Path: `config\env.ps1`
* Typ: file

This script is executed, at the end of the Bench environment setup.
Inside of the _environment setup hook script_ is the [PowerShell API](/ref/ps-api/) available.

### Setup Hook {#config-setup}

* Description: The hook script for app setup.
* Path: `configsetup.ps1`
* Typ: file

This script is executed, at the end of the setup of one ore multiple apps.
Inside of the _setup hook script_ is the [PowerShell API](/ref/ps-api/) available.

### Bench Resources Directory {#res-dir}

* Description: This directory contains resources for the Bench system.
* Path: `res`
* Typ: directory

### App Custom Resources Directory {#res-apps-dir}

* Description: This directory contains resources used by custom scripts
  of included apps.
* Path: `res\apps`
* Config Property: [AppResourceBaseDir](/ref/config/#AppResourceBaseDir)
* Typ: directory

### Custom App Library Template {#res-apps-template}

* Description: The template for the [custom app library](#config-apps).
* Path: `res\apps.template.md`
* Config Property: [CustomAppIndexTemplateFile](/ref/config/#CustomAppIndexTemplateFile)
* Typ: file

### App Activation Template {#res-app-activation-template}

* Description: The template for the [app activation list](#config-apps-activated)
* Path: `res\apps-activated.template.txt`
* Config Property: [AppActivationTemplateFile](/ref/config/#AppActivationTemplateFile)
* Typ: file

### App Deactivation Template {#res-app-deactivation-template}

* Description: The template for the [app deactivation list](#config-apps-deactivated)
* Path: `res\apps-deactivated.template.txt`
* Config Property: [AppDeactivationTemplateFile](/ref/config/#AppDeactivationTemplateFile)
* Typ: file

### Bootstrap Batch File {#res-bench-install}

* Description: The installation batch script for setup and upgrade of Bench.
* Path: `res\bench-install.batch`
* Typ: file

### Site Configuration Template {#res-site-config-template}

* Description: The template for the site configuration file.
* Path: `res\bench-site.template.md`
* Config Property: [SiteConfigTemplateFile](/ref/config/#SiteConfigTemplateFile)
* Typ: file

The site configuration file is written in [Markdown list syntax](/ref/markup-syntax).

### ConEmu Configuration Template {#res-conemu-template}

* Description: The template for the [ConEmu configuration](#config-conemu).
* Path: `res\ConEmu.template.xml`
* Config Property: [ConEmuConfigTemplateFile](/ref/config/#ConEmuConfigTemplateFile)
* Typ: file

### Default Configuration {#res-config}

* Description: The default configuration of Bench.
* Path: `res\config.md`
* Typ: file

The default configuration file is written in [Markdown list syntax](/ref/markup-syntax).

### Custom Configuration Template {#res-config-template}

* Description: The template for the [custom configuration](#config-config).
* Path: `res\config.template.md`
* Config Property: [CustomConfigTemplateFile](/ref/config/#CustomConfigTemplateFile)
* Typ: file

### Version File {#res-version}

* Description: A text file with the version number of the Bench release as the only content.
* Path: `res\version.txt`
* Config Property: [VersionFile](/ref/config/#VersionFile)
* Typ: file

### App Installation Directory {#lib-dir}

* Description: This directory contains the app installations.
* Path: `lib`
* Config Property: [LibDir](/ref/config/#LibDir)
* Typ: directory

Every activated app gets installed in it's [target directory](/ref/app-properties/#Dir).
Usually the app target directories are direct sub-folders of the _app installation directory_.
The default value for the [target directory](/ref/app-properties/#Dir) of an app is
its ID in lower case.

### Execution Proxy Directory {#lib-proxies-dir}

* Description: Execution proxy scripts for adorned executables are stored in this directory.
* Path: `lib\_proxies`
* Config Property: [AppAdornmentBaseDir](/ref/config/#AppAdornmentBaseDir)
* Typ: directory

For every adorned executable, a batch file is generated, which runs the actual
executable with registry isolation, pre-run, and post-run scripts.
These execution proxies are stored in a directory named with the apps ID,
and placed in the _execution proxy directory_.

### Launcher Script Directory {#lib-launcher-dir}

* Description: Launcher scripts are stored in this directory.
* Path: `lib\_launcher`
* Config Property: [LauncherScriptDir](/ref/config/#LauncherScriptDir)
* Typ: directory

For every launcher, a batch file is generated, to inject command line arguments
in the shell call and call the execution proxy if necessary.

### Changelog {#changelog}

* Description: The file a description for all changes of Bench throughout the released versions.
* Path: `CHANGELOG.md`
* Typ: file

### Environment Script File {#env}

* Description: The Bench environment file.
* Path: `env.cmd`
* Typ: file

This file is generated as a result of the environment setup of Bench.
It can be called from another batch script to load the Bench environment.

This file sets a number of environment variables according to the Bench configuration.
These environment variables provide information about the Bench system,
provide the isolation from the Windows user profile,
and put the Bench apps on the `PATH`.
The following variables are always set:

* `USERNAME` from [UserName](/ref/config/#UserName)
* `USEREMAIL` from [UserEmail](/ref/config/#UserEmail)
* `BENCH_HOME` the root directory of Bench
* `BENCH_DRIVE` the drive on which the root directory of Bench is stored
* `BENCH_VERSION` from [the version file](#res-version)
* `BENCH_PATH` the directories with executables from the Bench apps

### License {#license}

* Description: The text of the license under which Bench is published.
* Path: `LICENSE.md`
* Typ: file

### Readme {#readme}

* Description: A brief project description of Bench.
* Path: `README.md`
* Typ: file

### Project Archive Directory {#archive-dir}

* Description: When the Bench task for the creation of a project backup is executed,
  it stores an archive file with the project in this directory.
* Path: `archive`
* Config Property: [ProjectArchiveDir](/ref/config/#ProjectArchiveDir)
* Typ: directory

The format of the archive files can be controlled via the configuration property
[ProjectArchiveFormat](/ref/config/#ProjectArchiveFormat).
The name of the archive files in this directory is build with the respective
project name and a timestamp.

### App Resource Cache {#cache-dir}

* Description: This directory contains all downloaded app resources.
* Path: `cache`
* Config Property: [DownloadDir](/ref/config/#DownloadDir)
* Typ: directory

### Home Directory {#home-dir}

* Description: This directory is the isolated user profile root for the Bench environment.
* Path: `home`
* Config Property: [HomeDir](/ref/config/#HomeDir)
* Typ: directory

If the configuration property [OverrideHome](/ref/config/#OverrideHome)
is set to `true`, the environment variables `HOME` and `USERPROFILE`
in the Bench environment are pointing to this directory.

### Local Application Data Directory {#home-app-data-local-dir}

* Description: This directory is the `AppData\Local` in the isolated user profile
  for the Bench environment.
* Path: `home\AppData\Local`
* Config Property: [LocalAppDataDir](/ref/config/#LocalAppDataDir)
* Typ: directory

If the configuration property [OverrideHome](/ref/config/#OverrideHome)
is set to `true`, the environment variable `LOCALAPPDATA`
in the Bench environment is pointing to this directory.

### Application Data Directory {#home-app-data-roaming-dir}

* Description: This directory is the `AppData\Roaming` in the isolated user profile
  for the Bench environment.
* Path: `home\AppData\Roaming`
* Config Property: [AppDataDir](/ref/config/#AppDataDir)
* Typ: directory

If the configuration property [OverrideHome](/ref/config/#OverrideHome)
is set to `true`, the environment variable `APPDATA`
in the Bench environment is pointing to this directory.

### Registry Isolation Directory {#home-registry-isolation-dir}

* Description: In this directory backups of registry keys are stored when the
  registry isolation mechanism is used.
* Path: `home\registry_isolation`
* Config Property: [AppRegistryBaseDir](/ref/config/#AppRegistryBaseDir)
* Typ: directory

### Launcher Directory {#launcher-dir}

* Description: This directory contains Windows shortcuts for all activated apps with a launcher.
* Path: `launcher`
* Config Property: [LauncherDir](/ref/config/#LauncherDir)
* Typ: directory

This directory can be added as a toolbar to the Windows taskbar.
This way the [Bench Dashboard][] and all graphical apps from Bench are directly
available on the Desktop.

### Log Directory {#log-dir}

* Description: For every setup and app installation process a log file is stored in this directory.
* Path: `log`
* Config Property: [LogDir](/ref/config/#LogDir)
* Typ: directory

### Projects Root Directory {#projects-dir}

* Description: This directory is the root for development projects.
* Path: `projects`
* Config Property: [ProjectRootDir](/ref/config/#ProjectRootDir)
* Typ: directory

If a project folder is placed in this directory, Bench knows about it,
and can provide assistance when working on this project in the Bench environment.

### Temporary Directory {#tmp-dir}

* Description: This is the directory for temporary files in the Bench system.
* Path: `tmp`
* Config Property: [TempDir](/ref/config/#TempDir)
* Typ: directory

If the configuration property [OverrideTemp](/ref/config/#OverrideTemp)
is set to `true`, the environment variables `TEMP` and `TMP`
in the Bench environment are pointing to this directory.

### Site Configuration File {#bench-site}

* Description: The site configuration file(s).
* Path: `bench-site.md`
* Config Property: [SiteConfigFileName](/ref/config/#SiteConfigFileName)
* Typ: file

This file can exist multiple times in the Bench root directory and its parents.
All of them are loaded and applied after the [default](#res-config)
and [custom configuration](#config-config) &ndash; from the file system root down
to the Bench root.

**Example**

1. `D:\foo\bar\bench\res\config.md` (Default Configuration)
2. `D:\foo\bar\bench\config\config.md` (Custom Configuration)
3. `D:\foo\bench-site.md` (Site Configuration)
4. `D:\foo\bar\bench\bench-site.md` (Site Configuration)

The configuration files are loaded in the listed order.
If a configuration value appears in multiple configuration files,
the value from the file, loaded latest, wins.

[Bench CLI]: /ref/bench-ctl
[Bench Dashboard]: /ref/dashboard
[Git]: /ref/apps#Git
