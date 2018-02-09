+++
date = "2018-02-09T12:00:00+02:00"
description = "Activate and install apps in different ways"
title = "Selecting and Installing Apps"
weight = 2
+++

[Setup Window]: /ref/dashboard/#setup
[Auto Setup Button]: /ref/dashboard/#setup-taskcontrol
[App List]: /ref/dashboard/#setup-applist
[App Activation List]: /ref/file-structure/#config-apps-activated
[User App Library]: /ref/file-structure/#config-apps
[Bench CLI]: /ref/bench-cli

Apps are defined in one of the loaded app libraries or in your custom app library.
An app definition contains an ID, a download URL or the identifier
of a managed package, and some additional information for Bench.
If you want to use an app in the Bench environment, it must be activated
and installed.
You can activate and install an app via the Bench Dashboard,
by editing a text file, or running a command at the shell.
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
how things work behind the scenes, you can use the [Bench CLI][]
or edit the configuration directly.

To activate an app on the command line type `bench app activate <ID>`.
Replace `<ID>` with the ID of the app.
If you do not know the IDs of the available apps, take a look at
the [app list](/apps) or run `bench list -t apps -p ID,Label,Version`.

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

To kick-off the installation of the activated apps, run
`bench manage setup`.

Do not be alarmed, if this process takes a while, because the activated
apps must be downloaded and extracted.
Depending on the bandwidth of your internet connection, your CPU and harddisk
speed, this take more or less time.
If you want more information during the setup process, add the verbose flag:
`bench -v manage setup`.

## Next {#next}
Now you can [delete](/tutorial/apps-remove)
or [upgrade](/tutorial/apps-upgrade) apps,
[define your own](/tutorial/apps-custom) apps,
and [start working on a project](/tutorial/project-work).

## See Also

Tech Guides

* [App Activation](/guide/selection)
* [App Setup and Upgrade](/guide/app-setup)
* [Launcher](/guide/launcer)
* [Shells](/guide/shell)

Reference Docs

* [App Library](/apps)
* [File Structure](/ref/file-structure)
* [Bench Dashboard](/ref/dashboard)
