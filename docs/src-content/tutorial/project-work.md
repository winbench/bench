+++
date = "2016-11-18T17:50:00+02:00"
description = "Edit and build a project in the Bench environment"
title = "Working on a Project"
weight = 10
+++

[Launcher]: /guide/launcher
[Bench Shell]: /guide/shell
[Selecting And Installing]: /tutorial/apps

When working on a project inside of the Bench environment,
the most important thing is, to go through the [Launchers][Launcher],
when opening an IDE or calling a compiler.
Only then all the required command line tools are on the `PATH`.
<!--more-->

The support for projects in Bench is not very mature yet.
But some support already exists.

**Overview**

<!-- #data-list /*/* -->

## Editor or IDE Based Approach {#ide-centric}
To open a project in a text editor or an IDE which was installed with Bench,
use the [Launcher][] of the editor or the IDE and
open your project from inside the application.

Bench has preliminary support for assisting you with this step:
Run the action script `actions\project-editor.cmd`.
This script opens a menu, where you can select the project you want to work on,
and then trys to recognize the project type.
If the project type is supported by Bench it opens the appropriate IDE or editor.
Otherwise it will open the projects folder in the default editor.

## Shell Based Approach {#shell-centric}
To work on a project in the Bench environment is really easy.
Just open the [Bench Shell][] of your choice and navigate
into your project directory.

A [Bench Shell][] always starts in the Bench root directory.
Therefore, to navigate to your project, just type the following command:

```sh
cd projects/<your project>
```

Replace `<your project>` with the name of your projects directory.

Now you can call whatever command line tool you have installed
in the Bench environment.

Be aware of the fact, that [selecting and installing][]
additional command line tools will not change the `PATH` variable
of a running shell.
When you install new apps, you have to restart a [Bench Shell][],
to get the updated `PATH` variable.

## See Also

Tutorials

* [Creating a New Project](/tutorial/project-new)
* [Importing a Project](/tutorial/project-import)
* [Selecting and Installing Apps](/tutorial/apps)

Tech Guides

* [Launcher](/guide/launcher)
* [Bench Shell](/guide/shell)

Reference Docs

* [File Structure](/ref/file-structure)
