+++
date = "2016-11-16T17:29:00+02:00"
description = "Activate and install apps in different ways"
title = "Selecting and Installing Apps"
weight = 2
+++

[Setup Window]: /ref/dashboard/#setup
[Auto Setup Button]: /ref/dashboard/#setup-taskcontrol
[App List]: /ref/dashboard/#setup-applist
[App Activation List]: /ref/file-structure/#config-apps-activated
[Bench App Library]: /ref/file-structure/#res-apps
[User App Library]: /ref/file-structure/#config-apps
[Bench CLI]: /ref/bench-ctl

Apps are defined in the Bench app library or in your custom app library.
An app definition contains an ID, a download URL or the identifier
of a managed package, and some additional information for Bench.
If you want to use an app in the Bench environment, it must be activated
and installed.
You can activate and install an app via the Bench Dashboard
or by editing a text file and running a command at the shell.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

A shortened version of this tutorial can be found in the
[Quickstart](/start/apps).

## Activating Apps by Mouse {#via-dashboard}
The easiest way to activate an app is to use the Bench Dashboard.
It is automatically shown at the end of the Bench setup.
To open it manually, you can use the shortcut in the Bench root folder
or in the `launcher` folder.

In the Bench Dashboard open the [Setup Window][] by clicking on the
wrench in the [toolbar](/ref/dashboard/#main-window-toolbar).
To activate an app with all its dependencies, just check its box
in the column _Active_ of the [App List][].

Some apps are groups &ndash; meaning the app itself has no executables,
instead it activates a number of other apps as dependencies.
You can activate as many groups and apps as you want.

To kick-off the installation of all activated apps, just click
the [Auto Setup Button][] ![Auto Setup Button](/img/do_16.png).
When the download and installation has finished, you can close the
setup window.

## Activating Apps by Keyboard {#via-shell}
If you want to automate the app activation process, or just want to know
how things work behind the scenes, you can edit the configuration directly.

Open the [App Activation List][] `config\apps-activated.txt`
in you favorite text editor.
Every non empty line, not commented out by a leading `#`,
is treated as an app ID.
For more detailed information about the app activation list take a look
at the tech guide for [App Activation](/guide/selection).

Example:

```
ActiveAppID

# InactiveAppID
```

If you do not know the IDs of the available apps, take a look at the
[Bench App Library][] in `res\apps.md` and the
[User App Library][] in `config\apps.md`.

To kick-off the installation of the activated apps, run the [Bench CLI][]
with the [setup action](/ref/bench-ctl/#setup).
E.g. if your Bench root folder is `C:\bench`, then run:

```cmd
cd C:\bench\actions
bench-ctl setup
```

Do not be alarmed, if this process takes a while, because the activated
apps must be downloaded and extracted.
Depending on the bandwidth of your internet connection and your CPU speed,
this can take a while.
If you want more information during the setup process, add `verbose`
as second command line argument: `bench-ctl setup verbose`.

## Next {#next}
Now you can [delete](/tutorial/apps-remove)
or [upgrade](/tutorial/apps-upgrade) apps,
[define your own](/tutorial/apps-custom) apps,
[import](/tutorial/project-import)
or [create](/tutorial/project-new) a project,
and [start working](/tutorial/project-work).

## See Also

Tech Guides

* [App Activation](/guide/selection)
* [App Setup and Upgrade](/guide/app-setup)
* [Launcher](/guide/launcer)
* [Shells](/guide/shell)

Reference Docs

* [App Library](/ref/apps)
* [File Structure](/ref/file-structure)
* [Bench Dashboard](/ref/dashboard)
