+++
date = "2016-11-18T18:35:00+02:00"
description = "How do I use a Bench app in my project?"
title = "Working in a Project"
weight = 4
+++

[Working in a Project]: /tutorial/project-work/
[Launcher]: /guide/launcher
[Bench Shell]: /guide/shell
[App List]: /apps

The only important thing, when working on a project in the Bench environment,
is to start the IDE or text editor through its [Launcher][],
and to call command line tools from a [Bench Shell][].

When working with an IDE or a full-featured text editor, start it via
its [Launcher][] in the Bench Dashboard or its shortcut in the `launchers`
directory. Then open your project from within the IDE or text editor.
This way, the process of the IDE or editor has the Bench apps on the `PATH`.

When working on the command line, just open a [Bench Shell][] from
the Bench Dashboard or via its shortcut in the `launchers` directory.
Change the current directory to your project, which mostly is a sub-directory
in the `projects` directory, and use the command line tools you like.
If you miss a command line tool, checkout the [app list][], maybe it is allready
listed and you can [just activate it](/tutorial/apps).

You can find a more detailed description of working in a project in the tutorial
[Working in a Project][].
