+++
date = "2016-11-11T12:10:00+02:00"
description = "The components and their relations"
title = "System Architecture"
weight = 1
+++

The Bench system consists of a file system layout, a CLR binary, the Dashboard
and a couple of CMD batch and PowerShell scripts.
Since the architecture is constantly evolving until Bench hits the version 1.0
this article contains only a brief description of the current state.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

![Architecture Overview](/img/architecture.svg)

[Detailed Dependency Diagram](/img/dependencies.svg)

## Folder Layout {#fs}
Bench uses a file system layout with three folder groups.
For a detailed description of the file structure see:
[File Structure Reference](/ref/file-structure).

### Bench System {#fs-bench}
The first group contains scripts, binaries and resources
of the Bench system itself.
This group is replaced in case of an upgrade.

[`actions`](/ref/file-structure/#action-dir),
[`auto`](/ref/file-structure/#auto-dir),
[`res`](/ref/file-structure/#res-dir)

### User Configuration and Data {#fs-user}
The second group contains the user configuration
and user data.
This group is not changed by any Bench operation
like upgrade, app installation, or app removal.

[`archive`](/ref/file-structure/#archive-dir),
[`config`](/ref/file-structure/#config-dir),
[`home`](/ref/file-structure/#home-dir),
[`projects`](/ref/file-structure/#projects-dir),
[`log`](/ref/file-structure/#log-dir),
[`bench-site.md`](/ref/file-structure/#bench-site)

### Temporary Files and App Resources {#fs-work}
And the third group contains the installed apps
and temporary data.
This group can be deleted without any actual data loss,
because it is rebuild by Bench during the app installation
automatically.

[`launcher`](/ref/file-structure/#launcher-dir),
[`lib`](/ref/file-structure/#lib-dir),
[`tmp`](/ref/file-structure/#tmp-dir),
[`env.cmd`](/ref/file-structure/#env),
`BenchDashboard.lnk`

## Interface Scripts {#interface-scripts}
Bench provides a couple of scripts to perform actions in the Bench environment.
The most important script is `bench-ctl.cmd` for managing
the Bench environment.
Additional examples are `bench-ps.cmd` to start a CMD shell in the Bench
environment and `project-backup.cmd` to create a ZIP file in the project archive.

_Warning: The interface scripts are under reconsideration and will probably
change in one of the next releases._

## Power-Shell Scripts {#ps-scripts}
The bridge between the CMD scripts and the Bench core binary is realized
via PowerShell scripts.
They are loading the `BenchLib.dll` and call its public methods.
They implement the Bench actions and some additional tasks.

One important task, implemented by PowerShell scripts, is the
[execution adornment](/guide/isolation/#execution-adornment)
and the
[registry isolation](/guide/isolation/#registry-isolation).
In both cases, the an execution proxy in
[`lib\_proxies`](/ref/file-structure/#lib-proxies-dir)
calls `Run-Adorned.ps1` to execute pre- and post-execution scripts
and perform the registry isolation with `reg.lib.ps1`.

_Warning: The role of the PowerShell scripts in the Bench system
is under reconsideration and will probably change in one of the next releases._

## Bench Core Binary {#core}
The most logic in the Bench system is implemented in the `BenchLib.dll`
wich is also called the Bench core binary.
It is a Microsoft .NET assembly with a [public API](/ref/clr-api).
It supports loading the different layers of the
[Bench configuration](/ref/config), downloading, installing, updating,
and removing apps according to the loaded configuration.

## Bench Dashboard {#dashboard}
The dashboard is a program with a [graphical user interface](/ref/dashboard)
which makes the various features of the Bench system available
in a mouse-friendly way.
