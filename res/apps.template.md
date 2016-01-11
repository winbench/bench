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

There are currently three types of apps: app groups (Typ: `meta`), Windows executables (Typ: `default`), and NodeJS packages (Typ: `node-package`).

## App Group

* **ID**:
  The ID of the app.
* **Typ**:
  The application typ (required to be `meta`)
* **Dependencies**:
  A list with the IDs of all apps in this app group.
* **Environment**:
  A list of key-value-pairs, describing additional environment variables (optional, default is empty).
  E.g. `MY_APP_HOME=$MyApp:Dir$`, `MY_APP_LOG=D:\logs\myapp.log`

## Windows Executables

* **ID**:
  The ID of the app.
* **Typ**:
  The application typ (optional, default is `default`)
* **Dependencies**:
  A list with the IDs of apps, which must be activated too, for this app to work (optional, default is empty).
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
* **Dir**:
  The name of the target directory for the app (optional, default is the app ID in lowercase).
* **Path**:
  A list of relative paths inside the app directory to register in the environment `PATH`. (optional, default is `.`).
* **Register**:
  A boolean to indicate if the path(s) of the application should be added to the environment `PATH` (optional, default is `true`).
* **Exe**:
  The name of the app executable (optional, default is empty).
  The existance of an app executable is used to determine, if an app is allready installed.
* **Environment**:
  A list of key-value-pairs, describing additional environment variables (optional, default is empty).
  E.g. `MY_APP_HOME=$MyApp:Dir$`, `MY_APP_LOG=D:\logs\myapp.log`

Some restrictions for the properties:

* The properties _AppFile_ and _AppArchive_ are mutually exclusive.
* The property _AppArchiveSubDir_ is only recognized, if _AppArchive_ is used.
* The property _Path_ is only recognized, if _Register_ is `true`.

## NodeJS Packages

* **ID**:
  The ID of the app.
* **Typ**:
  The application typ (required to be `node-package`).
* **NpmPackage**:
  The name of the NPM package to install via NPM (optional, default is the app ID in lowercase).
* **Version**:
  The package version to install (e.g. `^2.5.0`), if empty install latest (optional, default empty).
* **Dependencies**:
  A list with the IDs of apps, which must be activated too, for this app to work (optional, default is empty).
* **Force**:
  A boolean, indicating if the package should allways be installed,
  even if it is allready installed (optional, default is `false`).
* **Exe**:
  The name of an NPM CLI wrapper from this package (optional, default is empty).
* **Environment**:
  A list of key-value-pairs, describing additional environment variables (optional, default is empty).
  E.g. `MY_APP_HOME=$MyApp:Dir$`, `MY_APP_LOG=D:\logs\myapp.log`

To determine, if a NodeJS package is allready installed, the existence of its package folder in `node_modules` in the NodeJS directory is checked.

## Custom Apps

Place your app definitions here.