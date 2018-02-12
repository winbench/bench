+++
date = "2016-06-22T13:42:49+02:00"
description = "The properties of the Bench configuration"
title = "Configuration Properties"
weight = 6
+++

[syntax]: /ref/markup-syntax

Configuration properties for Bench use the [list syntax in Markdown][syntax]
and can be defined in the following files.

* Default configuration file `res\config.md`
* User configuration file `config\config.md`
* Site configuration files `bench-site.md`
  in the Bench root directory and all of its parents.

The configuration files are applied in the order they are listed above.
The site configuration files are applied with the files near the file system root first
and the one in the Bench root directory last.
Configuration files applied later, override values from files applied earlier.

Therefore, the site configuration file in the Bench root directory has the highest priority
and the default configuration has the lowest.

Properties of the data type `path` can be absolute, or relative to the Bench root directory.

## Overview

There are two groups of configuration properties:
The first group contains properties, which are predefined by the default
configuration, and can _not_ be set in the user or site configuration.
These properties can not be overridden, because they are used during the
Bench setup process when no user or site configuration yet exists.
The second group contains properties, which are also predefined by the default
configuration, but _can_ be overridden in the user or site configuration.

**System Properties**

| Name | Data Type | Default |
|------|-----------|---------|
| [VersionFile](#VersionFile) | path | `res\version.txt` |
| [VersionUrl](#VersionUrl) | url | <https://github.com/winbench/bench/raw/master/res/version.txt> |
| [UpdateUrlTemplate](#UpdateUrlTemplate) | string | `https://github.com/winbench/bench/releases/download/v#VERSION#/Bench.zip` |
| [BootstrapUrlTemplate](#BootstrapUrlTemplate) | string | `https://github.com/winbench/bench/raw/v#VERSION#/res/bench-install.bat` |
| [UserConfigDir](#UserConfigDir) | path | `config` |
| [UserConfigFile](#UserConfigFile) | path | `$UserConfigDir$\config.md` |
| [UserConfigTemplateFile](#UserConfigTemplateFile) | path | `res\config.template.md` |
| [SiteConfigFileName](#SiteConfigFileName) | string | `bench-site.md` |
| [SiteConfigTemplateFile](#SiteConfigTemplateFile) | path | `res\bench-site.template.md` |
| [AppLibs](#AppLibs) | dictionary | `core: github:mastersign/bench-apps-core` |
| [AppLibsInstallDir](#AppLibsInstallDir) | path | `$LibDir$\applibs` |
| [AppLibIndexFileName](#AppLibIndexFileName) | string | `apps.md` |
| [AppLibCustomScriptDirName](#AppLibCustomScriptDirName) | string | `res` |
| [AppLibResourceDirName](#AppLibResourceDirName) | string | `res` |
| [AppActivationFile](#AppActivationFile) | path | `$UserConfigDir$\apps-activated.txt` |
| [AppActivationTemplateFile](#AppActivationTemplateFile) | path | `res\apps-activated.template.txt` |
| [AppDeactivationFile](#AppDeactivationFile) | path | `$UserConfigDir$\apps-deactivated.txt` |
| [AppDeactivationTemplateFile](#AppDeactivationTemplateFile) | path | `res\apps-deactivated.template.txt` |
| [UserAppIndexTemplateFile](#UserAppIndexTemplateFile) | path | `res\apps.template.md` |
| [ConEmuConfigFile](#ConEmuConfigFile) | path | `$UserConfigDir$\ConEmu.xml` |
| [ConEmuConfigTemplateFile](#ConEmuConfigTemplateFile) | path | `res\ConEmu.template.xml` |
| [LibDir](#LibDir) | path | `lib` |
| [AppsInstallDir](#AppsInstallDir) | path | `$LibDir$\apps` |
| [Website](#Website) | URL | <https://winbench.org> |
| [Use64Bit](#Use64Bit) | boolean | automatically determined |
| [WizzardApps](#WizzardApps) | dictionary | groups from the default app library |
| [WizzardSelectedApps](#WizzardSelectedApps) | list | empty |
| [WizzardStartAutoSetup](#WizzardStartAutoSetup) | boolean | `true` |

**Customizable Properties**

| Name | Type | Data Type | Default |
|------|------|-----------|---------|
| [AutoUpdateCheck](#AutoUpdateCheck) | User | boolean | `true` |
| [UseProxy](#UseProxy) | Site | boolean | `false` |
| [ProxyBypass](#ProxyBypass) | Site | list of strings | `localhost` |
| [HttpProxy](#HttpProxy) | Site | URL | `http://127.0.0.1:80` |
| [HttpsProxy](#HttpsProxy) | Site | URL | `http://127.0.0.1:443` |
| [DownloadAttempts](#DownloadAttempts) | Site | integer | 3 |
| [ParallelDownloads](#ParallelDownloads) | Site | integer | 4 |
| [UserName](#UserName) | User/Site | string | user |
| [UserEmail](#UserEmail) | User/Site | string | user@localhost |
| [KnownLicenses](#KnownLicenses) | User/Site | dictionary | A selection from <https://spdx.org/licenses/> |
| [AppsVersionIndexDir](#AppsVersionIndexDir) | User/Site | path | `$LibDir$\versions` |
| [CacheDir](#CacheDir) | User/Site | path | `cache` |
| [AppLibsCacheDir](#AppLibsDownloadDir) | User/Site | path | `$CacheDir$\applibs` |
| [AppsCacheDir](#AppsCacheDir) | User/Site | path | `$CacheDir$\apps` |
| [AppsAdornmentBaseDir](#AppsAdornmentBaseDir) | User | path | `$LibDir$\proxies` |
| [AppsRegistryBaseDir](#AppsRegistryBaseDir) | User | path | `$HomeDir$\registry_isolation` |
| [TempDir](#TempDir) | User/Site | path | `tmp` |
| [LogDir](#LogDir) | User | path | `log` |
| [HomeDir](#HomeDir) | User/Site | path | `home` |
| [AppDataDir](#AppDataDir) | User/Site | path | `$HomeDir$\AppData\Roaming` |
| [LocalAppDataDir](#LocalAppDataDir) | User/Site | path | `$HomeDir$\AppData\Local` |
| [OverrideHome](#OverrideHome) | User/Site | boolean | `true` |
| [OverrideTemp](#OverrideTemp) | User/Site | boolean | `true` |
| [IgnoreSystemPath](#IgnoreSystemPath) | User/Site | boolean | `true` |
| [RegisterInUserProfile](#RegisterInUserProfile) | User | boolean | `false` |
| [UseRegistryIsolation](#UseRegistryIsolation) | User | boolean | `true` |
| [CustomPath](#CustomPath) | User/Site | string list | empty |
| [CustomEnvironment](#CustomEnvironment) | User/Site | dictionary | empty |
| [Allow64Bit](#Allow64Bit) | User/Site | boolean | `false` |
| [ProjectRootDir](#ProjectRootDir) | User/Site | path | `projects` |
| [LauncherDir](#LauncherDir) | User | path | `launcher` |
| [LauncherScriptDir](#LauncherScriptDir) | User | path | `$LibDir$\launcher` |
| [QuickAccessCmd](#QuickAccessCmd) | User/Site | boolean | `true` |
| [QuickAccessPowerShell](#QuickAccessPowerShell) | User/Site | boolean | `false` |
| [QuickAccessBash](#QuickAccessBash) | User/Site | boolean | `false` |
| [DashboardSetupAppListColumns](#DashboardSetupAppListColumns) | User/Site | string list | `Order`, `Label`, `Version`, `Active`, `Deactivated`, `Status`, `Typ`, `Comment` |
| [TextEditorApp](#TextEditorApp) | User/Site | string | `Bench.Notepad2` |
| [MarkdownEditorApp](#MarkdownEditorApp) | User/Site | string | `Bench.MarkdownEdit` |

## System Properties

Properties in this group can not be customized, by overriding them in
the user or site configuration.

If it is necessary to change them anyways, the default configuration
in `res\config.md` must be edited before the initial Bench setup is executed,
and the Bench upgrade mechanism can not be used, because it would override
these changes.

### VersionFile {#VersionFile}

* Description: The path to a text file, which contains nothing but the version number of the Bench distribution.
* Data Type: path
* Default: `res\version.txt`
* Type: System

### VersionUrl {#VersionUrl}

* Description: The URL to retrieve the version number of the latest Bench release.
* Data Type: url
* Default: <https://github.com/winbench/bench/raw/master/res/version.txt>
* Type: System

### UpdateUrlTemplate {#UpdateUrlTemplate}

* Description: The URL template to generate an URL for retrieving a Bench system update.
* Data Type: string
* Default: `https://github.com/winbench/bench/releases/download/v#VERSION#/Bench.zip`
* Type: System

The placeholder `#VERSION#` in the URL template will be replaced
by the version number of the Bench system update,
to generate the actual update URL.

The update is expected to be a ZIP file, containing the Bench system files.

### BootstrapUrlTemplate {#BootstrapUrlTemplate}

* Description: The URL template to generate an URL for retrieveing the bootstrap script file.
* Data Type: string
* Default: `https://github.com/winbench/bench/raw/v#VERSION#/res/bench-install.bat`
* Type: System

The placeholder `#VERSION#` in the URL template will be replaced
by the version number of the targeted Bench release,
to generate the actual URL.

### UserConfigDir {#UserConfigDir}

* Description: The path to the directory with the user configuration (`config.md`, `apps-activated.txt`, ...) is stored.
* Data Type: path
* Default: `config`
* Type: System

The user configuration directory is designed in a way, that is can be easily put under version control.

### UserConfigFile {#UserConfigFile}

* Description: The path to the user configuration file.
* Data Type: path
* Default: `$UserConfigDir$\config.md`
* Type: System

The specified file must be a Markdown file and follow the [Markdown list syntax][syntax].

### UserConfigTemplateFile {#UserConfigTemplateFile}

* Description: The path to the user configuration template file,
  which is copied during the Bench setup in case no user configuration exists.
* Data Type: path
* Default: `res\config.template.md`
* Type: System

### SiteConfigFileName {#SiteConfigFileName}

* Description: The name of the site configuration file.
* Data Type: string
* Default: `bench-site.md`
* Type: System

Site configuration files are searched in the Bench root directory and in all of its parents.
The specified file must be a Markdown file and follow the [Markdown list syntax][syntax].

### SiteConfigTemplateFile {#SiteConfigTemplateFile}

* Description: The path to the site configuration template file,
  which is copied during the Bench setup in case no site configuration exists.
* Data Type: path
* Default: `res\bench-site.template.md`
* Type: System

### AppLibs {#AppLibs}

* Description: A table with URLs of app libraries to load in the Bench environment.
* Data Type: dictionary
* Default: `core: github:mastersign/bench-apps-core`
* Type: User

The table consists of [key/value pairs](/ref/markup-syntax/#lists-and-dictionaries).
Where the key is a unique ID for the app library inside the Bench environment,
and the value is an URL to a ZIP file with the app library.
The order of the table entries dictates the order in which the app libraries are loaded.

The URL can use one of the following protocols: `http`, `https`, `file`.
If the protocol `file` is used, the URL can refer to a ZIP file
or just a directory containing the app library.
If the app library is hosted as a GitHub repository, a short form for the URL
can be used: `github:<user or organization>/<repository name>`;
which is automatically expanded to an URL with the `https` protocol.

The default value of the base configuration contains only apps, which
are required or directly known by Bench. This value should be overridden
in the user or site configuration, to include more app libraries.

For starters the following list of app libraries is advised:

```Markdown
* `AppLibs`:
    + `core`: `github:mastersign/bench-apps-core`
    + `default`: `github:mastersign/bench-apps-default`
```

### AppLibsInstallDir {#AppLibsInstallDir}

* Description: The path of the directory, where to load the app libraries.
* Data Type: path
* Default: `$LibDir$\applibs`
* Type: System

### AppLibIndexFileName {#AppLibIndexFileName}

* Description: The name of the index file in an app library.
* Data Type: string
* Default: `apps.md`
* Type: System

The specified file must be a Markdown file and follow the [Markdown list syntax][syntax].

### AppLibCustomScriptDirName {#AppLibCustomScriptDirName}

* Description: The name of the directory with the custom scripts in an app library.
* Data Type: string
* Default: `res`
* Type: System

It is used from custom scripts to retrieve paths to app resources during the setup.

### AppLibResourceDirName {#AppLibResourceDirName}

* Description: The name of the directory with additional setup resources in an app library.
* Data Type: string
* Default: `res`
* Type: System

It is used from custom scripts to retrieve paths to resources, e.g. during the app setup.

### AppActivationFile {#AppActivationFile}

* Description: The path to a file with a list of activated apps.
* Data Type: path
* Default: `$UserConfigDir$\apps-activated.txt`
* Type: System

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a `#` is interpreted as an app ID.
Only non-space characters, up to the first space or the end of a line, are considered.

### AppActivationTemplateFile {#AppActivationTemplateFile}

* Description: The path to the app activation template file,
  which is copied during the Bench setup in case no user configuration exists.
* Data Type: path
* Default: `res\apps-activated.template.txt`
* Type: System

### AppDeactivationFile {#AppDeactivationFile}

* Description: The path to a file with a list of deactivated apps.
* Data Type: path
* Default: `$UserConfigDir$\apps-deactivated.txt`
* Type: System

The specified file must be an UTF8 encoded text file.
Every non empty line, which is not commented with a `#` is interpreted as an app ID.
Only non-space characters, up to the first space or the end of a line, are considered.

### AppDeactivationTemplateFile {#AppDeactivationTemplateFile}

* Description: The path to the app deactivation template file,
  which is copied during the Bench setup in case no user configuration exists.
* Data Type: path
* Default: `res\apps-deactivated.template.txt`
* Type: System

### UserAppIndexTemplateFile {#UserAppIndexTemplateFile}

* Description: The path to the user app library template file,
  which is copied during the Bench setup in case no user configuration exists.
* Data Type: path
* Default: `res\apps.template.md`
* Type: System

### ConEmuConfigFile {#ConEmuConfigFile}

* Description: The path to the ConEmu configuration, used in the Bench Dashboard.
* Data Type: path
* Default: `$UserConfigDir$\ConEmu.xml`
* Type: System

### ConEmuConfigTemplateFile {#ConEmuConfigTemplateFile}

* Description: The path to the ConEmu configuration template file,
  which is copied during the Bench setup in case no user configuration exists.
* Data Type: path
* Default: `res\ConEmu.template.xml`
* Type: System

### LibDir {#LibDir}

* Description: The path to the base directory for files downloaded and installed by Bench.
* Data Type: path
* Default: `lib`
* Type: System

### AppsInstallDir {#AppsInstallDir}

* Description: The path to the base directory where Bench apps are installed.
* Data Type: path
* Default: `$LibDir$\apps`
* Type: System

### Website {#Website}

* Description: The URL for the Bench documentation.
* Data Type: URL
* Default: <https://winbench.org>
* Type: System

### Use64Bit {#Use64Bit}

* Description: The runtime decision if the 64Bit binaries of the apps will be used.
* Data Type: boolean
* Default: automatically determined
* Type: Runtime

See [`Allow64Bit`](#Allow64Bit).

### WizzardApps {#WizzardApps}

* Description: A list of apps and groups to offer for activation during the user configuration initialization.
* Data Type: dictionary
* Default: groups from the default app library
* Type: System

The items of the dictionary must have the format `<Label>: <App ID>`.
The following Markdown would be a proper setting for this property:

```Markdown
* WizzardApps:
    + `Group A`: `Bench.Group.A`
    + `Group B`: `Bench.Group.B`
```

### WizzardSelectedApps {#WizzardSelectedApps}

* Description: A list with app IDs selected for activation, when initializing a new user configuration.
* Data Type: list
* Default: empty
* Type: Temporary

### WizzardStartAutoSetup {#WizzardStartAutoSetup}

* Description: A temporary switch which is used during the Bench setup process.
* Data Type: boolean
* Default: `true`
* Type: Temporary

## Customizable Properties

Properties in this group are customizable and can be set in
`config/config.md` or in a `bench-site.md` file.

### AutoUpdateCheck {#AutoUpdateCheck}

* Description: A flag to control whether the Bench system is automatically checking for updates or not.
* Data Type: boolean
* Default: `true`
* Type: User

### UseProxy {#UseProxy}

* Description: A switch to activate the use of a HTTP(S) proxy in Bench.
* Data Type: boolean
* Possible Values: `true`, `false`
* Default: `false`
* Type: Site

### ProxyBypass {#ProxyBypass}

* Description: A list with domains which are not to be contacted via proxy.
* Data Type: list of strings
* Default: `localhost`
* Type: Site

This property is used during the download of app resources and interpreted
by the [`System.Net.WebProxy` class](https://msdn.microsoft.com/en-us/library/system.net.webproxy.bypasslist.aspx).

The usage in the custom scripts for apps is not completely implemented yet.

### HttpProxy {#HttpProxy}

* Description: The URL for the HTTP proxy.
* Data Type: URL
* Default: `http://127.0.0.1:80`
* Type: Site

### HttpsProxy {#HttpsProxy}

* Description: The URL for the HTTPS proxy.
* Data Type: URL
* Default: `http://127.0.0.1:443`
* Type: Site

### DownloadAttempts {#DownloadAttempts}

* Description: The number of download attempts for an app resource.
* Data Type: integer
* Default: 3
* Type: Site

### ParallelDownloads {#ParallelDownloads}

* Description: The maximal number of parallel downloads for app resources.
* Data Type: integer
* Default: 4
* Type: Site

### UserName {#UserName}

* Description: The name of the Bench user.
* Data Type: string
* Default: user
* Type: User/Site

This property is used to set the environment variable `USER_NAME`
and in custom scripts e.g. from [Git](/apps/Bench.Git).

### UserEmail {#UserEmail}

* Description: The email address of the Bench user.
* Data Type: string
* Default: user@localhost
* Type: User/Site

This property is used to set the environment variable `USER_EMAIL`
and in custom scripts e.g. from [Git](/apps/Bench.Git).

### KnownLicenses {#KnownLicenses}

* Description: A dictionary with SPDX license identifiers associated with a URL.
* Data Type: dictionary
* Default: A selection from <https://spdx.org/licenses/>
* Type: User/Site

If the app property `License` is set to an SPDX identifier listed in this
dictionary, the app property `LicenseUrl` defaults to the associated URL.

### AppsVersionIndexDir {#AppsVersionIndexDir}

* Description: The directory to store the currently installed version numbers of the apps.
* Data Type: path
* Default: `$LibDir$\versions`
* Type: User/Site

### CacheDir {#CacheDir}

* Description: The path to the directory where downloaded files are cached.
* Data Type: path
* Default: `cache`
* Type: User/Site

### AppLibsCacheDir {#AppLibsDownloadDir}

* Description: The path of the directory, where downloaded app libraries are cached.
* Data Type: path
* Default: `$CacheDir$\applibs`
* Type: User/Site

### AppsCacheDir {#AppsCacheDir}

* Description: The path to the directory where downloaded app resources are cached.
* Data Type: path
* Default: `$CacheDir$\apps`
* Type: User/Site

### AppsAdornmentBaseDir {#AppsAdornmentBaseDir}

* Description: The path to the directory where wrapper scripts are stored,
  which allow the adornment of app executions.
* Data Type: path
* Default: `$LibDir$\proxies`
* Type: User

### AppsRegistryBaseDir {#AppsRegistryBaseDir}

* Description: The path to the directory where registry backups
  for the app isolation mechanism are stored.
* Data Type: path
* Default: `$HomeDir$\registry_isolation`
* Type: User

### TempDir {#TempDir}

* Description: The path to the temporary directory of the Bench environment.
* Data Type: path
* Default: `tmp`
* Type: User/Site

### LogDir {#LogDir}

* Description: The path to the directory where Bench setup logs are stored.
* Data Type: path
* Default: `log`
* Type: User

### HomeDir {#HomeDir}

* Description: The path to the user profile of the Bench environment.
* Data Type: path
* Default: `home`
* Type: User/Site

### AppDataDir {#AppDataDir}

* Description: The path to the `AppData\Roaming` directory in the user profile
  of the Bench environment.
* Data Type: path
* Default: `$HomeDir$\AppData\Roaming`
* Type: User/Site

### LocalAppDataDir {#LocalAppDataDir}

* Description: The path to the `AppData\Local` directory in the user profile
  of the Bench environment.
* Data Type: path
* Default: `$HomeDir$\AppData\Local`
* Type: User/Site

### OverrideHome {#OverrideHome}

* Description: A switch to control if the environment variables `HOME`, `HOMEPATH`, `HOMEDRIVE`, `USERPROFILE`, `APPDATA`, and `LOCALAPPDATA` are overridden in the Bench environment.
* Data Type: boolean
* Default: `true`
* Type: User/Site

### OverrideTemp {#OverrideTemp}

* Description: A switch to control if the environment variables `TEMP` and `TMP` are overridden in the Bench environment.
* Data Type: boolean
* Default: `true`
* Type: User/Site

### IgnoreSystemPath {#IgnoreSystemPath}

* Description: A switch to control if only Bench tools are on the `PATH` in the Bench environment or if the `PATH` of the current Windows user is preserved.
* Data Type: boolean
* Default: `true`
* Type: User/Site

### RegisterInUserProfile {#RegisterInUserProfile}

* Description: A switch to control if the Bench apps are registered in the current
  Windows user profile.
* Data Type: boolean
* Default: `false`
* Type: User

### UseRegistryIsolation {#UseRegistryIsolation}

* Description: A switch to activate the registry isolation mechanism for
  adorned executables.
* Data Type: boolean
* Default: `true`
* Type: User

### CustomPath {#CustomPath}

* Description: A list with additional directories, to put on the `PATH` environment variable.
* Data Type: string list
* Default: empty
* Type: User/Site

### CustomEnvironment {#CustomEnvironment}

* Description: A dictionary with additional environment variables.
* Data Type: dictionary
* Default: empty
* Type: User/Site

### Allow64Bit {#Allow64Bit}

* Description: A flag to allow download and installation of 64Bit binaries
  on a 64Bit Windows operating system.
* Data Type: boolean
* Default: `false`
* Type: User/Site

If set to `true` Bench is checking the operating system it is running on,
and if it is a 64Bit Windows, it is using the 64Bit alternatives
in the app properties to download and install 64Bit binaries.
See [`Use64Bit`](#Use64Bit).

**Warning:** This property should be left to `false`, if the Bench
environment is used as a portable environment on different machines,
and it is unclear if all machines support 64Bit code.

### ProjectRootDir {#ProjectRootDir}

* Description: The path to the directory where projects are stored.
* Data Type: path
* Default: `projects`
* Type: User/Site

<!--
### ProjectArchiveDir {#ProjectArchiveDir}

* Description: The path to the directory where project backups are stored.
* Data Type: path
* Default: `archive`
* Type: User/Site

### ProjectArchiveFormat {#ProjectArchiveFormat}

* Description: The file format of the project backups.
* Data Type: string
* Possible Values: `zip`, `7z`, `tar`, `wim`
* Default: `zip`
* Type: User/Site
-->

### LauncherDir {#LauncherDir}

* Description: The path to the directory where launcher shortcuts are stored.
* Data Type: path
* Default: `launcher`
* Type: User

### LauncherScriptDir {#LauncherScriptDir}

* Description: The path to the directory where launcher wrapper scripts are stored.
* Data Type: path
* Default: `$LibDir$\launcher`
* Type: User

### QuickAccessCmd {#QuickAccessCmd}

* Description: A switch which controls if a launcher for the Windows command interpreter (CMD) is created.
* Data Type: boolean
* Default: `true`
* Type: User/Site

### QuickAccessPowerShell {#QuickAccessPowerShell}

* Description: A switch which controls if a launcher for the PowerShell is created.
* Data Type: boolean
* Default: `false`
* Type: User/Site

### QuickAccessBash {#QuickAccessBash}

* Description: A switch which controls if a launcher for the Git Bash is created.
* Data Type: boolean
* Default: `false`
* Type: User/Site

This switch only takes affect, if the [Git app](/apps/Bench.Git) is activated.

### DashboardSetupAppListColumns {#DashboardSetupAppListColumns}

* Description: A list with the columns to display in the setup dialog of the _BenchDashboard_.
* Data Type: string list
* Default: `Order`, `Label`, `Version`, `Active`, `Deactivated`, `Status`, `Typ`, `Comment`
* Type: User/Site

### TextEditorApp {#TextEditorApp}

* Description: The ID of an app which is used as the default text editor.
* Data Type: string
* Default: `Bench.Notepad2`
* Type: User/Site

The default text editor is used to edit the app activation list.

### MarkdownEditorApp {#MarkdownEditorApp}

* Description: The ID of an app which is used as the default Markdown editor.
* Data Type: string
* Default: `Bench.MarkdownEdit`
* Type: User/Site

The default Markdown editor is used to edit configuration files.
