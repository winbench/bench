+++
date = "2016-06-22T13:42:49+02:00"
description = "The properties of the Bench configuration"
title = "Configuration Properties"
weight = 6
+++

Configuration properties for Bench use the [list syntax in Markdown][syntax]
and can be defined in the following files.

* Default configuration file `res\config.md`
* Custom configuration file `config\config.md`
* Site configuration files `bench-site.md`
  in the Bench root directory and all of its parents.

The configuration files are applied in the order they are listed above.
The site configuration files are applied with the files near the file system root first
and the one in the Bench root directory last.
Configuration files applied later override values from files applied earlier.

Therefore, the site configuration file in the Bench root directory has the highest priority
and the default configuration has the lowest.

Properties of the data type `path` can be absolute, or relative to the Bench root directory.

## Overview

There are two groups of configuration properties:
The first group contains properties, which are predefined by the default
configuration, and can _not_ be set in the custom or site configuration.
These properties can not be overridden, because they are used during the
Bench setup process when no custom or site configuration yet exists.
The second group contains properties, which are also predefined by the default
configuration, but _can_ be overridden in the custom or site configuration.

**System Properties**

| Name | Data Type | Default |
|------|-----------|---------|
| [VersionFile](#VersionFile) | path | `res\version.txt` |
| [CustomConfigDir](#CustomConfigDir) | path | `config` |
| [CustomConfigFile](#CustomConfigFile) | path | `$CustomConfigDir$\config.md` |
| [CustomConfigTemplateFile](#CustomConfigTemplateFile) | path | `res\config.template.md` |
| [SiteConfigFileName](#SiteConfigFileName) | string | `bench-site.md` |
| [SiteConfigTemplateFile](#SiteConfigTemplateFile) | path | `res\bench-site.template.md` |
| [AppIndexFile](#AppIndexFile) | path | `res\apps.md` |
| [AppActivationFile](#AppActivationFile) | path | `$CustomConfigDir$\apps-activated.txt` |
| [AppActivationTemplateFile](#AppActivationTemplateFile) | path | `res\apps-activated.template.txt` |
| [AppDeactivationFile](#AppDeactivationFile) | path | `$CustomConfigDir$\apps-deactivated.txt` |
| [AppDeactivationTemplateFile](#AppDeactivationTemplateFile) | path | `res\apps-deactivated.template.txt` |
| [CustomAppIndexFile](#CustomAppIndexFile) | path | `$CustomConfigDir$\apps.md` |
| [CustomAppIndexTemplateFile](#CustomAppIndexTemplateFile) | path | `res\apps.template.md` |
| [ConEmuConfigFile](#ConEmuConfigFile) | path | `$CustomConfigDir$\ConEmu.xml` |
| [ConEmuConfigTemplateFile](#ConEmuConfigTemplateFile) | path | `res\ConEmu.template.xml` |
| [AppVersionIndexDir](#AppVersionIndexDir) | path | `$LibDir$\_versions` |
| [AppResourceBaseDir](#AppResourceBaseDir) | path | `res\apps` |
| [ActionDir](#ActionDir) | path | `actions` |
| [LibDir](#LibDir) | path | `lib` |
| [Website](#Website) | URL | <http://mastersign.github.io/bench> |
| [WizzardEditCustomConfigBeforeSetup](#WizzardEditCustomConfigBeforeSetup) | boolean | `false` |
| [WizzardStartAutoSetup](#WizzardStartAutoSetup) | boolean | `true` |

**Customizable Properties**

| Name | Typ | Data Type | Default |
|------|-----|-----------|---------|
| [UseProxy](#UseProxy) | Site | boolean | `false` |
| [ProxyBypass](#ProxyBypass) | Site | list of strings | `localhost` |
| [HttpProxy](#HttpProxy) | Site | URL | `http://127.0.0.1:80` |
| [HttpsProxy](#HttpsProxy) | Site | URL | `http://127.0.0.1:443` |
| [DownloadAttempts](#DownloadAttempts) | Site | integer | 3 |
| [ParallelDownloads](#ParallelDownloads) | Site | integer | 4 |
| [UserName](#UserName) | Custom/Site | string | user |
| [UserEmail](#UserEmail) | Custom/Site | string | user@localhost |
| [DownloadDir](#DownloadDir) | Custom/Site | path | `cache` |
| [AppAdornmentBaseDir](#AppAdornmentBaseDir) | Custom | path | `$LibDir$\_proxies` |
| [AppRegistryBaseDir](#AppRegistryBaseDir) | Custom | path | `$HomeDir$\registry_isolation` |
| [TempDir](#TempDir) | Custom/Site | path | `tmp` |
| [LogDir](#LogDir) | Custom | path | `log` |
| [HomeDir](#HomeDir) | Custom/Site | path | `home` |
| [AppDataDir](#AppDataDir) | Custom/Site | path | `$HomeDir$\AppData\Roaming` |
| [LocalAppDataDir](#LocalAppDataDir) | Custom/Site | path | `$HomeDir$\AppData\Local` |
| [OverrideHome](#OverrideHome) | Custom/Site | boolen | `true` |
| [OverrideTemp](#OverrideTemp) | Custom/Site | boolean | `true` |
| [IgnoreSystemPath](#IgnoreSystemPath) | Custom/Site | boolean | `true` |
| [ProjectRootDir](#ProjectRootDir) | Custom/Site | path | `projects` |
| [ProjectArchiveDir](#ProjectArchiveDir) | Custom/Site | path | `archive` |
| [ProjectArchiveFormat](#ProjectArchiveFormat) | Custom/Site | string | `zip` |
| [LauncherDir](#LauncherDir) | Custom | path | `launcher` |
| [LauncherScriptDir](#LauncherScriptDir) | Custom | path | `$LibDir$\_launcher` |
| [QuickAccessCmd](#QuickAccessCmd) | Custom/Site | boolean | `true` |
| [QuickAccessPowerShell](#QuickAccessPowerShell) | Custom/Site | boolean | `false` |
| [QuickAccessBash](#QuickAccessBash) | Custom/Site | boolean | `false` |
| [EditorApp](#EditorApp) | Custom/Site | string | `VSCode` |

## System Properties

Properties in this group can not be customized, by overriding them in
the custom or site configuration.

If it is necessary to change them anyways, the default configuration
in `res\config.md` must be edited before the initial Bench setup is executed,
and the Bench upgrade mechanism can not be used, because it would override
these changes.

### VersionFile {#VersionFile}

* Description: The path to a text file, which contains nothing but the version number of the Bench distribution.
* Data Type: path
* Default: `res\version.txt`
* Typ: System

### CustomConfigDir {#CustomConfigDir}

* Description: The path to the directory with the custom configuration (`config.md`, `apps-activated.txt`, ...) is stored.
* Data Type: path
* Default: `config`
* Typ: System

The custom configuration directory is designed in a way, that is can be easily put under version control.

### CustomConfigFile {#CustomConfigFile}

* Description: The path to the custom configuration file.
* Data Type: path
* Default: `$CustomConfigDir$\config.md`
* Typ: System

The specified file must be a Markdown file and follow the [Markdown list syntax][syntax].

### CustomConfigTemplateFile {#CustomConfigTemplateFile}

* Description: The path to the custom configuration template file,
  which is copied during the Bench setup in case no custom configuration exists.
* Data Type: path
* Default: `res\config.template.md`
* Typ: System

### SiteConfigFileName {#SiteConfigFileName}

* Description: The name of the site configuration file.
* Data Type: string
* Default: `bench-site.md`
* Typ: System

Site configuration files are searched in the Bench root directory and in all of its parents.
The specified file must be a Markdown file and follow the [Markdown list syntax][syntax].

### SiteConfigTemplateFile {#SiteConfigTemplateFile}

* Description: The path to the site configuration template file,
  which is copied during the Bench setup in case no site configuration exists.
* Data Type: path
* Default: `res\bench-site.template.md`
* Typ: System

### AppIndexFile {#AppIndexFile}

* Description: The path to a library file for all program definitions, included in Bench.
* Data Type: path
* Default: `res\apps.md`
* Typ: System

The specified file must be a Markdown file and follow the [Markdown list syntax][syntax].

### AppActivationFile {#AppActivationFile}

* Description: The path to a file with a list of activated apps.
* Data Type: path
* Default: `$CustomConfigDir$\apps-activated.txt`
* Typ: System

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a `#` is interpreted as an app ID.
Only non-space characters, up to the first space or the end of a line, are considered.

### AppActivationTemplateFile {#AppActivationTemplateFile}

* Description: The path to the app activation template file,
  which is copied during the Bench setup in case no custom configuration exists.
* Data Type: path
* Default: `res\apps-activated.template.txt`
* Typ: System

### AppDeactivationFile {#AppDeactivationFile}

* Description: The path to a file with a list of deactivated apps.
* Data Type: path
* Default: `$CustomConfigDir$\apps-deactivated.txt`
* Typ: System

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a `#` is interpreted as an app ID.
Only non-space characters, up to the first space or the end of a line, are considered.

### AppDeactivationTemplateFile {#AppDeactivationTemplateFile}

* Description: The path to the app deactivation template file,
  which is copied during the Bench setup in case no custom configuration exists.
* Data Type: path
* Default: `res\apps-deactivated.template.txt`
* Typ: System

### CustomAppIndexFile {#CustomAppIndexFile}

* Description: The path to a library file with custom program definitions.
* Data Type: path
* Default: `$CustomConfigDir$\apps.md`
* Typ: System

The specified file must be a Markdown file and follow the [Markdown list syntax][syntax].

### CustomAppIndexTemplateFile {#CustomAppIndexTemplateFile}

* Description: The path to the custom app library template file,
  which is copied during the Bench setup in case no custom configuration exists.
* Data Type: path
* Default: `res\apps.template.md`
* Typ: System

### ConEmuConfigFile {#ConEmuConfigFile}

* Description: The path to the ConEmu configuration, used in the Bench Dashboard.
* Data Type: path
* Default: `$CustomConfigDir$\ConEmu.xml`
* Typ: System

### ConEmuConfigTemplateFile {#ConEmuConfigTemplateFile}

* Description: The path to the ConEmu configuration template file,
  which is copied during the Bench setup in case no custom configuration exists.
* Data Type: path
* Default: `res\ConEmu.template.xml`
* Typ: System

### AppVersionIndexDir {#AppVersionIndexDir}

* Description: The directory to store the currently installed version numbers of the apps.
* Data Type: path
* Default: `$LibDir$\_versions`
* Typ: System

### AppResourceBaseDir {#AppResourceBaseDir}

* Description: The path to a directory, containing additional resource files,
  which are used during the execution of custom setup scripts.
* Data Type: path
* Default: `res\apps`
* Typ: System

It is used from custom scripts to retrieve absolute paths to the additional resources.

### ActionDir {#ActionDir}

* Description: The path to a directory with Bench action scripts.
* Data Type: path
* Default: `actions`
* Typ: System

Bench action scripts are typically `*.cmd` files.

### LibDir {#LibDir}

* Description: The path to the base directory where Bench apps are installed.
* Data Type: path
* Default: `lib`
* Typ: System

### Website {#Website}

* Description: The URL for the Bench documentation.
* Data Type: URL
* Default: <http://mastersign.github.io/bench>
* Typ: System

### WizzardEditCustomConfigBeforeSetup {#WizzardEditCustomConfigBeforeSetup}

* Description: A temporary switch which is used during the Bench setup process.
* Data Type: boolean
* Default: `false`
* Typ: Temporary

### WizzardStartAutoSetup {#WizzardStartAutoSetup}

* Description: A temporary switch which is used during the Bench setup process.
* Data Type: boolean
* Default: `true`
* Typ: Temporary

## Customizable Properties

Properties in this group are customizable and can be set in
`config/config.md` or in a `bench-site.md` file.

### UseProxy {#UseProxy}

* Description: A switch to activate the use of a HTTP(S) proxy in Bench.
* Data Type: boolean
* Possible Values: `true`, `false`
* Default: `false`
* Typ: Site

### ProxyBypass {#ProxyBypass}

* Description: A list with domains which are not to be contacted via proxy.
* Data Type: list of strings
* Default: `localhost`
* Typ: Site

This property is used during the download of app resources and interpreted
by the [`System.Net.WebProxy` class](https://msdn.microsoft.com/en-us/library/system.net.webproxy.bypasslist.aspx).

The usage in the custom scripts for apps is not completely implemented yet.

### HttpProxy {#HttpProxy}

* Description: The URL for the HTTP proxy.
* Data Type: URL
* Default: `http://127.0.0.1:80`
* Typ: Site

### HttpsProxy {#HttpsProxy}

* Description: The URL for the HTTPS proxy.
* Data Type: URL
* Default: `http://127.0.0.1:443`
* Typ: Site

### DownloadAttempts {#DownloadAttempts}

* Description: The number of download attempts for an app resource.
* Data Type: integer
* Default: 3
* Typ: Site

### ParallelDownloads {#ParallelDownloads}

* Description: The maximal number of parallel downloads for app resources.
* Data Type: integer
* Default: 4
* Typ: Site

### UserName {#UserName}

* Description: The name of the Bench user.
* Data Type: string
* Default: user
* Typ: Custom/Site

This property is used to set the environment variable `USER_NAME`
and in custom scripts e.g. from [Git](/ref/apps/#Git).

### UserEmail {#UserEmail}

* Description: The email address of the Bench user.
* Data Type: string
* Default: user@localhost
* Typ: Custom/Site

This property is used to set the environment variable `USER_EMAIL`
and in custom scripts e.g. from [Git](/ref/apps/#Git).

### DownloadDir {#DownloadDir}

* Description: The path to the directory where downloaded app resources are cached.
* Data Type: path
* Default: `cache`
* Typ: Custom/Site

### AppAdornmentBaseDir {#AppAdornmentBaseDir}

* Description: The path to the directory where wrapper scripts are stored,
  which allow the adornment of an app execution.
* Data Type: path
* Default: `$LibDir$\_proxies`
* Typ: Custom

### AppRegistryBaseDir {#AppRegistryBaseDir}

* Description: The path to the directory where registry backups
  for the app isolation mechanism are stored.
* Data Type: path
* Default: `$HomeDir$\registry_isolation`
* Typ: Custom

### TempDir {#TempDir}

* Description: The path to the temporary directory of the Bench environment.
* Data Type: path
* Default: `tmp`
* Typ: Custom/Site

### LogDir {#LogDir}

* Description: The path to the directory where Bench setup logs are stored.
* Data Type: path
* Default: `log`
* Typ: Custom

### HomeDir {#HomeDir}

* Description: The path to the user profile of the Bench environment.
* Data Type: path
* Default: `home`
* Typ: Custom/Site

### AppDataDir {#AppDataDir}

* Description: The path to the `AppData\Roaming` directory in the user profile
  of the Bench environment.
* Data Type: path
* Default: `$HomeDir$\AppData\Roaming`
* Typ: Custom/Site

### LocalAppDataDir {#LocalAppDataDir}

* Description: The path to the `AppData\Local` directory in the user profile
  of the Bench environment.
* Data Type: path
* Default: `$HomeDir$\AppData\Local`
* Typ: Custom/Site

### OverrideHome {#OverrideHome}

* Description: A switch to control if the environment variables `HOME`, `HOMEPATH`, `HOMEDRIVE`, `USERPROFILE`, `APPDATA`, and `LOCALAPPDATA` are overridden in the Bench environment.
* Data Type: boolen
* Default: `true`
* Typ: Custom/Site

### OverrideTemp {#OverrideTemp}

* Description: A switch to control if the environment variables `TEMP` and `TMP` are overridden in the Bench environment.
* Data Type: boolean
* Default: `true`
* Typ: Custom/Site

### IgnoreSystemPath {#IgnoreSystemPath}

* Description: A switch to control if only Bench tools are on the `PATH` in the Bench environment or if the `PATH` of the current Windows user is preserved.
* Data Type: boolean
* Default: `true`
* Typ: Custom/Site

### ProjectRootDir {#ProjectRootDir}

* Description: The path to the directory where projects are stored.
* Data Type: path
* Default: `projects`
* Typ: Custom/Site

### ProjectArchiveDir {#ProjectArchiveDir}

* Description: The path to the directory where project backups are stored.
* Data Type: path
* Default: `archive`
* Typ: Custom/Site

### ProjectArchiveFormat {#ProjectArchiveFormat}

* Description: The file format of the project backups.
* Data Type: string
* Possible Values: `zip`, `7z`, `tar`, `wim`
* Default: `zip`
* Typ: Custom/Site

### LauncherDir {#LauncherDir}

* Description: The path to the directory where launcher shortcuts are stored.
* Data Type: path
* Default: `launcher`
* Typ: Custom

### LauncherScriptDir {#LauncherScriptDir}

* Description: The path to the directory where launcher wrapper scripts are stored.
* Data Type: path
* Default: `$LibDir$\_launcher`
* Typ: Custom

### QuickAccessCmd {#QuickAccessCmd}

* Description: A switch which controls if a launcher for the Windows command interpreter (CMD) is created.
* Data Type: boolean
* Default: `true`
* Typ: Custom/Site

### QuickAccessPowerShell {#QuickAccessPowerShell}

* Description: A switch which controls if a launcher for the PowerShell is created.
* Data Type: boolean
* Default: `false`
* Typ: Custom/Site

### QuickAccessBash {#QuickAccessBash}

* Description: A switch which controls if a launcher for the Git Bash is created.
* Data Type: boolean
* Default: `false`
* Typ: Custom/Site

This switch only takes affect, if the [Git app](/ref/apps/#Git) is activated.

### EditorApp {#EditorApp}

* Description: The ID of an app which is used as the default text editor.
* Data Type: string
* Default: `VSCode`
* Typ: Custom/Site

[syntax]: /ref/markup-syntax
