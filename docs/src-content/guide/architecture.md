+++
date = "2016-11-11T12:10:00+02:00"
description = "The components and their relations"
title = "System Architecture"
weight = 1
+++

The Bench system consists of a [file system layout](#fs), binaries for
[Core Library](#core), [Command Line Interface](#cli), and [Dashboard](#dashboard),
a couple of [PowerShell](#ps-scripts)/[CMD](#cmd-scripts) scripts,
the [app libraries](#app-libs), and the [configuration](#config).
Since the architecture is constantly evolving until Bench hits the version 1.0
this article contains only a brief description of the current state.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

![Architecture Overview](/img/architecture.svg)

## File System Layout {#fs}
Bench uses a file system layout with three folder groups.
For a detailed description of the file structure see:
[File Structure Reference](/ref/file-structure).

### Bench System {#fs-bench}
The first group contains binaries, scripts and resources
of the Bench system itself.
This group is replaced in case of an upgrade.

[`auto`](/ref/file-structure/#auto-dir),
[`res`](/ref/file-structure/#res-dir)

### User Configuration and Data {#fs-user}
The second group contains the user configuration
and user data.
This group is not changed by any Bench operation
like upgrade, app installation, or app removal.

[`config`](/ref/file-structure/#config-dir),
[`home`](/ref/file-structure/#home-dir),
[`projects`](/ref/file-structure/#projects-dir),
[`archive`](/ref/file-structure/#archive-dir),
[`log`](/ref/file-structure/#log-dir),
[`bench-site.md`](/ref/file-structure/#bench-site)

### Temporary Files and App Resources {#fs-work}
And the third group contains the installed apps
and temporary data.
This group can be deleted without any actual data loss,
because it is rebuild by Bench during the app installation
automatically.

[`cache`](/ref/file-structure/#cache-dir),
[`lib`](/ref/file-structure/#lib-dir),
[`launcher`](/ref/file-structure/#launcher-dir),
[`tmp`](/ref/file-structure/#tmp-dir),
[`env.cmd`](/ref/file-structure/#env),
`BenchDashboard.lnk`

## Bench Core Binary {#core}
The most logic in the Bench system is implemented in the `BenchLib.dll`
wich is also called the _Bench Core_ binary.
It is a Microsoft .NET assembly with a [public API](/ref/clr-api).
It supports loading the different layers of the
[Bench configuration](/ref/config), downloading, installing, updating,
and removing apps according to the loaded configuration.

## Command Line Interface {#cli}
Bench provides a command line interface via the [`bench.exe`](/ref/bench-cli).
It allows to manage and interact with the Bench environment.
The _Bench CLI_ depends on the _Bench Core_.

## Bench Dashboard {#dashboard}
The dashboard is a program with a [graphical user interface](/ref/dashboard)
which makes the various features of the Bench system available
in a mouse-friendly way.
The _Bench Dashboard_ depends on the _Bench Core_.

## Power-Shell Scripts {#ps-scripts}
Some features are implemented by PowerShell scripts.
They load the `BenchLib.dll` and call its public methods.

Two important features, implemented by PowerShell scripts, are the
[execution adornment](/guide/isolation/#execution-adornment)
and the
[registry isolation](/guide/isolation/#registry-isolation).
In both cases, an execution proxy in
[`lib\_proxies`](/ref/file-structure/#lib-proxies-dir)
calls `Run-Adorned.ps1` to execute pre- and post-execution scripts.
The registry isolation is implemented as a couple of functions in `reg.lib.ps1`.

## CMD Scripts {#cmd-scripts}
There are three CMD batch scripts for launching
a shell in the Bench environment:
`bench-cmd.cmd`, `bench-ps.cmd`, and `bench-bash.cmd`.
These scripts depend on the `env.cmd` in the Bench root directory
for loading the environment variables.

## App Libraries {#app-libs}
The knowledge about app resources and thier installation strategy is
stored in [app libraries](/ref/app-library).
An app library contains an index with the [app properties](/ref/app-properties)
and optionally PowerShell scripts for customization of setup steps
and execution adornment.

App libraries are hosted independently and are [referenced](/ref/config/#AppLibs)
in the Bench configuration.
The [user configuration](/ref/file-structure/#config-dir) is also an app library.
It is called the _user app library_, and contains app descriptions
written by the user.

## Configuration Levels {#config}
Bench supports three levels of configuration:

1. The [default configuration](/ref/file-structure/#res-config),
   which comes with the Bench installation and should not be edited.
2. The [user configuration](/ref/file-structure/#config-config),
   which can be versioned and shared via Git.
3. And the [site configuration](/ref/file-structure/#bench-site),
   which can be stored outside of the Bench environment
   and can take influence on multiple Bench environments.

Configuration files are written in [Markdown list syntax](/ref/markup-syntax/)
and support a number of [configuration properties](/ref/config).
