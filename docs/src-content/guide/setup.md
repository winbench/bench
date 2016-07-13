+++
date = "2016-06-22T13:05:14+02:00"
description = "The process of installing or upgrading Bench"
draft = true
title = "Bench Setup and Upgrade"
weight = 2
+++

The setup of the Bench system is performed in the following steps:

* Creating a directory as Bench root
* Downloading the (latest) [bootstrap file][] in the Bench root
* Running the [bootstrap file][] ...
    + Downloading the (latest) Bench archive (`bench.zip`) from GitHub
    + Deleting the following folders in the Bench root: `actions`, `auto`, `res`, `lib`, `tmp`
    + Extracting the Bench archive
* Running the Bench CLI in [initialization mode](/ref/bench-ctl/#initialize) ...
    + Initializing the [site configuration][]
    + Downloading missing app resources for [required apps][]
    + Installing the [required apps][]
    + Initializing the [custom configuration][]
* Running the Bench CLI in [setup mode](/ref/bench-ctl/#setup) ...
    + Downloading missing app resources for activated apps
    + Installing activated apps
    + Writing the [Bench environment file][]
    + Creating Launcher shortcuts

Running the Bench CLI in [upgrade mode](/ref/bench-ctl/#upgrade)
performs all steps listed above, except creating the Bench root directory.

[bootstrap file]: /ref/file-structure/#res-bench-install
[site configuration]: /ref/file-structure/#bench-site
[custom configuration]: /ref/file-structure/#config-dir
[required apps]: /ref/apps/#apps-required
[Bench environment file]: /ref/file-structure/#env
