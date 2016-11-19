+++
date = "2016-06-22T13:29:04+02:00"
description = "The accessability of apps through Launchers and the Dashboard"
title = "Launchers"
weight = 7
+++

[launcher directory]: /ref/file-structure/#launcher-dir
[launcher script]: /ref/file-structure/#lib-launcher-dir
[dashboard-main-window]: /ref/dashboard/#main-window
[isolation]: /guide/isolation
[adornment]: /guide/isolation/#execution-adornment
[environment script]: /ref/file-structure/#env
[registry isolation]: /guide/isolation/#registry-isolation

To start a Bench app or shell, launchers are provided.
Launchers appear in two different forms:
A shortcut in the [launcher directory][], and an icon in the Bench Dashboard
[main window][dashboard-main-window].
Additionally to start them by double click, dragging a file onto a launcher,
starts the linked app or shell and passes the path of the dropped file
as a command line parameter to the app or shell.
Usually this means, the app opens and the shell executes the dropped file.
<!-- more -->

**Overview**

<!-- #data-list /*/* -->

## Loading the Bench Environment {#env}
A launcher starts an app or a shell inside of the Bench environment.
Therefore, the started app or shell has a modified set of environment variables
according to the [isolation settings][isolation] of Bench.

If the launcher in the Bench Dashboard is used, the process is created
directly with a modified set of environment variables.
If the shortcut is used, a [launcher script][] (CMD batch file) is executed,
which first calls the [environment script][] `env.cmd` to load the Bench
environment, and then starts the app or shell.

## Running Adornments {#adornment}
If an app has [pre- or post-execution scripts][adornment], or it uses the [registry isolation][]
feature, the launcher executes the app via the PowerShell script
`auto/lib/Run-Adorned.ps1`. This script takes care of performing the
registry isolation and running the pre- and post-execution scripts.

## Passing Arguments {#passing-args}
The launcher is designed to be as transparent as possible during the execution
of an app. This means it passes given command line arguments down to the app.
This behavior is important to allow opening a file by dropping it onto
an app launcher.

## Drag & Drop Launching {#drag-and-drop}
If a file is dropped onto a launcher, its path is passed as a command line
argument to the linked app or shell.
Usually this means the app opens the dropped file and the shell executes the
dropped file.

This behavior comes in handy, when the [launcher directory][] is used as source
for a toolbar in the Windows taskbar. In that situation not only the Bench apps
are easily available in the Windows taskbar, but dropping a script or a file
onto a launcher icon in that toolbar, starts an app or a shell inside of the
Bench environment with all consequences.

See also: [Drag & Drop Launching](/guide/shell/#drag-and-drop) for shells
