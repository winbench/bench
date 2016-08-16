+++
date = "2016-06-22T13:22:33+02:00"
description = "The process of installing or upgrading an app"
title = "App Setup and Upgrade"
weight = 3
+++

[App Types]: /ref/app-types
[Default App]: /ref/app-types/#default
[NuGet Package]: /ref/app-types/#nuget-package
[App Version]: /ref/app-properties/#Version
[App Dependencies]: /ref/app-properties/#Dependencies
[App ArchiveTyp]: /ref/app-properties/#ArchiveTyp
[App SetupTestFile]: /ref/app-properties/#SetupTestFile
[App Launcher]: /ref/app-properties/#AppLauncher
[App Automation Directory]: /ref/file-structure/#auto-apps-dir
[Custom Configuration Directory]: /ref/file-structure/#config-dir
[User App Automation Directory]: /ref/file-structure/#config-apps-dir
[Setup Hook]: /ref/file-structure/#config-setup
[Environment Setup Hook]: /ref/file-structure/#config-env
[Custom Extract Script]: /ref/file-structure/#custom-script-extract
[Custom Setup Script]: /ref/file-structure/#custom-script-setup
[Custom Environment Setup Script]: /ref/file-structure/#custom-script-env
[Custom Removal Script]: /ref/file-structure/#custom-script-remove

An app in Bench is installed according to its [typ][App Types].
Default apps are installed by downloading their app resource file/archive
extracting or copying it into the apps target directory.
Meta apps are installed completely via custom scripts or they
just serve as groups for other apps via their [dependencies][App Dependencies].
Package apps are installed via their package manager.
Custom scripts can extend or override different steps of the setup process.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

## App Lifecycle
The lifecycle of an app generally breaks down into the following steps.
Not all app types support or need all steps.

1. Download app resource
   (only for [default apps][default app])
2. Extracting the app resource archive
   (only for [default apps][default app] with archive resource,
   can be overridden by a [custom extract script][])
3. Setup of the resources by copying to the target directory or installation
   via package manager
   (can be followed by a [custom setup script][])
4. Setup of the execution adornment proxies and the launchers
5. Environment setup for configuration depending on the proxy settings,
   the absolute path of the Bench root directory, or other parameters
   that may change after setting up the app
   (this step can be performed multiple times, whenever the environment changes,
   implemented by a [custom environment setup script][]])
6. Uninstalling the app
   (can be preceded by a [custom removal script][])
7. Deleting cached resources

Upgrading an app generally means, uninstalling the app, deleting cached resources,
and installing it again.
Apps which are defined with a fixed version number in their definition
can not really be upgraded without changing their definition.
But apps without a specific version number or a version range,
can be upgraded in that way.

## Dependencies and Order
Before an app can be installed, all of its dependencies must be installed.
This is necessary to allow custom setup scripts of an app
depend on the apps dependencies to be installed.
If an app with dependencies is installed in a task, then the task is automatically
extended to cover all dependencies not installed yet.
The same is true the other way around for the uninstallation of an app.
Before an app is uninstalled, all apps that depend on this app
are uninstalled before the app is uninstalled.

If multiple apps are installed or uninstalled in one task, the steps, e.g.
_download_, _setup_, and _environment setup_, can be performed consolidated.
Meaning first all _downloads_ are performed, then all _setups_ are performed,
and at last the _environment setup_ for all affected apps is performed.
Multiple apps are always installed in the order their ID is first listed in
the application libraries &ndash; first Bench app library, then user app library.

## Custom Scripts and Hooks {#custom-scripts}
Simple [default apps][default app] with XCOPY deployment or
package apps, usually do not need custom scripts.
But many Windows programs require some additional setup or configuration
steps to work properly.
That includes e.g. moving or deleting files extracted from the apps resource
archive, writing configuration files, even executing programs for initialization.

For an app included in the Bench system, the custom scripts are stored in the
[app automation directory][].
For an app defined in the user app library, the custom scripts are stored
in the subfolder `apps` of the [custom configuration directory][].
Additional automation, not directly associated with a specific app
can be implemented in the hooks for [setup][setup hook] and
[environment setup][environment setup hook].

## Details for Different App Types {#details}
In the following sections, specifics of the setup process for the different
app types are explained.

### Meta Apps {#typ-meta}
_Meta Apps_ do not have an app resource, which could be downloaded
and installed, via the install mechanisms Bench provides.
And it can not be installed via a package manager, which is supported by Bench.

Therefore, all setup steps &ndash; given that there are some &ndash;
have to be performed by the custom scripts or hooks, respectively.
Because _Meta Apps_ do not have resources, downloaded by Bench,
their setup process consists only of the
[setup step for the app resources][custom setup script])
and the [setup step for the environment][custom environment setup script].
When uninstalling or upgrading _Meta Apps_, the [uninstall step][custom remove]
is used.

