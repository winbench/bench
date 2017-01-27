+++
date = "2017-01-11T12:30:00+02:00"
description = "The format of app libaries"
title = "App Library"
weight = 4
+++

App libraries provide the app definitions, Bench needs, to download and install programs.
Bench comes with a number of its own app libraries, hosted on [GitHub](https://github.com)
and preconfigured in a newly initialized Bench environment.
But sometimes you want to define your own apps, which are not included
in the official Bench app libraries.
To define your own apps, you can just edit the [`apps.md`](#apps-file)
in the [`config`](/ref/file-structure/#config-dir) directory.
Because, the user configuration directory of a Bench environment is
at the same time the _user app library_.
If you want to share your app definitions with others,
you can create separate app libraries and [host](#hosting) them in different ways.

**Overview**

<!-- #data-list /*/* -->

## App Definition

An app library consists of a number of app definitions.
App definitions allows Bench to download extract and install Windows programs.
And an app definition consists of:

1. a number of [app properties](/ref/app-properties)
   specified in the [app index](#apps-file) of the library,
2. [custom scripts](#scripts-dir) to customize the setup steps of an app
   via PowerShell, and
3. [setup resources](#res-dir), which can be used by the custom scripts.

## File and Directory Layout
An app library contains at least an [app index file](#apps-file).
Additionally it can contain the [custom scripts](#scripts-dir) for its apps
and [setup resources](#res-dir) needed by the [custom scripts](#scripts-dir).
It is good practice to add a [readme](#readme-file) and a [license](#license-file) file too.

The full layout of an app library looks like this:

* [`apps.md`](#apps-file) App Index File
* `README.md` Readme file with a brief description of the app library
* `LICENSE.md` The license under which the app library is published
* [`scripts`](#scripts-dir) Custom Scripts Directory
  ([AppLibCustomScriptDirName](/ref/config/#AppLibCustomScriptDirName))
    + _app namespace_ &rarr;
    + [`<app-id>.extract.ps1`](#custom-script-extract)
    + [`<app-id>.setup.ps1`](#custom-script-setup)
    + [`<app-id>.env.ps1`](#custom-script-env)
    + [`<app-id>.remove.ps1`](#custom-script-remove)
    + [`<app-id>.pre-run.ps1`](#custom-script-pre-run)
    + [`<app-id>.post-run.ps1`](#custom-script-post-run)
* [`res`](#res-dir) App Setup Resources Directory
  ([AppLibResourceDirName](/ref/config/#AppLibResourceDirName))
    + _app namespace_ &rarr;
    + ...

Custom scripts and setup resources are organized by the namespace of their app.
E.g. if an app `MyNamespace.MyApp` has a custom script for the environment setup,
the custom script would have the path `scripts\mynamespace\myapp.env.ps1`.

## App Index {#apps}

The IDs and properties of all apps in a library are stored in the app index.
The app index consists of a Markdown file, with a section for every app.
Additionally to the machine readable [app properties](/ref/app-properties),
every app definition can contain Markdown text which is displayed
as a description for an app in the [BenchDashboard](/ref/dashboard).

### App Index File {#apps-file}

* Description: The app index file.
* Path: `<app lib>\apps.md`
* Config Property: [AppLibIndexFileName](/ref/config/#AppLibIndexFileName)
* Type: file
* Required: yes

The app index file is written in [Markdown list syntax](/ref/markup-syntax).
Every app is defined by a number of [App Properties](/ref/app-properties/).

## Custom Scripts {#scripts}

Custom scripts allow the maintainer of an app library to customize certain steps
in the setup and execution of apps via PowerShell scripts.

The following types of custom scripts are supported:

* [`extract`](#custom-script-extract)
* [`setup`](#custom-script-setup)
* [`env`](#custom-script-env)
* [`remove`](#custom-script-remove)
* [`pre-run`](#custom-script-pre-run)
* [`post-run`](#custom-script-post-run)
* [`test`](#custom-script-test)

If the file format of the downloadable setup program for an app is not supported
by Bench, then the file extraction can be implemented in the custom script
for the _extract_ setup.

If the original setup program of a Windows application performs some necessary
actions to prepare the program files for execution, these actions
usually need to be reimplemented in the custom script for the _setup_ step.
And clean-up tasks to uninstall an app properly need to be implemented in the
custom script for the _remove_ step.

If the configuration files of a program contain absolute paths the programs
installation directory, updating the configuration files need to be implemented
in the custom script for the _env_ step.

To perform some tasks every time before or after a program is executed,
they can be implemented in the custom scripts for the _pre-run_ and _post-run_ steps.

When the app is tested for proper definition, the _test_ step is performed
after the installation.

### App Custom Script Directory {#scripts-dir}

* Description: The directory with the custom scripts of the apps.
* Path: `<app lib>\scripts`
* Config Property: [AppLibCustomScriptDirName](/ref/config/#AppLibCustomScriptDirName)
* Type: directory
* Required: no

Custom scripts are organized by the namespaces of their app.
E.g. if an app `MyNamespace.MyApp` has a custom script for the environment setup,
the custom script would have the path `scripts\mynamespace\myapp.env.ps1`.

### App Custom Script `extract` {#custom-script-extract}

* Description: Custom script for app resource extraction.
* Path: `<app lib>\scripts\<app ns>\<app-id>.extract.ps1`
* Type: file

Custom scripts for app resource extraction must be named with the app ID
in lower case, and the name extension `.extract.ps1`.

The _custom script for extraction_ is executed if the
[`ArchiveTyp`](/ref/app-properties/#ArchiveTyp) is set to `auto` or `custom`.
If the [`ArchiveTyp`](/ref/app-properties/#ArchiveTyp) of the app is set
to `auto` and a _custom script for extraction_ for this app exists,
the custom script takes precedence over the other extraction methods.

Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.
Custom extraction scripts are called with two command line arguments:

1. The absolute path of the downloaded app resource archive
2. The absolute path of the target directory to extract the resources

Example for the extraction of a nested archive:

```PowerShell
param ($archivePath, $targetDir)

$nestedArchive = "nested.zip"

# create temporary directory
$tmpDir = Empty-Dir "$(Get-ConfigValue TempDir)\custom_extract"

# get path of 7-Zip
$7z = App-Exe "Bench.7z"

# call 7-Zip to extract outer archive
& $7z x "-o$tmpDir" "$archivePath"

# check if expected inner archive exists
if (!(Test-Path "$tmpDir\$nestedArchive"))
{
    throw "Did not find the expected content in the app resource."
}

# call 7-Zip to extract inner archive
& $7z x "-o$targetDir" "$tmpDir\$nestedArchive"

# Delete temporary directory
Purge-Dir $tmpDir
```

### App Custom Script `setup` {#custom-script-setup}

* Description: Custom script for app setup.
* Path: `<app lib>\scripts\<app ns>\<app-id>.setup.md`
* Type: file

Custom scripts for app resource extraction must be named with the app ID
in lower case, and the name extension `.setup.ps1`.

If a _custom setup script_ for an app exists, it is executed after
the installation of the (extracted) app resources in the
[apps target dir](#lib-app).
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) is available.

### App Custom Script `env` {#custom-script-env}

* Description: Custom script for environment setup.
* Path: `<app lib>\scripts\<app ns>\<app-id>.env.ps1`
* Type: file

Custom scripts for environment setup must be named with the app ID
in lower case, and the name extension `.env.ps1`.

If a _custom environment setup script_ for an app exists, it is executed
after the setup to update configuration files depending
on the location of Bench or other [configuration properties](/ref/config).
It is also called if the _Upade Environment_ task for Bench is executed.
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.

### App Custom Script `remove` {#custom-script-remove}

* Description: Custom script for app deinstallation.
* Path: `<app lib>\scripts\<app ns>\<app-id>.remove.ps1`
* Type: files

Custom scripts for deinstallation must be named with the app ID
in lower case, and the name extension `.remove.ps1`.

If a _custom deinstallation script_ for an app exists, it is executed
instead of the default uninstall method.
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.

### App Custom Script `pre-run` {#custom-script-pre-run}

* Description: Pre-run hook for adorned executables of an app.
* Path: `<app lib>\scripts\<app ns>\<app-id>.pre-run.ps1`
* Type: file

The _custom pre-run script_ is executed immediately before an app executable is run.
It is only executed if an app executable is run via its execution proxy.
This is usually the case because it is listed in
[AdornedExecutables](/ref/app-properties/#AdornedExecutables).
The [main executable](/ref/app-properties/#Exe) of an app is automatically
included in the list of adorned executables
if the [registry isolation](/ref/app-properties/#RegistryKeys) is used.
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.

### App Custom Script `post-run` {#custom-script-post-run}

* Description: Post-run hook for adorned executables of an app.
* Path: `<app lib>\scripts\<app ns>\<app-id>.post-run.ps1`
* Type: file

The _custom post-run script_ is executed immediately after an app executable is run.
It is only executed if an app executable is run via its execution proxy.
This is usually the case because it is listed in
[AdornedExecutables](/ref/app-properties/#AdornedExecutables).
The [main executable](/ref/app-properties/#Exe) of an app is automatically
included in the list of adorned executables
if the [registry isolation](/ref/app-properties/#RegistryKeys) is used.
Inside of the _custom script_ is the [PowerShell API](/ref/ps-api/) available.

### App Custom Script `test` {#custom-script-test}

* Description: Test hook for testing the installation of an app.
* Path: `<app lib>\scripts\<app ns>\<app-id>.test.ps1`
* Type: file

The _custom test script_ is executed, when an app definition is tested.
It is executed after a successful installation, after the existence of the
main executable was checked.
The test script fails when it writes an error or throws an exception;
otherwise it will count as a successful test.

## App Setup Resources {#res}

App setup resources are files, which are used by custom scripts to
perform necessary tasks during the various setup and execution steps of an app.

### User App Resources Directory {#res-dir}

* Description: The directory with setup resources for the apps.
* Path: `<app lib>\res`
* Config Property: [AppLibResourceDirName](/ref/config/#AppLibResourceDirName)
* Type: directory
* Required: no

App setup resources are organized by the namespaces of their app.
E.g. if an app `MyNamespace.MyApp` has a setup resource named `config-template.xml`,
it would have the path `<app lib>\res\mynamespace\myapp\config-template.xml`.

To get the absolute path of an app setup resource file from a custom script,
you can use the PowerShell API function [`App-SetupResource`](/ref/ps-api/#fun-app-setupresource).

E.g. to retrieve the path of the `config-template.xml` from above, the custom
script `<app lib>\scripts\mynamespace\myapp.setup.ps1` could contain:

```PowerShell
$myAppDir = App-Dir "MyNamespace.MyApp"
$templateFile = App-SetupResource "MyNamespace.MyApp" "config-template.xml"
copy $templateFile "$myAppDir\config.xml" -Force
```

## Hosting {#hosting}

An app library must be reachable via an URL, so it can be refered to in the
[`AppLibs`](/ref/config/#AppLibs) configuration property.
Currently supported are _https(s)_ and _file_ URLs.
Additionally, there is a shorthand form for _GitHub_ hosted app libraries.

If the library is hosted via _http(s)_, the library must be packed in a ZIP file.
If the library is reachable in the file system, it can be packed in a ZIP file,
but it can also just be an open folder.
If the library is packed in a ZIP file, its content can be put directly in
the ZIP file, or it can be wrapped in exactly one sub-folder.
But in this case, no other files or folders are allowed in the root of the ZIP file.

### Example 1 &ndash; open folder in the filesystem {#hosting-example-1}

This is a fitting approach if you want to use a locally maintained app library
in multiple Bench environments, or if you have an infrastructure with
SMB shares.

* Path to the app library folder: `D:\applibs\my-applib`
* Content of the folder:
    + `apps.md`
    + `scripts`
        - ...
* URL in `AppLibs`: `file:///D:/applibs/my-applib`

### Example 2 &ndash; ZIP file on a web server {#hosting-example-2}

This is a fitting approach if you want to share you app library
without publishing it on GitHub, or if you have an intranet web server
under the control of you development team.
You could even store the app library in a cloud storage like DropBox
and distribute a public link for sharing.

* Name of the ZIP file: `my-apps.zip`
* Content of the ZIP file:
    + `apps.md`
    + `README.md`
    + `LICENSE.md`
    + `scripts`
        - ...
* URL in `AppLibs`: `http://www.your-domain.net/bench-app-libs/my-apps.zip`

### Example 3 &ndash; public GitHub repository {#hosting-example-3}

This is a fitting approach if you app library is free to be used by anybody.

* Username on GitHub: `the-programmer`
* Repository name: `favorite-bench-apps`
* Content of the repository:
    + `apps.md`
    + `README.md`
    + `LICENSE.md`
    + `scripts`
        - ...
* URL in the `AppLibs`: `github:the-programmer/favorite-bench-apps`
* Main branch must be: `master`
* Automatically expanded URL: `https://github.com/the-programmer/favorite-bench-apps/archive/master.zip`
* GitHub generated content of the ZIP archive:
    + `favorite-bench-apps-master`
        - `apps.md`
        - `README.md`
        - `LICENSE.md`
        - `scripts`
