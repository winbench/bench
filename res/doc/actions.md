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

[Yeoman]: http://yeoman.io "The web's scaffolding tool for modern web apps"
[Git]: https://git-scm.com
[Gulp]: http://gulpjs.com
[Grunt]: http://gruntjs.com
[7zip]: http://7-zip.org