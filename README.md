# Bench

```
      _____________________________________
     /   _    __       __                 /|
    /   /_)  /_  /| / /   /_/            //
   /   /_)  /_  / |/ |_  / /   o        //|
  /____________________________|_______//||
  |____________________________O______|/ ||
    | || | ||                  |  | || | ||
    | || |_|/                  °  | || |_|/
    | ||                          | ||
    | ||      LET'S GET BUSY!     | ||
    |_|/                          |_|/
```

> Portable Environment for Software Development on Windows

## Prerequisites

* Microsoft Windows 7 or higher (with at least PowerShell 2)
* Internet access for HTTP/HTTPS (possibly via proxy, but without credentials)
* The Windows user account does _not_ need admin priviledges
* A harddrive or a removable drive with at least 2 GB free memory

## Install

* Create a folder on a drive with at least 2 GB space, maybe call it `bench`.
* Save the bootstrap file [bench.bat](https://github.com/mastersign/bench/raw/master/res/bench.bat)
  inside this folder.
* Run the bootstrap file.
  (You probably have to open the properties dialog for the downloaded file, and allow the execution first.)
* Follow the instructions ...
    + Enter your name and email address for your [Git] identity.
    + Configure the HTTP proxy server in the `config.ps1` if you are behind a proxy firewall.
    + Maybe activate or deactivate some apps in the `config.ps1`.
    + Wait for _Bench_ to  download resources.  
      (Depending on the number and size of the activated apps, this can take a while.)
    + If [Git] is setup and you do not have an SSH key-pair, one is generated
      and you are prompted to enter a password for the private key.
    + Wait for _Bench_ to setup the apps.  
      (Depending on the number and size of the activated apps, this can take a while.
      Usally a package manager like NPM or PIP is used to install a couple of apps,
      therefore, the internet connection is required in this phase two.)

* Run the `new-project.cmd` action to scaffold a project with [Yeoman]:
  Choose the _MdProc_ generator to build a [Markdown] documentation project.
* Run the `clonte-git-project.cmd` action to work on an existing project.

## Apps

_Bench_ downloads a selection of commandline tools and provides an isolated
path environment with all tools available.

Take a look at the [app registry](res/apps.md).

## Isolated Environment

The setup process of _Bench_ generates a file which is called `auto/env.cmd`.
This CMD script initializes the execution environment for programs to run inside of _Bench_.

The following environment variables are set by `auto/env.cmd`.

* `BENCH_HOME` is set to the root path of _Bench_
* `BENCH_PATH` is the list with the paths to the registered apps in _Bench_
* `USERNAME` is set to the config value `UserName` from `config.ps1`
* `USEREMAIL` is set to the config value `UserEmail`from `config.ps1`
* `HOMEDRIVE` is set to the drive letter of the config value `HomeDir`, which is `%BENCH_HOME%\home` per default
* `HOMEPATH` is set to the path of the config value `HomeDir`, which is `%BENCH_HOME%\home` per default
* `APPDATA` is set to `%HOMEDRIVE%%HOMEPATH%\AppData`
* `LOCALAPPDATA` is set to `%APPDATA%\Local`
* `PATH` is set to `%BENCH_PATH%;%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0`

## Actions

Actions are CMD scripts in the root folder of _Bench_ to quickly start working inside of the _Bench_ environment.

Every action runs `auto/env.cmd` first to initialize the isolated _Bench_ environment.

### `shell`

Open a _PowerShell_ prompt.

### `open-editor`

Open the app defined by the config value `EditorApp`, which is _Visual Studio Code_ by default.

Command line arguments given for `open-editor.cmd` are passed to the editor.

### `new-project`

Create a new project with _Yeoman_:

* create a new folder in the path, defined by the config value `ProjectRootDir`, which is `%BENCH_HOME%\projects` by default
* start [Yeoman] to bootstrap the new project
* put the new project folder under [Git] version control
* commit the initial project files

### `clone-git-project`

Clones a [Git] repository as a new project.

### `project-shell`

Open a _PowerShell_ prompt for a specific project.

The path to the project can be specified as a first command line argument to `project-shell.cmd`.
If no project path is specified, the project can be selected interactively.

### `project-editor`

Open the app defined by the config value `EditorApp`, which is _Visual Studio Code_ by default,
and pass the path of a specific project.

The path to the project can be specified as a first command line argument to `project-editor.cmd`.
If no project path is specified, the project can be selected interactively.

### `project-watch`

Runs the `watch` task for a specific project, if the project is automated with [Gulp] or [Grunt]. 

### `project-backup`

Saves a compressed copy of a specific project in the folder, specified by the config value `ProjectArchiveDir`,
which is `%BENCH_ROOT%\archive` by default.
The archive format can be specified with the config value `ProjectArchiveFormat` and can be every
filename extension supported by [7zip].

## Management and Upgrade

There are some management scripts in the `auto` folder to help you with changes, updates or an incomplete setup.

### `update-env`

If you have moved the _Bench_ directory, or you are using _Bench_ on a USB drive and it has a new drive letter,
you must update environment file `auto/env.cmd`.

The `update-env.cmd` script does exactly that.

### `bench-setup`

This script performs the following steps:

* Initializing the custom configuration if there is none
* Downloading missing app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `auto/env.cmd`
* Removing the bootstrap file `bench.bat`, if it is found

You need an internet connection for this script.

This script usally can be run repeatedly without any riscs.

### `bench-refresh`

If your installed apps are corrupted, or you want to update NPM or PIP packages,
you can run this script.
It performs the following steps:

* Removing all installed app files
* Downloading missing app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `auto/env.cmd`

You need an internet connection for this script.

### `bench-upgrade`

If you want to upgrade the whole _Bench_ environment, you can run this script.
It performs the following steps:

* If _Bench_ is a working copy of the _Bench_ GitHub repository, the system is updated with a _pull_ from master.
  Possibly changed files of the _Bench_ system itself are resetted.
* Deleting all downloaded app resources
* Removing all installed app files
* Downloading all required app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `auto/env.cmd`

If the internet connection is not stable, or some of the app resources are not available,
this script leaves you with a possible unusable environment.
But you can allways run `bench-setup.cmd` or `bench-refresh.cmd` in an attempt to repair the missing apps.

This script does not touch any user data in the _Bench_ home directory,
and it does not touch the custom configuration (`config.ps1` and `apps.md`) in the _Bench_ root folder either.
But upgrading a _Bench_ environment is not well tested yet, and is not advised without a backup.

[Yeoman]: http://yeoman.io "The web's scaffolding tool for modern web apps"
[Markdown]: https://daringfireball.net/projects/markdown/
[Git]: https://git-scm.com
[Gulp]: http://gulpjs.com
[Grunt]: http://gruntjs.com
[7zip]: http://7-zip.org