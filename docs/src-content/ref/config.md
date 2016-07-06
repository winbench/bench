+++
date = "2016-06-22T13:42:49+02:00"
description = "The properties of the Bench configuration"
draft = true
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

There two groups of configuration properties:
The first group contains properties, which are predefined by the default
configuration, and can _not_ be set in the custom or site configuration.
These properties can not be overridden, because they are used during the
Bench setup process when no custom or site configuration yet exists.
The second group contains properties, which are also predefined by the default
configuration, but _can_ be overridden in the custom or site configuration.

**System Properties**

<!--
#data-table /*/System Properties/*
#column Name: name(.)
#column Data Type: value(Data Type)
#column Default: value(Default)
-->

**Customizable Properties**

<!--
#data-table /*/Customizable Properties/*
#column Name: name(.)
#column Typ: value(Typ)
#column Data Type: value(Data Type)
#column Default: value(Default)
-->

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

* Description: The path to the directory with the custom configuration (`config.md`, `apps-activated.txt`, ...)
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
* Typ: Custom, Site

This property is used to set the environment variable `USER_NAME`
and in custom scripts e.g. from [Git](/ref/apps/#Git).

### UserEmail {#UserEmail}

* Description: The email address of the Bench user.
* Data Type: string
* Default: user@localhost
* Typ: Custom, Site

This property is used to set the environment variable `USER_EMAIL`
and in custom scripts e.g. from [Git](/ref/apps/#Git).

### DownloadDir {#DownloadDir}

* Description: The path to the directory where downloaded app resources are cached.
* Data Type: path
* Default: `cache`
* Typ: Custom, Site

### AppAdornmentBaseDir {#AppAdornmentBaseDir}

* Default: `$LibDir$\_proxies`

### AppRegistryBaseDir {#AppRegistryBaseDir}

* Default: `$HomeDir$\registry_isolation`

### TempDir {#TempDir}

* Default: `tmp`

### LogDir {#LogDir}

* Default: `log`

### HomeDir {#HomeDir}

* Default: `home`

### AppDataDir {#AppDataDir}

* Default: `$HomeDir$\AppData\Roaming`

### LocalAppDataDir {#LocalAppDataDir}

* Default: `$HomeDir$\AppData\Local`

### OverrideHome {#OverrideHome}

* Default: `true`

### OverrideTemp {#OverrideTemp}

* Default: `true`

### IgnoreSystemPath {#IgnoreSystemPath}

* Default: `true`

### ProjectRootDir {#ProjectRootDir}

* Default: `projects`

### ProjectArchiveDir {#ProjectArchiveDir}

* Default: `archive`

### ProjectArchiveFormat {#ProjectArchiveFormat}

* Default: `zip`

### LauncherDir {#LauncherDir}

* Default: `launcher`

### LauncherScriptDir {#LauncherScriptDir}

* Default: `$LibDir$\_launcher`

### WizzardEditCustomConfigBeforeSetup {#WizzardEditCustomConfigBeforeSetup}

* Default: `false`

### WizzardStartAutoSetup {#WizzardStartAutoSetup}

* Default: `true`

### QuickAccessCmd {#QuickAccessCmd}

* Default: `true`

### QuickAccessPowerShell {#QuickAccessPowerShell}

* Default: `false`

### QuickAccessBash {#QuickAccessBash}

* Default: `false`

### EditorApp {#EditorApp}

* Default: `VSCode`

### Website {#Website}

* Default: <http://mastersign.github.io/bench>

[syntax]: /ref/markup-syntax
