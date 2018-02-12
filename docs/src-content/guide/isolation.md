+++
date = "2016-06-22T13:23:31+02:00"
description = "The mechanisms for isolating Bench from the Windows system"
title = "Isolation and Portability"
weight = 4
+++

[Mastersign.Bench.BenchEnvironment]: https://winbench.org/clr-api/html/T_Mastersign_Bench_BenchEnvironment.htm
[OverrideHome]: /ref/config/#OverrideHome
[OverrideTemp]: /ref/config/#OverrideTemp
[IgnoreSystemPath]: /ref/config/#IgnoreSystemPath
[RegisterInUserProfile]: /ref/config/#RegisterInUserProfile
[UseRegistryIsolation]: /ref/config/#UseRegistryIsolation
[UseProxy]: /ref/config/#UseProxy
[AppsAdornmentBaseDir]: /ref/config/#AppsAdornmentBaseDir
[RegistryKeys]: /ref/app-properties/#RegistryKeys
[AdornedExecutables]: /ref/app-properties/#AdornedExecutables

Bench provides multiple mechanisms to isolate Bench apps from the Windows system
and the current Windows user profile, and vice versa.
The most important technique is to change the environment variables when starting programs.
Additionally it supports running a pre- and post-execution script for programs,
called execution adornment, which is used by Bench itself, to provide
Windows registry isolation.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

