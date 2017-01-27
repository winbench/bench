+++
date = "2017-01-02T16:30:00+02:00"
description = "The layout of the folders and files in the Bench environment"
title = "File Structure"
weight = 3
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

* [`auto`](#auto-dir) Bench Automation
    + [`bin`](#auto-bin-dir) Bench Binaries
        - [`bench.exe`](/ref/bench-cli/)
        - [`BenchDashboard.exe`](/ref/dashboard/)
        - [`BenchLib.dll`](/ref/clr-api/)
        - [`bench-cmd.cmd`](#auto-bin-bench-cmd)
        - [`bench-ps.cmd`](#auto-bin-bench-ps)
        - [`bench-bash.cmd`](#auto-bin-bench-bash)
        - ...
    + [`lib`](#auto-lib-dir) Bench Scripts
        - `bench.lib.ps1`
        - ...
* [`config`](#config-dir) User Configuration
  ([CustomConfigDir](/ref/config/#CustomConfigDir))
    + [`scripts`](/ref/app-library/#scripts-dir) Custom Scripts Directory
    + [`res`](/ref/app-library/res-dir) App Setup Resources Directory
    + [`apps.md`](/ref/app-library/apps-file) App Index of the User App Library
    + [`apps-activated.txt`](#config-apps-activated) App Activation List
      ([AppActivationFile](/ref/config/#AppActivationFile))
    + [`apps-deactivated.txt`](#config-apps-deactivated) App Deactivation List
      ([AppDeactivationFile](/ref/config/#AppDeactivationFile))
    + [`config.md`](#config-config) User Configuration File
      ([CustomConfigFile](/ref/config/#CustomConfigFile))
    + [`ConEmu.xml`](#config-conemu)
      ([ConEmuConfigFile](/ref/config/#ConEmuConfigFile))
    + [`env.ps1`](#config-env) Environment Setup Hook
    + [`setup.ps1`](#config-setup) Setup Hook
* [`res`](#res-dir) Bench Resources
    + [`apps.template.md`](#res-apps-template)
      ([CustomAppIndexTemplateFile](/ref/config/#CustomAppIndexTemplateFile))
    + [`apps-activated.template.txt`](#res-app-activation-template)
      ([AppActivationTemplateFile](/ref/config/#AppActivationTemplateFile))
    + [`apps-deactivated.template.txt`](#res-app-deactivation-template)
      ([AppDeactivationTemplateFile](/ref/config/#AppDeactivationTemplateFile))
    + [`bench-install.bat`](#res-bench-install) Bootstrap Batch File
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
     + [`_applibs`](#lib-applibs-dir)
       ([AppLibsDir](/ref/config/#AppLibsDir))
     + [`_proxies`](#lib-proxies-dir)
       ([AppAdornmentBaseDir](/ref/config/#AppAdornmentBaseDir))
     + [`_launcher`](#lib-launcher-dir)
       ([LauncherScriptDir](/ref/config/#LauncherScriptDir))
     + [`_versions`](#lib-versions-dir)
       ([AppVersionIndexDir](/ref/config/#AppVersionIndexDir))
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
    + [`_applibs`](#cache-applibs-dir) Downloaded App Libraries
      ([AppLibsDownloadDir](/ref/config/#AppLibsDownloadDir))
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

### Bench Automation Directory {#auto-dir}

* Description: The base directory for the Bench scripts and binaries.
* Path: `auto`
* Type: directory

### Bench Binary Directory {#auto-bin-dir}

* Description: The directory with all binary executables and libraries of Bench.
* Path: `auto\bin`
* Type: directory

### Action `bench-bash` {#auto-bin-bench-bash}

* Description: Starts a [Git][] shell in the Bench environment.
* Path: `auto\bin\bench-bash.cmd`
* Type: file

This action will fail if [Git][] is not installed

### Action `bench-cmd` {#auto-bin-bench-cmd}

* Description: Starts a Windows CMD console in the Bench environment.
* Path: `auto\bin\bench-cmd.cmd`
* Type: file

### Action `bench-ps` {#auto-bin-bench-ps}

* Description: Starts a PowerShell console in the Bench environment.
* Path: `auto\bin\bench-ps.cmd`
* Type: file

### Bench Script Directory {#auto-lib-dir}

* Description: The directory with the PowerShell scripts of Bench.
* Path: `auto\lib`
* Type: directory

### User Configuration Directory {#config-dir}

* Description: The directory for the user configuration.
* Path: `config`
* Config Property: [CustomConfigDir](/ref/config/#CustomConfigDir)
* Type: directory

This directory is designed to be put under version control,
to manage and share Bench configurations.

The user configuration directory contains the user configuration
and is the user [app library](/ref/app-library) at the same time.

### App Activation List {#config-apps-activated}

* Description: The list of activated apps.
* Path: `config\apps-activated.txt`
* Config Property: [AppActivationFile](/ref/config/#AppActivationFile)
* Type: file

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a `#` is interpreted
as an app ID.
Only non-space characters, up to the first space or the end of a line,
are considered.

### App Deactivation List {#config-apps-deactivated}

* Description: The list of deactivated apps.
* Path: `config\apps-deactivated.txt`
* Config Property: [AppDeactivationFile](/ref/config/#AppDeactivationFile)
* Type: file

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a `#` is interpreted
as an app ID.
Only non-space characters, up to the first space or the end of a line,
are considered.

### User Configuration File {#config-config}

* Description: The user configuration file.
* Path: `config\config.md`
* Config Property: [CustomConfigFile](/ref/config/#CustomConfigFile)
* Type: file

The _user configuration file_ is written in [Markdown list syntax](/ref/markup-syntax).

### ConEmu Configuration {#config-conemu}

* Description: The configuration for the embedded ConEmu console in the [Dasboard](/ref/dashboard/).
* Path: `config\ConEmu.xml`
* Config Property: [ConEmuConfigFile](/ref/config/#ConEmuConfigFile)
* Type: file

### Environment Setup Hook {#config-env}

* Description: The hook script for environment setup.
* Path: `config\env.ps1`
* Type: file

This script is executed, at the end of the Bench environment setup.
Inside of the _environment setup hook script_ is the [PowerShell API](/ref/ps-api/) available.

### Setup Hook {#config-setup}

* Description: The hook script for app setup.
* Path: `configsetup.ps1`
* Type: file

This script is executed, at the end of the setup of one ore multiple apps.
Inside of the _setup hook script_ is the [PowerShell API](/ref/ps-api/) available.

### Bench Resources Directory {#res-dir}

* Description: This directory contains resources for the Bench system.
* Path: `res`
* Type: directory

### Custom App Library Template {#res-apps-template}

* Description: The template for the [custom app library](#config-apps).
* Path: `res\apps.template.md`
* Config Property: [CustomAppIndexTemplateFile](/ref/config/#CustomAppIndexTemplateFile)
* Type: file

### App Activation Template {#res-app-activation-template}

* Description: The template for the [app activation list](#config-apps-activated)
* Path: `res\apps-activated.template.txt`
* Config Property: [AppActivationTemplateFile](/ref/config/#AppActivationTemplateFile)
* Type: file

### App Deactivation Template {#res-app-deactivation-template}

* Description: The template for the [app deactivation list](#config-apps-deactivated)
* Path: `res\apps-deactivated.template.txt`
* Config Property: [AppDeactivationTemplateFile](/ref/config/#AppDeactivationTemplateFile)
* Type: file

### Bootstrap Batch File {#res-bench-install}

* Description: The installation batch script for setup and upgrade of Bench.
* Path: `res\bench-install.batch`
* Type: file

This file is the _installation program_ for Bench.
The latest version can be downloaded from:
<https://github.com/mastersign/bench/raw/master/res/bench-install.bat>.

### Site Configuration Template {#res-site-config-template}

* Description: The template for the site configuration file.
* Path: `res\bench-site.template.md`
* Config Property: [SiteConfigTemplateFile](/ref/config/#SiteConfigTemplateFile)
* Type: file

The site configuration file is written in [Markdown list syntax](/ref/markup-syntax).

### ConEmu Configuration Template {#res-conemu-template}

* Description: The template for the [ConEmu configuration](#config-conemu).
* Path: `res\ConEmu.template.xml`
* Config Property: [ConEmuConfigTemplateFile](/ref/config/#ConEmuConfigTemplateFile)
* Type: file

### Default Configuration {#res-config}

* Description: The default configuration of Bench.
* Path: `res\config.md`
* Type: file

The default configuration file is written in [Markdown list syntax](/ref/markup-syntax).

### Custom Configuration Template {#res-config-template}

* Description: The template for the [custom configuration](#config-config).
* Path: `res\config.template.md`
* Config Property: [CustomConfigTemplateFile](/ref/config/#CustomConfigTemplateFile)
* Type: file

### Version File {#res-version}

* Description: A text file with the version number of the Bench release as the only content.
* Path: `res\version.txt`
* Config Property: [VersionFile](/ref/config/#VersionFile)
* Type: file

### App Installation Directory {#lib-dir}

* Description: This directory contains the app installations.
* Path: `lib`
* Config Property: [LibDir](/ref/config/#LibDir)
* Type: directory

Every activated app gets installed in it's [target directory](/ref/app-properties/#Dir).
Usually the app target directories are direct sub-folders of the _app installation directory_.
The default value for the [target directory](/ref/app-properties/#Dir) of an app is
its ID in lower case.

### App Library Load Directory {#lib-applibs-dir}

* Description: This diectory is used to load the app libraries.
* Path: `lib\_applibs`
* Config Property: [AppLibsDir](/ref/config/#AppLibsDir)
* Type: directory

App libraries are loaded as sub-directories, named by their
ID from the [AppLibs](/ref/config/#AppLibs) table.

### Execution Proxy Directory {#lib-proxies-dir}

* Description: Execution proxy scripts for adorned executables are stored in this directory.
* Path: `lib\_proxies`
* Config Property: [AppAdornmentBaseDir](/ref/config/#AppAdornmentBaseDir)
* Type: directory

For every adorned executable, a batch file is generated, which runs the actual
executable with registry isolation, pre-run, and post-run scripts.
These execution proxies are stored in a directory named with the apps ID,
and placed in the _execution proxy directory_.

### Launcher Script Directory {#lib-launcher-dir}

* Description: Launcher scripts are stored in this directory.
* Path: `lib\_launcher`
* Config Property: [LauncherScriptDir](/ref/config/#LauncherScriptDir)
* Type: directory

For every launcher, a batch file is generated, to inject command line arguments
in the shell call and call the execution proxy if necessary.

### Version Index Directory {#lib-versions-dir}

* Description: The version numbers of the currently installed apps are stored here.
* Path: `lib\_versions`
* Config Property: [AppVersionIndexDir](/ref/config/#AppVersionIndexDir)
* Type: directory

When Bench installes an app, it stores the version number from the app property
[Version](/ref/app-properties/#Version) in a text file, named with the apps ID.
This stored version number is used determine, if the app definition was updated
since the installation of the app.

### Changelog {#changelog}

* Description: A file with a description for all changes of Bench throughout the released versions.
* Path: `CHANGELOG.md`
* Type: file

### Environment Script File {#env}

* Description: The Bench environment file.
* Path: `env.cmd`
* Type: file

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
* Type: file

### Readme {#readme}

* Description: A brief project description of Bench.
* Path: `README.md`
* Type: file

### Project Archive Directory {#archive-dir}

* Description: When the Bench task for the creation of a project backup is executed,
  it stores an archive file with the project in this directory.
* Path: `archive`
* Config Property: [ProjectArchiveDir](/ref/config/#ProjectArchiveDir)
* Type: directory

The format of the archive files can be controlled via the configuration property
[ProjectArchiveFormat](/ref/config/#ProjectArchiveFormat).
The name of the archive files in this directory is build with the respective
project name and a timestamp.

### App Resource Cache {#cache-dir}

* Description: This directory contains all downloaded app resources.
* Path: `cache`
* Config Property: [DownloadDir](/ref/config/#DownloadDir)
* Type: directory

### App Libary Download Cache {#cache-applibs-dir}

* Description: This directory contains all downloaded app libraries.
* Path: `cache\_applibs`
* Config Property: [AppLibsDownloadDir](/ref/config/#AppLibsDownloadDir)
* Type: directory

### Home Directory {#home-dir}

* Description: This directory is the isolated user profile root for the Bench environment.
* Path: `home`
* Config Property: [HomeDir](/ref/config/#HomeDir)
* Type: directory

If the configuration property [OverrideHome](/ref/config/#OverrideHome)
is set to `true`, the environment variables `HOME` and `USERPROFILE`
in the Bench environment are pointing to this directory.

### Local Application Data Directory {#home-app-data-local-dir}

* Description: This directory is the `AppData\Local` in the isolated user profile
  for the Bench environment.
* Path: `home\AppData\Local`
* Config Property: [LocalAppDataDir](/ref/config/#LocalAppDataDir)
* Type: directory

If the configuration property [OverrideHome](/ref/config/#OverrideHome)
is set to `true`, the environment variable `LOCALAPPDATA`
in the Bench environment is pointing to this directory.

### Application Data Directory {#home-app-data-roaming-dir}

* Description: This directory is the `AppData\Roaming` in the isolated user profile
  for the Bench environment.
* Path: `home\AppData\Roaming`
* Config Property: [AppDataDir](/ref/config/#AppDataDir)
* Type: directory

If the configuration property [OverrideHome](/ref/config/#OverrideHome)
is set to `true`, the environment variable `APPDATA`
in the Bench environment is pointing to this directory.

### Registry Isolation Directory {#home-registry-isolation-dir}

* Description: In this directory backups of registry keys are stored when the
  registry isolation mechanism is used.
* Path: `home\registry_isolation`
* Config Property: [AppRegistryBaseDir](/ref/config/#AppRegistryBaseDir)
* Type: directory

### Launcher Directory {#launcher-dir}

* Description: This directory contains Windows shortcuts for all activated apps with a launcher.
* Path: `launcher`
* Config Property: [LauncherDir](/ref/config/#LauncherDir)
* Type: directory

This directory can be added as a toolbar to the Windows taskbar.
This way the [Bench Dashboard][] and all graphical apps from Bench are directly
available on the Desktop.

### Log Directory {#log-dir}

* Description: For every setup and app installation process a log file is stored in this directory.
* Path: `log`
* Config Property: [LogDir](/ref/config/#LogDir)
* Type: directory

### Projects Root Directory {#projects-dir}

* Description: This directory is the root for development projects.
* Path: `projects`
* Config Property: [ProjectRootDir](/ref/config/#ProjectRootDir)
* Type: directory

If a project folder is placed in this directory, Bench knows about it,
and can provide assistance when working on this project in the Bench environment.

### Temporary Directory {#tmp-dir}

* Description: This is the directory for temporary files in the Bench system.
* Path: `tmp`
* Config Property: [TempDir](/ref/config/#TempDir)
* Type: directory

If the configuration property [OverrideTemp](/ref/config/#OverrideTemp)
is set to `true`, the environment variables `TEMP` and `TMP`
in the Bench environment are pointing to this directory.

### Site Configuration File {#bench-site}

* Description: The site configuration file(s).
* Path: `bench-site.md`
* Config Property: [SiteConfigFileName](/ref/config/#SiteConfigFileName)
* Type: file

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

[Bench CLI]: /ref/bench-cli
[Bench Dashboard]: /ref/dashboard
[Git]: /apps/Bench.Git
