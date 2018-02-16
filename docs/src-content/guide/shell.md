+++
date = "2018-02-16T12:00:00+02:00"
description = "The interaction between a shell and the Bench environment"
title = "Shells"
weight = 6
+++

[isolation]: /guide/isolation
[launcher]: /guide/launcher
[dashboard shell buttons]: /ref/dashboard/#main-window-shells
[environment script]: /ref/file-structure/#env

The [isolated environment][isolation] of Bench is available either via
starting an app or starting a Bench shell respectively.
Like apps, shells can be started with a [launcher][].
Additionally, shells can be started from the [toolbar][dashboard shell buttons]
in the main window of the Bench Dashboard.
Bench supports three shells: Windows Command Prompt (CMD), PowerShell, and Bash.
<!-- more -->

**Overview**

<!-- #data-list /*/* -->

## Environment {#env}
When a Bench shell is started, the environment variables for the shell process
are modified according to the [isolation settings][isolation] of Bench.
Therefore, inside the shell and all its child processes, the Bench apps
are available and the location of the home directory is [potentially overridden](/ref/config/#HomeDir) and refers to the
[home directory](/ref/file-structure/#home-dir) in the Bench environment.

## Quick Access {#quick-access}
Not all three shells are allways available via the Bench Dashboard and their launchers.
Instead there are configuration properties to control which shells are available.

* [`QuickAccessCmd`](/ref/config/#QuickAccessCmd)
  for making the Windows Command Prompt (CMD) available
* [`QuickAccessPowerShell`](/ref/config/#QuickAccessPowerShell)
  for making the PowerShell available
* [`QuickAccessBash`](/ref/config/#QuickAccessBash)
  for making the Git Bash available

## Drag & Drop Launching {#drag-and-drop}
If the isolation features of Bench are activated, then the Bench apps
are not available if a program is started the usual way from the Explorer,
start menu or via the run dialog (Win+R).

To achieve starting a script or program, inside of the Bench environment,
it can be run, of cause, by starting a shell via its launcher and then
typing in the execution command for the program.
But this can be quite tedious. Instead the script or program file can be dragged
on to the [launcher][] shortcut of a Bench shell.

By doing so, the path of the file is passed through the launcher,
down to the Bench shell and the shell is executing the script or program.
Because the script interpreter or the program is run as a child of the Bench
shell now, the script or program inherits the environment variables of the
Bench shell. And therefore, is executed inside of the Bench environment.

## Shell Entry Points {#entry-points}
For every Bench shell, there exists a batch script to start the shell
inside the Bench environment.

* [`auto\bin\bench-cmd.cmd`](/ref/file-structure/#auto-bin-bench-cmd)
* [`auto\bin\bench-ps.cmd`](/ref/file-structure/#auto-bin-bench-ps)
* [`auto\bin\bench-bash.cmd`](/ref/file-structure/#auto-bin-bench-bash)
  (is only working if the Git app is activated in the Bench environment)

These scripts are always availabe, regardless of the [quick access](#quick-access) properties.

## Loading the Bench Environment with Hindsight {#env-script}
It is possible to load the Bench environment into an already running shell.
This is simply accomplished by running the [environment script][] `env.cmd`.
Because this script is currently only generated as CMD batch file,
this feature is only available for the Windows Command Prompt (CMD).

If there is the need to automatically run scripts or programs inside
of a Bench environment, create the entry point for the automation as a CMD batch
file, call the `env.cmd` first and do everything else, like calling Bench apps,
afterwards.