## Environment Variables {#env-vars}
Bench provides isolation between the Bench apps and the Windows system
by changing the environment variables when it starts a program.
The composition of the environment variable values is implemented in
[Mastersign.Bench.BenchEnvironment][]
The manipulation of the environment variables is performed by
[launchers](/guide/launcher.md), which call the
[environment script file](/ref/file-structure/#env).

The following environment variables can be changed by Bench:

* `PATH`
  to make the registered Bench apps available on the command line,
  and optionally to hide tools from the command line, which are installed in Windows
* `TEMP`, `TMP`
  to keep temporary files inside of the Bench root directory
* `USERNAME`, `USEREMAIL`
  to override the name of the Windows user account with the Bench user name
* `USERPROFILE`, `HOME`, `HOMEDRIVE`, `HOMEPATH`
  to keep Bench user data inside of the Bench root directory
* `APPDATA`, `LOCALAPPDATA`
  to keep application settings and data from Bench apps inside the
  Bench root directory
* `HTTP_PROXY`, `HTTPS_PROXY`
  not primarily for isolation, but for specifying a HTTP(S) proxy
  for UNIX style command line tools

## Execution Adornment {#execution-adornment}
To allow running custom actions before an executable of a Bench app is started,
Bench supports running [pre-](/ref/file-structure/#custom-script-pre-run) and
[post-execution](/ref/file-structure/#custom-script-post-run) scripts.
Adornment scripts are not automatically run for every executable of an app.
Instead, adornment scripts are only run for executables listed in the app property
[AdornedExecutables][].

If a [launcher](/guide/launcher) starts the main executable of an app,
he checks, whether this executable is listed in [AdornedExecutables][],
and runs the pre- and post-execution scripts
(and the [registry isolation](#registry-isolation)).
To allow pre- and post-execution scripts when running command line tools,
the concept of execution proxies is used.

An _execution proxy_ is a Batch script (`.cmd`), named like the targeted executable,
calling the pre- and post-execution scripts before and after running the target
executable.
The parent directory of the proxy script is placed on the `PATH` environment variable,
but before the directory of the target executable.
Command line arguments, given to the proxy script, are passed to the target executable.
As a consequence, when calling an adorned command line tool
(without its filename extension), the execution proxy is found first on the `PATH`
and executed instead of the target executable.

Execution proxies are stored in the directory, specified by the configuration
property [AppsAdornmentBaseDir][], which is `lib\_proxies` by default.

## Registry Isolation {#registry-isolation}
Bench supports a simple form of isolating apps that write to the Windows registry.

For every app the property [RegistryKeys][]
can contain a list of registry paths, which are changed by the app.
The [execution adornment](#execution-adornment) for this app then first creates
a _system backup_ of the specified registry keys in the
[backup directory](/ref/file-structure/#home-registry-isolation-dir),
and restores a potential _bench backup_ to the registry.
Then the executable is started.
When the executable finishes, the potentially changed state of the specified
registry keys is stored as the new _bench backup_, and the _system backup_
is restored to the registry.

This mechanism requires the author of the app definition to find out,
which registry keys are changed by his app, and list them in the [RegistryKeys][]
property.

In case the [RegistryKeys][] property is not empty, the [main executable](/ref/app-properties/#Exe)
of the app is automatically added to the
[list of adorned executables](/ref/app-properties/#AdornedExecutables),
to activate the [execution adornment](#execution-adornment).
If the app has more than one executable, writing to the registry,
this list must contain all affected executables.

## Configuration Properties {#config-properties}
The isolation between Bench apps and the Windows system can be controlled
by a couple of configuration properties.

* [OverrideHome][] controls if the environment variables
  `USERNAME`, `USEREMAIL`, `USERPROFILE`, `HOME`, `HOMEDRIVE`, and `HOMEPATH`
  get changed by Bench.
* [OverrideTemp][] controls if the environment variables
  `TEMP` and `TMP`
  are changed by Bench.
* [IgnoreSystemPath][] controls whether the `PATH` variable is reset before adding
  paths to the Bench apps, or if the paths from the Windows settings are preserved.  
  When resetting, the `PATH` variable it is set to
  `%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0`.
* [RegisterInUserProfile][] controls if the `PATH` variable in the current Windows
  user profile is changed to contain paths to the Bench apps.
* [UseProxy][]
  controls if the environment variables
  `HTTP_PROXY` and `HTTPS_PROPXY`
  are changed by Bench.
* [UseRegistryIsolation][] controls, whether the
  [registry isolation](#registry-isolation) mechanism is activated, or not.

## Configuration Levels {#config-levels}
Bench uses a hierarchy of configuration files to support flexibility for
different scenarios &ndash; especially portable Bench environments.
The hierarchy has three levels:

1. [Default configuration](/ref/file-structure/#res-config)  
   The default configuration is predefined by Bench.
   This level can not be modified by the user.
   It contains system properties, which are not supposed to be changed at all.
   And it contains default values for customizable configuration properties.
2. [User configuration](/ref/file-structure/#config-config)  
   The user or custom configuration is defined by the user in the
   [user configuration directory](/ref/file-structure/#config-dir).
   This level can be put under version control, to be shared with others.
3. [Site configuration(s)](/ref/file-structure/#bench-site)  
   The site configuration(s) act as a local override, to adapt
   the configuration to a specific Windows installation or PC system,
   or thumb drive.
   Site configuration files can build a hierarchy of their own,
   to granular configure multiple side-by-side Bench installations.

All configuration files are written with the [Markdown list syntax](/ref/markup-syntax).

## Typical Configurations {#typical-configurations}
To achieve the maximum isolation between Bench and the Windows system,
the following settings are required.
This configuration is useful for a portable Bench environment
and it is also the default configuration of Bench.

* [OverrideHome][] = `true`
* [OverrideTemp][] = `true`
* [IgnoreSystemPath][] = `true`
* [RegisterInUserProfile][] = `false`
* [UseRegistryIsolation][] = `true`

To use multiple Bench environments smoothly side by side,
but allow the usage of the tools installed in Windows,
the following configuration is advised.

* [OverrideHome][] = `true`
* [OverrideTemp][] = `false`
* [IgnoreSystemPath][] = `false`
* [RegisterInUserProfile][] = `false`
* [UseRegistryIsolation][] = `true`

To use one Bench environment integrated into the Windows user profile
and without isolation, use the following configuration.
With this configuration, Bench apps can be used from the default Windows shells,
without starting a [Bench shell](/guide/shell).
Bench apps are executed without isolation as if they where installed
in the Windows system under the current user.

* [OverrideHome][] = `false`
* [OverrideTemp][] = `false`
* [IgnoreSystemPath][] = `false`
* [RegisterInUserProfile][] = `true`
* [UseRegistryIsolation][] = `false`
