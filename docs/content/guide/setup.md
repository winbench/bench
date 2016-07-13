+++
date = "2016-07-13"
description = "The process of installing or upgrading Bench"
title = "Bench Setup and Upgrade"
weight = 2
+++

[bootstrap file]: /ref/file-structure/#res-bench-install
[site configuration]: /ref/file-structure/#bench-site
[custom configuration]: /ref/file-structure/#config-dir
[Bench CLI]: /ref/bench-ctl
[required apps]: /ref/apps/#apps-required
[Bench environment file]: /ref/file-structure/#env
[initialization mode]: /ref/bench-ctl/#initialize
[setup mode]: /ref/bench-ctl/#setup

Bench is installed and upgraded with a [bootstrap file][], which downloads
the archive `Bench.zip` with the Bench system files, extracts its content in the Bench root directory,
and starts the [Bench CLI][] with the [Initialization mode][] and the [setup mode][].
The custom configuration is left untouched if it is already exists;
otherwise it can be initialized from a template or from a Git repository.
<!--more-->

The full setup of the Bench system is performed in the following steps:

* Creating a directory as Bench root
* Downloading the (latest) [bootstrap file][] in the Bench root
* Running the [bootstrap file][] ...
    + Downloading the (latest) Bench archive (`Bench.zip`) from GitHub
    + Deleting the following folders in the Bench root: `actions`, `auto`, `res`, `lib`, `tmp`
    + Extracting the Bench archive
* Running the Bench CLI in [initialization mode][] ...
    + Initializing the [site configuration][]
    + Downloading missing app resources for [required apps][]
    + Installing the [required apps][]
    + Initializing the [custom configuration][] from template or existing Git repository
* Running the Bench CLI in [setup mode][] ...
    + Downloading missing app resources for activated apps
    + Installing activated apps
    + Writing the [Bench environment file][]
    + Creating Launcher shortcuts

Running the Bench CLI in [upgrade mode](/ref/bench-ctl/#upgrade)
performs all steps listed above, except creating the Bench root directory.
