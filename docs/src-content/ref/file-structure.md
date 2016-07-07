+++
date = "2016-06-22T13:43:12+02:00"
description = "The layout of the folders and files in the Bench environment"
draft = true
title = "File Structure"
weight = 4
+++

The file structure of Bench is devided in two groups, the core and the extended file structure.
If the location of files and directories is not changed via custom or site configuration,
all files and folders of Bench live in one directory: the _root directory of Bench_.
In the following sections, the file structure is described according to the default configuration.
If a directory or file can be configured to be elsewhere, the responsible
configuration property is mentioned.

## Core Structure {#core}

The core structure consists of directories and files, which are installed
during the Bench setup, and _can not_ be moved via custom or site configuration.

* `actions` ([ActionDir](/ref/config/#ActionDir))  
  This directory contains `*.cmd` scripts to run a couple of useful tasks
  in the Bench environment.
  They do load the `env.cmd` by themselfs if necessary. Therefore,
  they can be started directly from an arbitrary command prompt, via a Windows shortcut,
  or from the explorer.
    + `bench-bash.cmd`
    + `bench-cmd.cmd`
    + `bench-ctl.cmd`
    + `bench-ps.cmd`
    + `clone-git-project.cmd`
    + `new-project.cmd`
    + `open-editor.cmd`
    + `project-backup.cmd`
    + `project-editor.cmd`
    + `project-ps.cmd`
    + `project-watch.cmd`
* `auto`
    + `apps`
        - `<app-id>.extract.ps1`
        - `<app-id>.setup.ps1`
        - `<app-id>.env.ps1`
        - `<app-id>.remove.ps1`
    + `bin`
        - `BenchDashboard.exe`
        - `BenchLib.dll`
    + `lib`
        - `bench.lib.ps1`
        - ...
    + `archive.cmd`
    + `editor.cmd`
    + `init.cmd`
    + `runps.cmd`
* `config` ([CustomConfigDir](/ref/config/#CustomConfigDir))
    + `apps.md` ([CustomAppIndexFile](/ref/config/#CustomAppIndexFile))
    + `apps-activated.txt` ([AppActivationFile](/ref/config/#AppActivationFile))
    + `apps-deactivated.txt` ([AppDeactivationFile](/ref/config/#AppDeactivationFile))
    + `config.md` ([CustomConfigFile](/ref/config/#CustomConfigFile))
    + `ConEmu.xml` ([ConEmuConfigFile](/ref/config/#ConEmuConfigFile))
    + `env.ps1`
    + `setup.ps1`
* `res`
    + `apps` ([AppResourceBaseDir](/ref/config/#AppResourceBaseDir))
    + `apps.template.md` ([CustomAppIndexTemplateFile](/ref/config/#CustomAppIndexTemplateFile))
    + `apps-activated.template.txt` ([AppActivationTemplateFile](/ref/config/#AppActivationTemplateFile))
    + `apps-deactivated.template.txt` ([AppDeactivationTemplateFile](/ref/config/#AppDeactivationTemplateFile))
    + `bench-install.bat`
    + `bench-site.template.md` ([SiteConfigTemplateFile](/ref/config/#SiteConfigTemplateFile))
    + `ConEmu.template.xml` ([ConEmuConfigTemplateFile](/ref/config/#ConEmuConfigTemplateFile))
    + `config.md`
    + `config.template.md` ([CustomConfigTemplateFile](/ref/config/#CustomConfigTemplateFile))
    + `version.txt` ([VersionFile](/ref/config/#VersionFile))
* `lib` ([LibDir](/ref/config/#LibDir))
* `CHANGELOG.md`
* `env.cmd`
* `LICENSE.md`
* `README.md`

## Extended Structure {#extended}

The extended structure consists of directories and files, which are created
during the usage of Bench &ndash; including the installation of apps,
and _can_ be moved via custom or site configuration.

* `archive`
* `cache`
* `home`
    + `AppData`
        - `Local`
        - `Roaming`
    + `Desktop`
    + `Documents`
    + ...
* `launcher`
* `log`
* `projects`
* `tmp`
* `bench-site.md` ([SiteConfigFileName](/ref/config/#SiteConfigFileName))
