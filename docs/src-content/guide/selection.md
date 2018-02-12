+++
date = "2018-02-09T12:00:00+02:00"
description = "The activation of apps, app groups and their dependencies"
title = "App Activation"
weight = 5
+++

[User App Library]: /ref/file-structure/#config-apps
[Activated Apps]: /ref/file-structure/#config-apps-activated
[Deactivated Apps]: /ref/file-structure/#config-apps-deactivated
[Meta App]: /ref/app-types/#meta
[Bench Dashboard]: /ref/dashbord/
[Mastersign.Bench.ActivationFile]: https://winbench.org/clr-api/html/T_Mastersign_Bench_ActivationFile.htm

Seleting an app for installation by Bench is called activating the app.
Apps can be activated explicitly by the user, or they can be activated
implicitly because they are required by Bench or another app.
An activated app can be explicitly deactivated by the user.
The activation and deactivation of apps is stored in two simple
text files.
<!--more-->

Usually the activation of apps is enough.
But under some rare circumstances it can be helpful to suppress
a required or implicitely activated app.
Only then it is necessary to use the feature for deactivating an app.

**Overview**

<!-- #data-list /*/* -->

## The List of Active Apps
When Bench runs its tasks for download, app setup, environment setup,
app removal, and others, it uses a list of active apps to know which apps
to setup or remove.
To compile this list, Bench reads the following files

* `lib\applibs\<app lib name>\apps.md`
* [`config\apps.md`][User App Library]
* [`config\apps-activated.txt`][Activated Apps]
* [`Config\apps-deactivated.txt`][Deactivated Apps]

The compilation works in the following order:

1. At first Bench reads the loaded app libraries `lib\applibs\*\apps.md`
2. Next it reads the [User App Library][] `config\app.md` to load overrides
   and user defined apps.
3. Some apps in the bench core app library are listed in the app category
   _Required_ and are thereby activated implicitely.
4. Bench than reads the [user activated apps][Activated Apps] in
   `config\apps-activated.txt` and activates the listed apps explicitely.
5. Bench than evaluates the dependencies of the activated apps recursively,
   and implicitely activates all apps listed as dependencies for
   activated apps.
6. At last Bench reads the [deactivated apps][Deactivated Apps] in
   `config\app-deactivated.txt` and deactivates the listed apps explicitely.

The list of active apps consists of all apps, which are activated
&ndash; explicitely or implicitely &ndash;
and are not deactivated.

## App groups
Some apps of the type [Meta App][] are app groups.
That means, they have no app resources and no executables, instead they
just have a list of dependencies.
If such an app is activated, all dependencies are activated implicitely.

This pattern is useful to group a number of apps for some scenario under
one descriptive name.

## Activation and Deactivation
The user currently has two options to activate or deactivate an app.
The first one is by using the setup dialog of the [Bench Dashboard][].
The second one is to edit the text files [`apps-activated.txt`][Activated Apps]
and [`apps-deactivated.txt`][Deactivated Apps] with a text editor and run
the Bench auto setup afterwards.

### Activation with Bench Dashboard
To activate or deactivate an app in the [Bench Dashboard](/ref/dashboard),
open the [setup dialog](/ref/dashboard/#setup) and just click on the checkboxes
in the [app list](/ref/dashboard/#setup-applist).
After making your app selection, you have to run the _Bench Auto Setup_,
to make the needed changes to the Bench installation.

![Bench Dashboard App Selection](/img/Dashboard_SetupSelection.png)

### Activation by Editing Text Files
The two text files for activation [`apps-activated.txt`][Activated Apps]
and deactivation [`apps-deactivated.txt`][Deactivated Apps]
are stored in the [user configuration directory](/ref/file-structure/#config-dir)
and must meet the following rules.

* The file must be an UTF8 encoded text file.
* Every non empty line, which is not commented with a `#` is interpreted as an app ID.
* Only non-space characters, up to the first space or the end of a line, are considered.

This example shows a possible text file for listing apps
`AppA`, `AppB`, `AppC`, and `AppE`:

```
# --- Activated Apps --- #

AppA
AppB this app has a comment
 AppC (this app ID is valid, despite the fact, that it is indended)

# AppD (this app is not activated, because the line is commented out)
AppE # how a comment after the app ID starts is irrelevant
     # but a # sign is recommended
```

The parsing of such app list is implemented in [Mastersign.Bench.ActivationFile][].
