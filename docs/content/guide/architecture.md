+++
date = "2016-06-21T16:34:13+02:00"
description = "The components and their relations"
draft = true
title = "System Architecture"
weight = 1
+++

The Bench system consists of a file system layout, a CLR binary, the Dashboard
and a couple of CMD batch and PowerShell scripts.
Since the architecture is constantly evolving until Bench hits the version 1.0
this article contains only a brief description of the current state.
<!--more-->

**Overview**

* [Folder Layout](#fs)
* [Interface Scripts](#interface-scripts)
* [Power-Shell Scripts](#ps-scripts)
* [Bench Core Binary](#core)
* [Bench Dashboard](#dashboard)

## Folder Layout {#fs}
Bench uses a file system layout with three folder groups.
For a detailed description of the file structure see:
[File Structure Reference](/ref/file-structure).

### Bench System {#fs-bench}
The first group contains scripts, binaries and resources
of the Bench system itself.
This group is replaced in case of an upgrade.

* [`actions`](/ref/file-structure/#action-dir)
* [`auto`](/ref/file-structure/#auto-dir)
* [`res`](/ref/file-structure/#res-dir)

### User Configuration and Data {#fs-user}
The second group contains the user configuration
and user data.
This group is not changed by any Bench operation
like upgrade, app installation, or app removal.

* [`config`](/ref/file-structure/#config-dir)
* [`home`](/ref/file-structure/#home-dir)
* [`projects`](/ref/file-structure/#projects-dir)
* [`log`](/ref/file-structure/#log-dir)
* [`bench-site.md`](/ref/file-structure/#bench-site)

### Temporary Files and App Resources {#fs-work}
And the third group contains the installed apps
and temporary data.
This group can be deleted without any actual data loss,
because it is rebuild by Bench during the app installation
automatically.

* [`launcher`](/ref/file-structure/#launcher-dir)
* [`lib`](/ref/file-structure/#lib-dir)
* [`tmp`](/ref/file-structure/#tmp-dir)
* [`env.cmd`](/ref/file-structure/#env)
* `BenchDashboard.lnk`

## Interface Scripts {#interface-scripts}
Bench provides a couple of scripts to perform actions
in the Bench environment.
Examples are

## Power-Shell Scripts {#ps-scripts}

## Bench Core Binary {#core}

## Bench Dashboard {#dashboard}
