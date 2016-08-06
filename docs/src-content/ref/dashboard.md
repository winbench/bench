+++
date = "2016-06-22T13:40:45+02:00"
description = "The graphical user interface for the Bench environment"
draft = true
title = "Bench Dashboard"
weight = 3
+++

[Main]: /img/Dashboard_Main.png
[Main Toolbar]: /img/Dashboard_MainToolbar.png
[Main Docs Menu]: /img/Dashboard_MainDocsMenu.png
[About]: /img/Dashboard_About.png
[Setup]: /img/Dashboard_Setup.png
[App Library]: /img/Dashboard_AppLibrary.png
[Shell]: /guide/shell/
[Isolation Mechanisms]: /guide/isolation

## Main Window {#main-window}
The main window is displayed when the dashboard is started.
It has a toolbar at the top, a status bar at the bottom,
and a list view control with app launchers in the middle area.

![Bench Dashboard Main Window][Main]

### Toolbar {#main-window-toolbar}
The toolbar contains the [shell start buttons](#main-window-shells) on the left,
and buttons for the [documentation menu](#main-window-docs-menu),
the [setup window](#setup), and the [about window](#about)
on the right.

![Bench Dashboard Main Toolbar][Main Toolbar]

### Shell Buttons {#main-window-shells}
The shell buttons open a different kind of [command interpreter][Shell] each.
Their appearance depends on the following configuration properties

* [`QuickAccessCmd`](/ref/config/#QuickAccessCmd)
  for displaying a button for the Window CMD
* [`QuickAccessPowerShell`](/ref/config/#QuickAccessPowerShell)
  for displaying a button for the PowerShell
* [`QuickAccessBash`](/ref/config/#QuickAccessBash)
  for displaying a button for the Git Bash

The command interpreters are started in the Bench environment,
with all the command line tools from Bench apps on the `PATH`,
and the other configured [isolation mechanisms][] in place.

### Documentation Menu {#main-window-docs-menu}
Clicking on the documenation button
![Documentation Menu Button](/img/docs_16.png)
in the [toolbar](#main-window-toolbar) of the [main window](#main-window),
opens a menu with all documentation links of the active apps.

![Documentation Menu][Main Docs Menu]

The first entry is a link to the Bench documentation website.
The entry for an app itself links to its main website, defined
by the app property [`Website`](/ref/app-properties/#Website).
Sub entries are defined by the dictionary in the app property
[`Docs`](/ref/app-properties/#Docs).

### Launcher List {#main-window-launchers}
The launchers in the middle area of the [main window](#main-window)
can be double clicked, to start the
[launcher executable](/ref/app-properties/#LauncherExecutable)
of the related app.

### Status Bar {#main-window-statusbar}
In the status bar, the root directory of the Bench installation is displayed.
This path is a link, which can be clicked to open the Windows Explorer
in the root directory of Bench.

Additionally the number of active apps is displayed.

## Setup Window {#setup}
Clicking on the setup button
![Setup Button](/img/setup_16.png)
in the [toolbar](#main-window-toolbar) of the [main window](#main-window),
opens the _Setup Window_.

![Bench Dashboard Setup Window][Setup]

TODO

## App Libraries {#app-libs}
By clicking on
_View_ &rarr; _App Repository_ or _View_ &rarr; _Custom Apps_
in the menu of the [setup window](#setup), a Markdown viewer for the app
libraries can be opened.

![App Library Viewer][App Library]

It has an index on the left to quickly browse through the library.
This viewer essentially just renders the Markdown files
`res\apps.md` and `config\apps.md` to HTML and displays them in a Browser control.
This is helpful to read additional information written alongside the app properties.

## About Window {#about}
Clicking on the info button
![Info Button](/img/info_16.png)
in the [toolbar](#main-window-toolbar) of the [main window](#main-window),
opens the _About Window_.

![Bench Dashboard About Window][About]

On the right at the top of the window, the current Bench version is displayed.
In the main area you can find some acknowledgements and the license informations
about Bench itself and included third party components.
On the left at the bottom of the window is a link to the authors homepage.