### Default Apps {#typ-default}
_Default Apps_ are programs, which consist of at least one file,
which is executable by Windows.
This can be an `*.exe`, a `*.cmd`, or a `*.bat` file.
A _Default App_ has a resource file or archive which can be downloaded via a HTTP(S) URL.
If the app has resource file, usually the resource file itself is the executable.
If the app has a resource archive, the executable can be one of many files.

#### Installation {#typ-default-install}
A _Default App_ can be installed, if it is not installed already.

The installation of a _Default App_ is performed by the following steps:

* Download the resource file or archive if not cached
* **If** the resource is an archive file:
    + **If** the [`ArchiveTyp`][App ArchiveTyp] is `auto` or `custom` and
      a [custom extract script][] `<app-id>.extract.ps1` is found:
      Run the custom script to extract the archive content.
    + **Else if** the [`ArchiveTyp`][App ArchiveTyp] is `auto`:
      Select an extractor by the filename extension and extract the archive.
    + **Else if** the [`ArchiveTyp`][App ArchiveTyp] is `msi`:
      Use _LessMSI_ to extract the archive content.
    + **Else if** the [`ArchiveTyp`][App ArchiveTyp] is `inno`:
      Use _InnoUnpacker_ to extract the archive content.
    + **Else if** the [`ArchiveTyp`][App ArchiveTyp] is `generic`:
      Use _7-Zip_ if installed, else use _DotNetZip_ library to extract the archive content.
* **Else**
 Copy the resource file to the apps target directory.
* Run the [custom setup script][] `<app-id>.setup.ps1` if one is found.
* Run the [setup hook script][Setup Hook] if it exists.
  If multiple apps are installed in one task, the hook script is only run once at the end.
* Create execution adornment proxies if needed.
* Create launcher scripts and shortcuts if the [App Launcher][] property is set.
* Run the [custom environment setup script][] `<app-id>.env.ps1` if one is found.
* Run the [environment setup hook script][Environment Setup Hook] if it exists.
  If multiple apps are installed in one task, the hook script is only run once at the end.

To check if a _Default App_ is already installed, the [`SetupTestFile`][App SetupTestFile]
is checked for existence.

#### Upgrade {#typ-default-upgrade}
A _Default App_ can be upgraded, if it is installed,
and its [`Version`][App Version] is empty or set to `latest`.

The upgrade of an _Default App_ is performed by [removing](#typ-default-uninstall) the app,
deleting its app resource in case it is cached,
and [installing](#typ-default-install) the app again.

#### Uninstalling {#typ-default-uninstall}
A _Default App_ can be uninstalled, if it is installed.

The removal of an app is performed by the following steps:

* Run the [custom removal script][] `<app-id>.remove.ps1` if one is found.
* Delete the apps target directory and all of its content.

### Package Apps {#typ-package}
_Package Apps_ are apps, which are managed by some kind of package manager.
Examples are _npm_ for _Node.js_ or _PIP_ for _Python_.

#### Installation {#typ-package-install}
A _Package App_ can be installed, if it is not installed already.

The installation of a _Package App_ is performed by the following steps:

* Execute the package manager in the installation mode,  
  e.g. `npm install package-xyz@3.1.2 --global`.
* Run the [custom setup script][] `<app-id>.setup.ps1` if one is found.
* Run the [setup hook script][Setup Hook] if it exists.
  If multiple apps are installed in one task, the hook script is only run once at the end.
* Create execution adornment proxies if needed.
* Create launcher scripts and shortcuts if the [App Launcher][] property is set.
* Run the [custom environment setup script][] `<app-id>.env.ps1` if one is found.
* Run the [environment setup hook script][Environment Setup Hook] if it exists.
  If multiple apps are installed in one task, the hook script is only run once at the end.

#### Upgrade {#typ-default-upgrade}
An _Package App_ can be upgraded, if it is installed.

The upgrade of an app is performed by [removing](#typ-package-uninstall) the app,
deleting its app resource in case it is cached, and [installing](#typ-package-install)
the app again.

#### Uninstallation {#typ-package-uninstall}
A _Package App_ can be uninstalled, if it is installed.

The uninstallation of a _Package App_ is performed by the following steps:

* Run the [custom removal script][] `<app-id>.remove.ps1` if one is found.
* Remove the installed package
    + If it is a [NuGet package][]:
      Deleting the app target directory with all its content.
    + Otherwise:
      Execute the package manager in the uninstallation mode,  
      e.g. `npm remove package-xyz --global`.

Notice: To remove the _Launcher_ for the app, the environment setup must be executed.
