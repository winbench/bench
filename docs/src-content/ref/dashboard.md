+++
date = "2016-06-22T13:40:45+02:00"
description = "The graphical user interface for the Bench environment"
title = "Bench Dashboard"
weight = 1
+++

[Main]: /img/Dashboard_Main.png
[Main Toolbar]: /img/Dashboard_MainToolbar.png
[Main Docs Menu]: /img/Dashboard_MainDocsMenu.png
[About]: /img/Dashboard_About.png
[Setup]: /img/Dashboard_Setup.png
[App Info]: /img/Dashboard_AppInfo.png
[App Library]: /img/Dashboard_AppLibrary.png
[Shell]: /guide/shell/
[Isolation Mechanisms]: /guide/isolation

<!-- #data-list /*/* -->

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

The window has a menu bar at the top, underneath a task control section,
in the middle the [app list](#setup-applist) and at the bottom
temporarily a download list and the ConEmu console view.

### Task Control Section {#setup-taskcontrol}
This section displays the currently running task with the last info message
and a progress bar.
Additionally it shows the number of pending tasks.

On the right the _Auto Setup Button_ ![Auto Setup Button](/img/do_16.png) is placed.
Clicking this button starts all tasks necessary to align the actual Bench setup
with the users configuration and app selection.
It removes inactive or deactivated apps, downloads required app resources,
installes active apps and updates launchers and the environment file `env.cmd`.
This button is possibly the most important button in the _Bench Dashboard_.

Clicking the _Auto Setup Button_ ![Cancel Button](/img/stop_16.png)
while a task is running, cancels this task.
But be noted, that not all tasks can be canceled immediately.

### App List {#setup-applist}
The app list shows all apps loaded from the Bench app library and the user app library.
It displays information about the app, its status in the Bench installation,
and allows the activation and deactivation of apps.

The list has the following columns:

* Status Icon
* **Order**
  The number, describing at which place in the app libraries this app was loaded.
  This number defines the order in which apps are processed during Bench tasks, like setup or removal.
* **Name**
  The [label](/ref/app-properties/#Label) of the app.
* **Typ**
  A nice name for the [typ](/ref/app-properties/#Typ) of the app.
* **Active**
  A checkbox to activate an app.
  This checkbox can have three states: Not activated, activated (by the user),
  or the third state, which means the app is implicitely activated by Bench or another app.
* **Deactivated**
  A checkbox to deactive an app.
  This checkbox can have two states: Not deactivated, or deactivated by the user.
* **Status**
  A short description of the app.
* **Version**
  The value of the app property [Version](/ref/app-properties/#Version).
* **Comment**
  A detailed description of the apps status.

Right-clicking a list entry opens a context menu with the following items:

* **Property Details**
  Opens the [app info window](#setup-appinfo).
* **Open Website**
  Opens the URL from the apps [Website property](/ref/app-properties/#Website)
  in the users default browser.  
  (Is only visible, if the Website property is set.)
* **Install**
  Runs the setup task for this app.  
  (Is only visible, if the app is not installed.)
* **Reinstall**
  Runs the removal task and the setup task for this app.  
  (Is only visible, if the app is installed and has cached app resources.)
* **Upgrade**
  Deletes cached resources, downloads resources when needed, and then runs the
  removal and the setup task for this app.  
  (Is only visible, if the app is installed, has a cached resource, and a version number &ndash; or it is managed by a package manager.)
* **Uninstall**
  Runs the removal task for this app.  
  (Is only visible, if the app is installed.)
* **Download Resource**
  Downloads the apps resource file or archive.  
  (Is only visible, if the app has a resource, which is not cached.)
* **Delete Resource**
  Deletes the cached resource of this app.  
  (Is only visible, if the app has a resource, which is cached.)

Double-clicking a list entry opens the [app info window](#setup-appinfo).

## App Info Window {#setup-appinfo}
Double clicking an app in the [app list](#setup-applist) of the [setup window](#setup), openes a window with some detail information about the app.
The app info window shows the values of its [apps properties](/ref/app-properties).

![App Info][]

This window has two tabs:

* **Effective**
  On this tab, the property values are displayed as seen by the Bench system.
  That means that all variable expansion, default value replacement,
  auto configuration, and relative path resolution has taken place.
* **Raw**
  On this tab, the property values are displayed as defined in the `apps.md` file.
  This can be helpful when debugging an apps definition.

## App Library Viewer {#app-libs}
Clicking on _View_ &rarr; _App Repository_ or _View_ &rarr; _Custom Apps_
in the menu of the [setup window](#setup), openes a Markdown viewer for
the app libraries.

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
