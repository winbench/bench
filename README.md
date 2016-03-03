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

* Microsoft Windows 7 or higher (with at least [PowerShell 3])
* Internet access for HTTP/HTTPS (possibly via proxy, but without credentials)
* The Windows user account does _not_ need admin priviledges
* A harddrive or a removable drive with at least 2 GB free memory

## Install

* Create a folder on a drive with at least 2 GB space, maybe call it `bench`.
* Save the bootstrap file [bench-install.bat](https://github.com/mastersign/bench/raw/master/res/bench-install.bat)
  inside this folder.
* Run the bootstrap file.
  (You probably have to open the properties dialog for the downloaded file, and allow the execution first.)
* Follow the instructions ...
    + Enter your name and email address for your [Git] identity.
    + Configure the HTTP proxy server in the `config.ps1` if you are behind a proxy firewall.
    + Maybe activate or deactivate some app groups or single apps in the `config.ps1`.  
      (To work with _MdProc_ on [Markdown] projects, activate the `Markdown` app group,
      by removing the leading `#` from the line with `Activate-App Markdown`.)
    + Wait for _Bench_ to  download resources.  
      (Depending on the number and size of the activated apps, this can take a while.)
    + If [Git] is set up and you do not have an SSH key-pair, one is generated
      and you are prompted to enter a password for the private key.
    + Wait for _Bench_ to setup the apps.  
      (Depending on the number and size of the activated apps, this can take a while.
      Usally a package manager like NPM or PIP is used to install a couple of apps,
      therefore, the internet connection is required in this phase too.)
* Maybe, run the `clone-git-project.cmd` action to work on an existing project.

## Quickstart with a new Markdown project

If you activated the `Markdown` app group, you can go ahead with these steps: 

* Run the `new-project.cmd` action to scaffold a project with [Yeoman]
* Choose the _MdProc_ generator to build a [Markdown] documentation project
* Choose one of the different [Markdown] project templates (`Demo`, `Minimal`, `Personal Log`, ...)
* Follow further instructions
* Take a look at the README file in the scaffolded project

## Apps

_Bench_ downloads a selection of commandline tools and applications,
and provides an isolated path environment with all tools available.

Take a look at the [app registry](res/apps.md) to check out the prepared apps.
You can add your own apps to the _Bench_ environment by editing the `apps.md`
in the root folder of _Bench_.

## Isolated Environment

The setup process of _Bench_ generates a file which is called `auto/env.cmd`.
This CMD script initializes the execution environment for programs to run inside of _Bench_.

The following environment variables are set by `auto/env.cmd`.

* `BENCH_HOME` is set to the root path of _Bench_ (e.g. `D:\bench`)
* `BENCH_APPS` is set to the root path of the _Bench_ apps, which is `%BENCH_HOME%\lib` per default
* `BENCH_PATH` is the list with the paths to the registered apps in _Bench_
* `BENCH_DRIVE` is set to the drive letter `%BENCH_HOME%` (e.g. `D:`)  
* `USERNAME` is set to the config value `UserName` from `config.ps1`
* `USEREMAIL` is set to the config value `UserEmail`from `config.ps1`
* `USERPROFILE` and `HOME` are set to `HomeDir`, which is `%BENCH_HOME%\home` per default
* `HOMEDRIVE` is set to the drive letter of `%HOME%` (e.g. `D:`)
* `HOMEPATH` is set to the path  without the drive letter of `%HOME%` (e.g. `\bench\home`)
* `APPDATA` is set to `%HOME%\AppData\Roaming`
* `LOCALAPPDATA` is set to `%HOME%\AppData\Local`
* `TEMP` and `TMP` are set to the path of the config value `TempDir`, which is `%BENCH_HOME%\tmp` per default
* `PATH` is set to `%BENCH_PATH%;%SystemRoot%;%SystemRoot%\System32;%SystemRoot%\System32\WindowsPowerShell\v1.0`

## Actions

Actions are CMD scripts in the `actions` folder of _Bench_ to quickly start working inside of the _Bench_ environment.

Every action runs `auto/env.cmd` first to initialize the isolated _Bench_ environment.

### Command Lines

#### `bench-cmd`

Start the Windows _CMD_.

Command line arguments given to `bench-cmd.cmd` are passed to _CMD_.

#### `bench-ps`

Start the _PowerShell_.

Command line arguments given to `bench-ps.cmd` are passed to _PowerShell_.

#### `bench-bash`

Start the _BASH_ of the _Git_ distribution.

Command line arguments given to `bench-bash.cmd` are passed to _BASH_.

### Without Project Context

#### `open-editor`

Open the app, defined by the config value `EditorApp`, which is _Visual Studio Code_ by default.

Command line arguments given for `open-editor.cmd` are passed to the editor.

#### `new-project`

Create a new project with _Yeoman_:

* create a new folder in the path, defined by the config value `ProjectRootDir`, which is `%BENCH_HOME%\projects` by default
* start [Yeoman] to bootstrap the new project
* put the new project folder under [Git] version control
* commit the initial project files

#### `clone-git-project`

Clones a [Git] repository as a new project.

### With Project Context

#### `project-ps`

Open a _PowerShell_ prompt for a specific project.

The path to the project can be specified as a first command line argument to `project-ps.cmd`.
If no project path is specified, the project can be selected interactively.

#### `project-editor`

Open the app defined by the config value `EditorApp`, which is _Visual Studio Code_ by default,
and pass the path of a specific project.

The path to the project can be specified as a first command line argument to `project-editor.cmd`.
If no project path is specified, the project can be selected interactively.

#### `project-watch`

Runs the `watch` task for a specific project, if the project is automated with [Gulp] or [Grunt]. 

#### `project-backup`

Saves a compressed copy of a specific project in the folder, specified by the config value `ProjectArchiveDir`,
which is `%BENCH_ROOT%\archive` by default.
The archive format can be specified with the config value `ProjectArchiveFormat` and can be every
filename extension supported by [7zip].

## Management and Upgrade

The _Bench_ system can be managed and upgraded with the `bench-ctl.cmd` script in the `actions` folder of _Bench_.

### Execution Modes

Depending on the number of arguments, given to `bench-ctl`, the script runs in one of the following modes.

#### Interactive `bench-ctl`

You can run `bench-ctl.cmd` without any arguments or by just double clicking it in the Windows Explorer,
to run it in interactive mode.
In the interactive mode, you are presented with all available tasks
and you can select a task to run, by typing an indicator key.

#### Automation `bench-ctl <task>`

You can specifiy a task as the first argument to `bench-ctl.cmd`.
The specified task is executed immediatly without further interaction with the user.

### Tasks

#### `update-env`

If you have moved the _Bench_ directory, or you are using _Bench_ on a USB drive and it has a new drive letter,
you must update the environment file `auto/env.cmd` and the launcher shortcuts.

The `update-env` task does exactly that.

#### `setup`

If you changed your `config.ps1` by activating more apps or altered
some settings, and also if an error occured during the initial installation of _Bench_,
the `setup` task is the right choice to complete the app installation.

This tasks performs the following steps:

* Initializing the custom configuration, if there is none
* Downloading missing app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `auto/env.cmd`
* Updating the launcher shortcuts
* Removing the bootstrap file `bench.bat`, if it is found

Usually, you need an internet connection for this script.

This script usally can be run repeatedly without any riscs.

#### `download`

If you want to download missing application resources without installing
any application in the _Bench_ environment, use this script.
It performs the following steps:

* Initializing the custom configuration, if there is none
* Download missing app resources

Running `download` prior to `setup` or `refresh` prefetches app resources to
reduce the time required later-on by `setup` or `refresh`.

However, be aware, that downloading the app resources is mostly not sufficient
to run the tasks `setup` or `refresh` afterwards, without an internet connection.
This is because NPM and PIP packages are not downloaded as app resources,
but downloaded by the package manager during installation.

#### `refresh`

If your installed apps are corrupted, or you want to update
NPM or PIP packages, you can run this script.
It performs the following steps:

* Removing all installed app files
* Downloading missing app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `auto/env.cmd`
* Updating the launcher shortcuts

Usually, you need an internet connection for this script.

#### `renew`

If you want to start fresh with your app selection, you can run this task.
It performs the following steps:

* Removing all downloaded app resource
* Removing all installed app files
* Downloading missing app resources
* Installing the apps in the _Bench_ environment
* Updating the _Bench_ environment file `auto/env.cmd`
* Updating the launcher shortcuts

You need an internet connection for this script.

#### `upgrade`

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
But you can allways run  the `setup` and `refresh` tasks in an attempt to repair the missing apps.

This script does not touch any user data in the _Bench_ home directory,
and it does not touch the custom configuration (`config.ps1` and `apps.md`) in the _Bench_ root folder either.
But upgrading a _Bench_ environment is not well tested yet, and is not advised without a backup.

[Yeoman]: http://yeoman.io "The web's scaffolding tool for modern web apps"
[Markdown]: https://daringfireball.net/projects/markdown/
[Git]: https://git-scm.com
[Gulp]: http://gulpjs.com
[Grunt]: http://gruntjs.com
[7zip]: http://7-zip.org
[PowerShell 3]: https://www.microsoft.com/en-us/download/details.aspx?id=34595
