# Custom Bench Apps

This document is the registry for custom applications, used by _Bench_.

An app is defined by a number of name-value-pairs.
The pairs are written as an unordered list, with a colon separating the name and the value.
A value can be surrounded by angle brackets `<` and `>` if it is a URL.
Any value can be surrounded by backticks.
If a value is a list, its items must be surrounded by backticks and separated by commas `, `.

All apps are identified by an ID, which must only contain alphanumeric characters
and must not start with a numeric character.
The ID must be the first entry in a list, defining an app.

There are currently two types of apps: Windows executables and NodeJS packages.

## Windows Executables

* **ID**:
  The ID of the app.
* **Typ**:
  The application typ (optional, default is `default`)
* **Url**:
  The URL to the file, containing the app binaries
* **File**:
  The name of the downloaded file (only for executable downloads like `*.exe` or `*.cmd`).
* **Archive**:
  The name of the downloaded archive with wildcards `?` and `*` (for archives which need to be extracted).
* **ArchiveSubDir**:
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

Some restrictions for the properties:

* The properties _File_ and _Archive_ are mutually exclusive.
* The property _ArchiveSubDir_ is only recognized, if _Archive_ is used.
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
* **Force**:
  A boolean, indicating if the package should allways be installed,
  even if it is allready installed (optional, default is `false`).
* **Exe**:
  The name of an NPM CLI wrapper from this package (optional, default is empty).

To determine, if a NodeJS package is allready installed, `npm list -g` is called.

## Custom Apps

Place your app definitions here.