+++
date = "2016-11-11T12:10:00+02:00"
description = "The embedding of a nice console"
title = "ConEmu Integration"
weight = 9
+++

The [setup dialog](/ref/dashboard/#setup) of the dashboard contains a console
panel at the bottom. This console panel shows an embedded ConEmu console.
When command line tools are called during the setup process,
thier output is visible inside of this console.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

## Focus Flicker and Workaround {#focus-flicker}
ConEmu is a very nice console emulator for the Windows environment,
and can even be embedded as a panel into applications.
However, if a process is started in a ConEmu session,
a new Window is created in the background and then hidden afterwards.
When starting a lot of new ConEmu sessions to run command line tools,
a repeating focus switch is caused.
This prohibits the user to use any other program during the setup process,
because the input focus is stolen from him all the time.

Bench implements a workaround for this behavior, by starting only one session
in the ConEmu console, running the PowerShell with `auto\lib\PsExecHost.ps1`.
This script creates a named pipe and awaits execution commands through it.
When the setup process needs to run a command line tool, it passes the path
and the arguments through the named pipe and `PsExecHost.ps1` runs the tool
in its ConEmu session.
The result is a nicely embedded execution of command line tools in the bottom
panel of the setup dialog with full support for colors etc.

## Fallback {#fallback}
When the ConEmu app is not installed at some point and the dashboard is started,
a fallback behavior is implemented, where command line tools are simply executed
through the Windows shell, which results in a new console window for each
executed command line tool.
