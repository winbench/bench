+++
date = "2016-11-17T15:33:00+02:00"
description = "Upgrade apps to a new version"
title = "Upgrading Apps"
weight = 3
+++

[Bench Dashboard]: /ref/dashboard
[Setup Window]: /ref/dashboard/#setup
[Auto Setup Button]: /ref/dashboard/#setup-taskcontrol
[App List]: /ref/dashboard/#setup-applist
[Bench CLI]: /ref/bench-cli
[Bench Shell]: /guide/shell

The most apps, included in bench, are defined with a specific version number.
You need to upgrade the whole Bench system, to get the latest Bench app library
with newer app versions. But some apps are defined without a version number,
and can be upgraded at any time.
And then there are managed packages like NPM or PIP packages.
The app definitions for these can define a range of versions and allow
upgrading the package in the given version interval too, without retrieving
a new app library.
And last but not least, you can override the version number of an app in
the user app library, if the Bench app library is outdated.
<!--more-->

**Overview**

<!-- #data-list /*/* -->

## Just give me the latest and greatest {#bench-upgrade}
If you just want to upgrade all apps to the latest versions known to Bench,
[upgrade the whole Bench system](/tutorial/upgrade).
If you want to upgrade individual apps read the following sections.

## Upgrade Default Apps without Version Number {#without-version}
This section describes how you can upgrade a [default app](/ref/app-types/#default),
which has no [Version](/ref/app-properties/#Version)
specified or has a version set to `latest`.

### Upgrading Comfortably {#without-version-dashboard}
Make shure the app in question is not running at this point.
Open the [Setup Window][] in the [Bench Dashboard][] and scroll in the
[App List][] to the app.
Right-click in its row to open the context menu and select _Upgrade_.

As a result, the apps resource is deleted from the download cache
and downloaded again. Then the app is deinstalled and reinstalled
with the freshly downloaded resource.
This process does not guarantee, that actually a higher version
is installed than before.
It just guarantees, that the installed version is the one,
currently available under the download URL of the app.

### Upgrading Manually {#without-version-manually}
Make shure the app in question is not running at this point.
Find the apps folder in the [`lib`](/ref/file-structure/#lib-dir)
directory and delete it with all its content.
Then find the apps resource in the download [`cache`](/ref/file-structure/#cache-dir)
directory and delete it too.

Now you just need to kick-off the automatic setup process with the
[Bench CLI][] by executing the [setup action](/ref/bench-cli/#cmd_bench-manage-setup).
E.g. if your Bench root folder is `C:\bench`, then run:

```cmd
cd C:\bench\auto\bin
.\bench manage setup
```

## Upgrade Packaged Apps {#packages}
Apps can be packages, managed by some kind of package manager like
[NPM](/ref/app-types/#node-package) or [PIP](/ref/app-types/#python-package).
If the app definition of these apps has no [Version](/ref/app-properties/#Version)
specified, has a version set to `latest`, or has a version set to a range,
then the package can be upgraded by removing it and installing it again.
Thereby, installing the highest version available and in the possibly
specified version range.

### Upgrading Comfortably {#packages-dashboard}
Make shure the app in question is not running at this point.
Open the [Setup Window][] in the [Bench Dashboard][] and scroll in the
[App List][] to the app.
Right-click in its row to open the context menu and select _Upgrade Package_.

As a result, the app is deinstalled and reinstalled by the package manager.
This process does not guarantee, that actually a higher version
is installed than before.
It just guarantees, that the installed version is the highest one,
available through the package manager, considering the possible given
version range.

### Upgrading Manually {#packages-manually}
You can allways use a package manager directly to upgrade a package app.
Open your favorite [Bench shell][] (CMD, PowerShell, or Bash) and
call the responsible package manager with the appropriate arguments.

## Override an Apps Version {#override}
If an app is defined in the Bench app library with a fixed version number,
you can try to change the version number by overriding it in your
user app library.
Checkout the app definition in the [Bench App Library](/ref/file-structure/#res-apps)
`res\apps.md` to figure out how the `Version` property is used for this particular app.

Open the [User App Library](/ref/file-structure/#config-apps) `config\apps.md`
in your favorite text editor and append a new section locking like this:

```md
### Name of the App

* `ID`: <AppID>
* `Version`: <X.Y.Z>
```

Replace `<AppID>` with the ID of the app you want to override,
and replace `<X.Y.Z>` with your desired version number of course.
The headline is actually ignored by Bench, but helps to organize the file.
The `ID` property starts the app definition.
And the `Version` property overrides the value, specified in the Bench app library.

But be aware, that just changing the version number, does not always work
that easily. Sometimes the download URLs change from one version to another
in an unanticipated way and the apps definition gets invalid.

Read more about defining apps in the tutorial [Custom Apps](/tutorial/apps-custom).

**Pro Tip:**
You can override any app property in this way.

Now you can kick-off the auto setup via the
[Auto Setup Button][] ![Auto Setup Button](/img/do_16.png) in the
[Bench Dashboard][] or the [setup action](/ref/bench-cli/#cmd_bench-manage-setup) of the [Bench CLI][],
to upgrade the app.

**Pro Tip:**
If the Bench Dashboard is already started and the setup window is open,
you can even observe Bench reloading the configuration automatically
when you change a configuration file.

## See Also

Tech Guides

* [App Setup and Upgrade](/guide/app-setup)

Reference Docs

* [File Structure](/ref/file-structure)
* [Bench Dashboard](/ref/dashboard)
* [Markdown List Syntax](/ref/markup-syntax)
* [App Properties](/ref/app-properties)
