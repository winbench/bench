+++
date = "2016-06-22T11:21:28+02:00"
description = "The command-line interface: bench-ctl"
draft = true
title = "Bench CLI"
weight = 2
+++

The _Bench_ system can be managed and upgraded with the `bench-ctl.cmd` script in the `actions` folder of _Bench_.

## Execution Modes
Depending on the number of arguments, given to `bench-ctl`, the tool runs in one of the following modes.

### Interactive `bench-ctl`
You can run `bench-ctl` without any arguments or by just double clicking it in the Windows Explorer,
to run it in interactive mode.
In the interactive mode, you are presented with all available tasks
and you can select a task to run, by typing an indicator key.

### Automation `bench-ctl <task>`
You can specifiy a task as the first argument to `bench-ctl`.
The specified task is executed immediatly without further interaction with the user.

## Tasks

### `update-env`
If you have moved the _Bench_ directory, or you are using _Bench_ on a USB drive and it has a new drive letter,
you must update the environment file `env.cmd` and the launcher shortcuts.

The `update-env` task does exactly that.

### `setup`
If you changed your configuration in the `config` folder by activating more apps or altered
some settings, and also if an error occured during the initial installation of _Bench_,
the `setup` task is the right choice to complete the app installation.

This tasks performs the following steps:

* Initializing the custom configuration, if there is none
* Downloading missing app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `env.cmd`
* Updating the launcher shortcuts

Usually, you need an internet connection for this script.
This script usally can be run repeatedly without any riscs.

### `download`
If you want to download missing application resources without installing
any application in the _Bench_ environment, use this script.
It performs the following steps:

* Initializing the custom configuration, if there is none
* Download missing app resources

Running `download` prior to `setup` or `reinstall` prefetches app resources to
reduce the time required later-on by `setup` or `reinstall`.

However, be aware, that downloading the app resources is mostly not sufficient
to run the tasks `setup` or `reinstall` afterwards, without an internet connection.
This is because NPM and PIP packages are not downloaded as app resources,
but downloaded by the package manager during installation.

### `reinstall`
If your installed apps are corrupted, or you want to update
NPM or PIP packages, you can run this script.
It performs the following steps:

* Removing all installed app files
* Downloading missing app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `env.cmd`
* Updating the launcher shortcuts

Usually, you need an internet connection for this script.

### `renew`
If you want to start fresh with your app selection, you can run this task.
It performs the following steps:

* Removing all downloaded app resource
* Removing all installed app files
* Downloading missing app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `env.cmd`
* Updating the launcher shortcuts

You need an internet connection for this script.

### `upgrade`
If you want to upgrade the whole _Bench_ environment, you can run this task.
It performs the following steps:

* Downloading and executing the latest bootstrap file `bench-install.cmd`
    + Downloading the archive `Bench.zip` of the latest release
    + Deleting the directories `actions`, `auto`, `res`, `lib` and `tmp`
    + Extracting the content of `Bench.zip`
    + Starting the app setup...
* Downloading all required app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `env.cmd`
* Updating the launcher shortcuts

If the internet connection is not stable, or some of the app resources are not available,
this script leaves you with a possible unusable environment.
But you can allways run  the `setup` and `reinstall` tasks in an attempt to repair the missing apps.

This script does not touch any user data in the _Bench_ home directory,
and it does not touch the custom configuration (`config.md`, `apps.md`, ...)
in the `config` folder either.
