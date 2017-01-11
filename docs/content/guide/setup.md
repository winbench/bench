+++
date = "2016-07-13"
description = "The process of installing or upgrading Bench"
title = "Bench Setup and Upgrade"
weight = 2
+++

[bootstrap file]: /ref/file-structure/#res-bench-install
[site configuration]: /ref/file-structure/#bench-site
[custom configuration]: /ref/file-structure/#config-dir
[Bench CLI]: /ref/bench-cli
[app libraries]: /ref/config/#AppLibs
[required apps]: /app_categories/required
[Bench environment file]: /ref/file-structure/#env
[initialization mode]: /ref/bench-cli/#cmd_bench-manage-initialize
[setup mode]: /ref/bench-cli/#cmd_bench-manage-setup

Bench is installed and upgraded with a [bootstrap file][], which downloads
the archive `Bench.zip` with the Bench system files, extracts its content in the Bench root directory,
and starts the [Bench CLI][] with the [initialization mode][] and the [setup mode][].
The custom configuration is left untouched if it already exists;
otherwise it can be initialized from a template or from a Git repository.
<!--more-->

The setup of a Bench installation is performed in the following steps:

* Creating a directory as Bench root
* Downloading the (latest) [bootstrap file][] in the Bench root
* Running the [bootstrap file][] ...
    + Downloading the (latest) Bench archive (`Bench.zip`)
    + Deleting the following folders in the Bench root:
      `actions`, `auto`, `res`, `tmp`, `cache\_applibs`, `lib\_applibs`,
      and the folders of the [required apps][] in `lib`
    + Extracting the Bench archive
* Running the [Bench CLI][] in [initialization mode][] ...
    + Initializing the [site configuration][]
    + Downloading missing [app libraries][]
    + Downloading missing app resources for [required apps][]
    + Installing the [required apps][]
    + Initializing the [custom configuration][] from template or existing Git repository
    + Downloading missing [app libraries][]
* Running the [Bench CLI][] in [setup mode][] ...
    + Downloading missing app resources for activated apps
    + Installing activated apps
    + Writing the [Bench environment file][]
    + Creating Launcher shortcuts

Running the [Bench CLI][] in [upgrade mode](/ref/bench-cli/#cmd_bench-manage-upgrade)
performs all steps listed above, except creating the Bench root directory.
