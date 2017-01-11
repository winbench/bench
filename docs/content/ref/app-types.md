+++
date = "2016-07-13"
description = "The different types of apps in Bench"
title = "App Types"
weight = 7
+++

[`Typ`]: /ref/app-properties/#Typ

There are currently the following types of apps:

* Typ [`meta`](#meta): app groups or apps with a fully customized setup process
* Typ [`default`](#default): Windows executables from a downloaded file, archive, or setup
* Typ [`node-package`](#node-package): Node.js packages, installable with NPM
* Typ [`python2-package`](#python-package): Python packages for Python 2 from PyPI, installable with PIP
* Typ [`python3-package`](#python-package): Python packages for Python 3 from PyPI, installable with PIP
* Typ [`ruby-package`](#ruby-package): Ruby packages, installable with Gem
* Typ [`nuget-package`](#nuget-package): NuGet packages, installable with NuGet

## Meta App {#meta}
An app is a _Meta App_ if its [`Typ`][] is set to `meta`.

A _Meta App_ is an app without an actual program.
It can be used as a **group** by only referencing other apps as dependencies.
Or it can be used to fully customize the download and setup process
of an app with custom scripts.

## Default Windows App {#default}
An app is a _Default Windows App_ if its [`Typ`][] property
is set to `default` or not set at all.

A _Default Windows App_ is the definition for some kind of script or binary
executable for the Windows operating system.
In contrast to other apps, a _Default Windows App_ has a program resource,
which can be downloaded as a file.
The resource can be an executable on its own, or an archive of some kind,
even a setup or installer EXE.

A _Default Windows App_ is extracted, and installed simply by copying
its files into its target directory.
Optionally some custom scripts can be executed during the setup,
to perform some additional configuration for the app.

## Node.js Package {#node-package}
An app is a _Node.js Package_ if its [`Typ`][] property is set to `node-package`.

A _Node.js Package_ is downloaded and installed by _npm_ the Node.js package manager.

To determine, if a _Node.js Package_ is already installed, the existence of its package folder in
`node_modules` in the Node.js directory is checked.

## Python Package {#python-package}
An app is a _Python Package_ if its [`Typ`][] property is set to `python2-package` or `python3-package`.

A _Python Package_ is downloaded and installed by _PIP_ the Python package manager.
_Python Packages_ for Python 2 and Python 3 are defined separately.
If you need a _Python Package_ for both major versions, you need to define it twice.

To determine, if a _Python Package_ is already installed, the existence of its package folder in
`lib\site-packages` in the Python directory is checked.

## Ruby Package {#ruby-package}
An app is a _Ruby Package_ if its [`Typ`][] property is set to `ruby-package`.

A _Ruby Package_ is actually a _gem_ and is downloaded and installed by the Ruby package
manager, also called _gem_.

To determine, if a _Ruby gem_ is already installed, the existence of its package folder in
`lib\ruby\gems\<ruby-version>\gems` in the Ruby directory is checked.

## NuGet Package {#nuget-package}
An app is a _NuGet Package_ if its [`Typ`][] property is set to `nuget-package`.

A _NuGet Package_ is one package from <https://www.nuget.org/packages> and all of its dependencies.
The package and its dependencies are installed inside of the apps target directory
`lib\<AppDir>` side-by-side.
The individual package folders in the app target directory `lib\<AppDir>\<PackageName>`
are created without a version number, to simplify building paths.

To determine, if a _NuGet Package_ is already installed,
the existence of the `*.nupkg` file of the main package
`lib\<AppDir>\<PackageName>\<PackageName>.nupkg` is checked.
