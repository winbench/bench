# Custom Bench Apps

This document is the registry for custom applications, used by _Bench_.

An app is defined by a number of name-value-pairs.
The pairs are written as an unordered list, with a colon separating the name and the value.
A value can be surrounded by angle brackets `<` and `>` if it is a URL.
Any value can be surrounded by backticks.
If a value is a list, its items must be surrounded by backticks and separated by commas `, `.

You can use placeholders in variable values.
Placeholders can be application specific configuration values
like `$Git:Dir$` and `$Npm:Path$` or global configuration values
like `$BenchRoot$` and `$ProjectArchiveDir$`.

All apps are identified by an ID, which must only contain alphanumeric characters
and must not start with a numeric character.
The ID must be the first entry in a list, defining an app.

## Common Properties

* **ID**:
  The ID of the app.
* **Typ**:
  The application typ (optional, default is `default`).
* **Dependencies**:
  A list with the IDs of all apps in this app group.
* **Force**:
  A boolean, indicating if the package should allways be installed,
  even if it is already installed (optional, default is `false`).
* **Dir**:
  The name of the target directory for the app (optional, default is the app ID in lowercase).
* **Path**:
  A list of relative paths inside the app directory to register in the environment `PATH`. (optional, default is `.`).
  This property is only recognized, if `Register` is `true`.
* **Register**:
  A boolean to indicate if the path(s) of the application should be added to the environment `PATH` (optional, default is `true`).
* **Environment**:
  A list of key-value-pairs, describing additional environment variables (optional, default is empty).
  E.g. `MY_APP_HOME=$MyApp:Dir$`, `MY_APP_LOG=D:\logs\myapp.log`
* **AdornedExecutables**:
  A list of executable paths, relative to the target directory of the app.
  Every listed executable will be adorned with pre- and post-execution scripts.
  (optional, default is empty)
* **Launcher**:
  A label for the app launcher (optional, default is empty).
  A launcher for the app is created only if this property is set to a non empty string.
* **LauncherExecutable**:
  The absolute path to the executable targeted by the app launcher
  (optional, default is the `Exe` property).
* **LauncherArguments**:
  A list with arguments to the app executable (optional, default is `%*`).
* **LauncherIcon**:
  The absolute path to the icon of the launcher (optional, default is the executable).

## App Types

There are currently four kinds of apps:

* Typ `meta`: app groups or apps with a fully customized setup process
* Typ `default`: Windows executables from a downloades file, archive, or setup
* Typ `node-package`: NodeJS packages, installable with NPM
* Typ `python-package`: Python packages from PyPI, installable with PIP

### App Group and Custom Setup

* **Typ**:
  The application typ (required to be `meta`)

### Windows Apps

A Windows app is some kind of executable for the Windows OS.

* **Url**:
  The URL to the file, containing the app binaries
* **DownloadCookies**:
  A list of cookies, to send with the download request (optional, default is empty)
* **AppFile**:
  The name of the downloaded file (only for directly executable downloads like `*.exe` or `*.cmd`).
* **AppArchive**:
  The name of the downloaded archive with wildcards `?` and `*` (for archives which need to be extracted).
* **AppArchiveTyp**:
  The archive typ, which yields to the extractor selection (optional, default is `auto`).
  Possible values are:
    + `auto` Try to determine the extractor by the filename extension or use the custom extractor script if it exists
    + `generic` Use 7-Zip to extract
    + `msi` Use LessMSI to extract
    + `inno` Use Inno Setup Unpacker to extract
    + `custom` Use the custom script `auto\apps\<app ID>.extract.ps1`
* **AppArchiveSubDir**:
  A sub folder in the archive to extract (optional, default is the archive root).

To determine, if a Windows app is already installed, the existance of its executable is checked.

Some restrictions for the properties:

* The properties _AppFile_ and _AppArchive_ are mutually exclusive.
* The property _AppArchiveSubDir_ is only recognized, if _AppArchive_ is used.

### NodeJS Packages

* **Typ**:
  The application typ (required to be `node-package`).
* **NpmPackage**:
  The name of the NPM package to install via NPM (optional, default is the app ID in lowercase).
* **Version**:
  The package version or version range to install (e.g. `2.5.0` or `>=1.2.0 <3.0.0`),
  if empty install latest (optional, default empty).
* **Exe**:
  The name of an NPM CLI wrapper from this package (optional, default is empty).
* **Path**:
  This property is ignored for NodeJS packages.

To determine, if a NodeJS package is already installed, the existence of its package folder in
`node_modules` in the NodeJS directory is checked.

### Python Package

* **Typ**:
  The application typ (required to be `python-package`)
* **PyPiPackage**:
  The name of the PyPI package to install via PIP (optional, default is the app ID in lowercase).
* **Version**:
  The package version or version range to install (e.g. `2.5.0` or `>=1.2.0,<3.0.0`),
  if empty install latest (optional, default empty).
* **PythonVersions**:
  A list with all Python version to install this package in (e.g. `2`, `3`),
  if empty install in all Python versions (optional, default empty).
* **Exe**:
  The name of an PIP CLI wrapper from this package (optional, default is empty).
* **Path**:
  This property is ignored for Python packages.

To determine, if a Python package is already installed, the existence of its package folder in
`lib\site-packages` in the Python directory is checked.

## Custom Apps

Place your app definitions here.
