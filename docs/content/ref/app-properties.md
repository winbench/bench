+++
date = "2016-06-22T13:43:28+02:00"
description = "The properties for the definition of an app"
draft = true
title = "App Properties"
weight = 7
+++

## Common Properties

These properties are recognized by all [app types].

* **ID**:
  The ID of the app.
* **Label**:
  A user friendly name for the app. 
  (optional, default is the apps ID)
* **Typ**:
  The application typ
  (optional, default is `default`).
* **Dependencies**:
  A list with the IDs of all apps in this app group.
* **Force**:
  A boolean, indicating if the package should allways be installed,
  even if it is already installed
  (optional, default is `false`).
* **Dir**:
  The name of the target directory for the app
  (optional, default is the app ID in lowercase).
* **Path**:
  A list of relative paths inside the app directory to register in the environment `PATH`
  (optional, default is `.`).
  This property is only recognized, if `Register` is `true`.
* **Register**:
  A boolean to indicate if the path(s) of the application should be added to the environment `PATH`
  (optional, default is `true`).
* **Environment**:
  A list of key-value-pairs, describing additional environment variables
  (optional, default is empty).
  E.g. `MY_APP_HOME: $MyApp:Dir$`, `MY_APP_LOG: D:\logs\myapp.log`
* **Exe**:
  The path of the main executable for this app, relative to the target directory of the app
  (optional, default is `<app ID>.exe`).
* **AdornedExecutables**:
  A list of executable paths, relative to the target directory of the app.
  Every listed executable will be adorned with pre- and post-execution scripts.
  (optional, default is empty)
* **RegistryKeys**:
  A list of relative key paths in the Windows registry hive `HKEY_CURRENT_USER`,
  which are used by this app and must be backed up and restored,
  during execution of this app.
  (optional, default is empty)
* **Launcher**:
  A label for the app launcher
  (optional, default is empty).
  A launcher for the app is created only if this property is set to a non empty string.
* **LauncherExecutable**:
  The path to the executable targeted by the app launcher,
  absolute or relative to the target directory of the app
  (optional, default is the `Exe` property).
* **LauncherArguments**:
  A list with arguments to the app executable
  (optional, default is `%*`).
* **LauncherIcon**:
  The path to the icon of the launcher,
  absolute or relative to the target directory of the app
  (optional, default is the executable).
  
### Default Windows App

These properties are recognized by all [default windows apps].

* **Url**:
  The URL to the file, containing the app binaries
* **DownloadCookies**:
  A list of cookies, to send with the download request
  (optional, default is empty)
* **ResourceName**:
  The name of the downloaded file
  (only for directly executable downloads like `*.exe` or `*.cmd`).
* **ArchiveName**:
  The name of the downloaded archive
  (for archives which need to be extracted like `*.zip`, `*.msi`, or setup programs).
* **ArchiveTyp**:
  The archive typ, which yields to the extractor selection
  (optional, default is `auto`).
  Possible values are:
    + `auto` Try to determine the extractor by the filename extension or use the custom extractor script if it exists
    + `generic` Use 7-Zip to extract
    + `msi` Use LessMSI to extract
    + `inno` Use Inno Setup Unpacker to extract
    + `custom` Use the custom script `auto\apps\<app ID>.extract.ps1`
* **ArchivePath**:
  A sub folder in the archive to extract
  (optional, default is the archive root).
* **SetupTestFile**:
  The existence of this file is used, to determine if the app is already installed
  (optional, default is the value of the property `App-Exe`).

Some restrictions for the properties:

* The properties _ResourceName_ and _ArchiveName_ are mutually exclusive.
* The property _ArchivePath_ is only recognized, if _ArchiveName_ is used.

### Node.js Package

These properties are recognized by [Node.js packages].

* **Typ**:
  The application typ
  (required to be `node-package`).
* **PackageName**:
  The name of the NPM package to install via NPM
  (optional, default is the app ID in lowercase).
* **Version**:
  The package version or version range to install (e.g. `2.5.0` or `>=1.2.0 <3.0.0`),
  if empty install latest
  (optional, default empty).
* **Exe**:
  The name of an NPM CLI wrapper from this package
  (optional, default is empty).
* **Path**:
  This property is ignored for Node.js packages.

### Python Package

These properties are recognizes by [Python packages].

* **Typ**:
  The application typ
  (required to be `python2-package` or `python3-package`)
* **PackageName**:
  The name of the PyPI package to install via PIP
  (optional, default is the app ID in lowercase).
* **Version**:
  The package version or version range to install (e.g. `2.5.0` or `>=1.2.0,<3.0.0`),
  if empty install latest
  (optional, default empty).
* **Exe**:
  The name of an PIP CLI wrapper from this package
  (optional, default is empty).
* **Path**:
  This property is ignored for Python packages.

### Ruby Package

These properties are recognized by [Ruby packages].

* **Typ**:
  The application typ
  (required to be `ruby-package`).
* **PackageName**:
  The name of the Ruby package to install via Gem
  (optional, default is the app ID in lowercase).
* **Version**:
  The package version or version range to install (e.g. `2.5.0`),
  if empty install latest
  (optional, default empty).
* **Exe**:
  The name of an Gem CLI wrapper from this package
  (optional, default is empty).
* **Path**:
  This property is ignored for Ruby packages.

[app types]: /ref/app-types/
[meta apps]: /ref/app-types/#meta-apps
[default windows apps]: /ref/app-types/#default-windows-app
[Node.js packages]: /ref/app-types/#node-js-package
[Python packages]: /ref/app-types/#python-package
[Ruby packages]: /ref/app-types/#ruby-package
