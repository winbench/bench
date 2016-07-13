+++
date = "2016-06-22T13:22:33+02:00"
description = "The process of installing or upgrading an app"
draft = true
title = "App Setup and Upgrade"
weight = 3
+++

[App Types]: /ref/app-types
[App Version]: /ref/app-properties/#Version
[App Dependencies]: /ref/app-properties/#Dependencies
[App ArchiveTyp]: /ref/app-properties/#ArchiveTyp
[App SetupTestFile]: /ref/app-properties/#SetupTestFile
[App Automation Directory]: /ref/file-structure/#auto-apps-dir
[Setup Hook]: /ref/file-structure/#config-setup
[Environment Setup Hook]: /ref/file-structure/#config-env
[Custom Extract Script]: /ref/file-structure/#auto-apps-extract
[Custom Setup Script]: /ref/file-structure/#auto-apps-setup
[Custom Environment Setup Script]: /ref/file-structure/#auto-apps-env
[Custom Removal Script]: /ref/file-structure/#auto-apps-remove

An app in Bench is installed according to its [typ][App Types].
Default apps are installed by downloading their app resource file/archive
extracting or copying it into the apps target directory.
Meta apps are installed completely via custom scripts or they
just serve as groups for other apps via their [dependencies][App Dependencies].
Package apps are installed via their package manager.
Custom scripts can extend or override different steps of the setup process.
<!--more-->

If an app is included in the Bench system, its automation
is implemented as custom scripts and is stored in the [app automation directory][].
If an app is defined in the custom app library, its automation
is implemented in the hooks for [setup][setup hook] and
[environment setup][environment setup hook].

## Meta Apps {#typ-meta}

Meta apps do not have an app resource, which could be downloaded
and installed, via the install mechanisms Bench provides.
And it can not be installed via a package manager, which is supported by Bench.

Therefore, all setup steps &ndash; given that there are some &ndash;
have to be performed by the custom scripts or hooks, respectively
for setup and environment setup.

TODO

## Default Apps {#typ-default}
Default apps are programs, which consist of at least one file,
which is executable by Windows.
This can be an `*.exe`, a `*.cmd`, or a `*.bat` file.
A default app has a resource file or archive which can be downloaded via a HTTP(S) URL.
If the app has resource file, usually the resource file itself is the executable.
If the app has a resource archive, the executable can be one of many files.

### Installation {#typ-default-install}
The installation of a default app is performed by the following steps:

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
  If multiple apps are installed in one task, the hook script is only run one at the end.
* Run the [custom environment setup script][] `<app-id>.env.ps1` if one is found.
* Run the [environment setup hook script][Environment Setup Hook] if it exists.
  If multiple apps are installed in one task, the hook script is only run one at the end.

Before an app can be installed, all of its dependencies must be installed.
This is necessary to allow custom setup scripts of an app
depend on the apps dependencies to be installed.
If an app with dependencies is installed in a task, then the task is automatically
extended to cover all dependencies not installed yet.

If multiple apps are installed in one task, the steps _download_, _setup_,
and _environment setup_ can be performed consolidated.
Meaning first all _downloads_ are performed, then all _setups_ are performed,
and at last the _environment setup_ for all affected apps is performed.
Multiple apps are always installed in the order their ID is first listed in
the application libraries (first internal, then custom).

To check if an app is already installed, the [`SetupTestFile`][App SetupTestFile]
is checked for existance.

### Upgrade {#typ-default-upgrade}
An app can be upgraded, if it is installed, and its [`Version`][App Version]
is empty or set to `latest`.

The upgrade of an app is performed by [removing](#typ-default-uninstall) the app,
deleting its app resource in case it is cached, and [installing](#typ-default-install)
the app afterwards.

### Uninstalling {#typ-default-uninstall}
An app can be uninstalled, if it is installed.

The removal of an app is performed in the following steps:

* Run the [custom removal script][] `<app-id>.remove.ps1` if it exists.
* Delete the apps target directory and all of its content.

## Package Apps {#typ-package}
Package apps are apps, which are managed by some kind of package manager.
Examples are _npm_ for _Node.js_ or _PIP_ for _Python_.

TODO
