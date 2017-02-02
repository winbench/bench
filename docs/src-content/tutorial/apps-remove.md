+++
date = "2016-11-17T15:34:00+02:00"
description = "Deactivate and delete apps with their resources"
title = "Removing Apps"
weight = 4
+++

[Setup Window]: /ref/dashboard/#setup
[Auto Setup Button]: /ref/dashboard/#setup-taskcontrol
[App List]: /ref/dashboard/#setup-applist
[App Activation List]: /ref/file-structure/#config-apps-activated
[App Deactivation List]: /ref/file-structure/#config-apps-deactivated
[Bench CLI]: /ref/bench-cli

If you do not need an app anymore installed in your Bench environment,
you can just deselect and remove it.
You can do this with the Bench Dashboard or you can edit the configuration
files yourself and call a command on the shell to deinstall the app.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

## Deactivating Apps by Mouse {#dashboard}
The easiest way to deactivate an app is to use the Bench Dashboard.
To open it, you can use the shortcut in the Bench root folder
or in the `launcher` folder.
In the Bench Dashboard open the [Setup Window][] by clicking on the
wrench in the [toolbar](/ref/dashboard/#main-window-toolbar).

To deactivate an app, which was activated by you explicitly,
just uncheck its box in the column _Active_ of the [App List][].
If an app was activated implicitly as a dependency and you still
want to deactivate it, you can check its box in the column _Deactivated_.
But be aware, that apps, depending on this deactivated app,
propably will not work properly.

To kick-off the deinstallation of apps which are installed, but not activated,
just click the [Auto Setup Button][] ![Auto Setup Button](/img/do_16.png).

If you also want to remove downloaded app resources, like archives
or setup programs, from the download cache, you can use the menu
in the [Setup Window][]: _Setup_ &rarr; _Clean-Up Obsolete Resources_.

## Deactivating Apps by Keyboard {#dashboard}
If you want to automate the app removal process, or just want to know
how things work behind the scenes, you can edit the configuration directly.

To deactivate an app, that was activated explicitly,
open the [App Activation List][] `config\apps-activated.txt`
in you favorite text editor and delete or comment out the line
with the apps ID.
To comment out a line, just preceed it with a `#`.
If an app was activated implicitly as a dependency and you still
want to deactivate it,
open the [App Deactivation List][] `config\apps-deactivated.txt`
in you favorite text editor and add a line with the apps ID.
But be aware, that apps, depending on this deactivated app,
propably will not work properly.

If you want to get rid of downloaded app resources to free up disk space,
just locate them in the [download cache](/ref/file-structure/#cache-apps-dir) directory
`cache\apps` and delete them.

To kick-off the deinstallation of apps which are installed, but not activated,
run the [Bench CLI][] with the [setup action](/ref/bench-cli/#cmd_bench-manage-setup).
E.g. if your Bench root folder is `C:\bench`, then run:

```cmd
cd C:\bench\auto\bin
.\bench manage setup
```

If you want more information during the setup process, add `--verbose`
as flag to the command line: `.\bench --verbose manage setup`.

## See Also

Tech Guides

* [App Activation](/guide/selection)
* [App Setup and Upgrade](/guide/app-setup)

Reference Docs

* [File Structure](/ref/file-structure)
* [Bench Dashboard](/ref/dashboard)
